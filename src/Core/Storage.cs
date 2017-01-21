#region License
/* 
 * Copyright (C) 1999-2017 John K�ll�n.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using Reko.Core.Code;
using Reko.Core.Expressions;
using Reko.Core.Lib;
using Reko.Core.Machine;
using Reko.Core.Types;
using Reko.Core.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace Reko.Core
{
    /// <summary>
    /// Encapsulates architecture-dependent storage mechanisms for an identifier.
    /// </summary>
    public abstract class Storage
    {
        public Storage(string storageKind)
        {
            this.Kind = storageKind;
        }

        public string Kind { get; private set; }
        public StorageDomain Domain { get; set; }
        public ulong BitAddress { get; set; }
        public virtual ulong BitSize { get; set; }
        public string Name { get; protected set; }
        public int Number { get; protected set; }

        /// <summary>
        /// Returns the bit offset of <paramref name="storage"/> within this 
        /// storage, or -1 if <paramref name="storage"/> is not a part of 
        /// this storage.
        /// </summary>
        /// <param name="storage"></param>
        /// <returns>The offset in bits, or -1.</returns>
        public abstract int OffsetOf(Storage storage);
        public abstract T Accept<T>(StorageVisitor<T> visitor);
        public abstract T Accept<C, T>(StorageVisitor<C, T> visitor, C context);


        public abstract bool OverlapsWith(Storage that);
        /*
        {
            if (this.Domain == StorageDomain.Memory && 
                that.Domain == StorageDomain.Memory)
            {
                return true;
            }
            if (this.Domain != that.Domain || this.Number != that.Number)
                return false;
            var thisStart = this.BitAddress;
            var thisEnd = this.BitAddress + this.BitSize;
            var thatStart = that.BitAddress;
            var thatEnd = that.BitAddress + that.BitSize;
            return thisStart < thatEnd && thatStart < thisEnd;
        }
        */

        /// <summary>
        /// A storage <code>a</code> is said to cover a storage 
        /// <code>b</code> if they alias a location, but after 
        /// assigning a with b, some of the original contents of 
        /// a still peek through.
        /// For instance, the x86 register 'eax' covers the 'ah' register,
        /// but the 'ah' register doesn't cover 'eax'.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public abstract bool Covers(Storage that);

        public virtual bool Exceeds(Storage that)
        {
            throw new NotImplementedException(string.Format("Exceeds not implemented for {0}.", that.GetType().Name));
        }

        public virtual BitRange GetBitRange()
        {
            return new BitRange(0, (int)BitSize);
        }

        public virtual SerializedKind Serialize()
        {
            throw new NotImplementedException(this.GetType().Name + ".Serialize not implemented.");
        }

        public override string ToString()
        {
            StringWriter w = new StringWriter();
            Write(w);
            return w.ToString();
        }

        public abstract void Write(TextWriter writer);
    }

    public static class StorageEx
    {
        public static bool As<T>(this Storage self, out T t) where T : Storage
        {
            t = self as T;
            return t != null;
        }
    }

    public enum StorageDomain
    {
        None = -1,
        Register = 0,
        Stack = 4096,   // Few architectures have this many registers (fingers xD)
        Memory = 4097,
        Flags = 4100,
        Temporary = 8192,
    }

    /// <summary>
    /// A flag register is a special kind of register that can store one 
    /// or more bit registers.
    /// </summary>
    public class FlagRegister : RegisterStorage
    {
        public FlagRegister(string name, int flagDomain, PrimitiveType size) :
            base(
                name,
                flagDomain,
                0,
                size)
        {
        }

        public override T Accept<T>(StorageVisitor<T> visitor)
        {
            return visitor.VisitFlagRegister(this);
        }

        public override T Accept<C, T>(StorageVisitor<C, T> visitor, C context)
        {
            return visitor.VisitFlagRegister(this, context);
        }

        public override int OffsetOf(Storage storage)
        {
            if (storage is FlagRegister)
                return 0;
            else
                return -1;
        }
    }

    /// <summary>
    /// This class represents groups of bits stored in flag registers. 
    /// Typically, these are the Carry, Zero, Overflow etc flags that are set
    /// after ALU operations.
    /// </summary>
	public class FlagGroupStorage : Storage
    {
        public FlagGroupStorage(FlagRegister freg, uint grfMask, string name, DataType dataType) : base("FlagGroup")
        {
            this.Domain = freg.Domain;
            this.FlagRegister = freg;
            this.FlagGroupBits = grfMask;
            this.Name = name;
            this.DataType = dataType;
        }

        public FlagRegister FlagRegister { get; private set; }
        public uint FlagGroupBits { get; private set; }
        public DataType DataType { get; private set; }

        public override T Accept<T>(StorageVisitor<T> visitor)
        {
            return visitor.VisitFlagGroupStorage(this);
        }

        public override T Accept<C, T>(StorageVisitor<C, T> visitor, C context)
        {
            return visitor.VisitFlagGroupStorage(this, context);
        }

        public override bool Covers(Storage sThat)
        {
            var that = sThat as FlagGroupStorage;
            if (that == null || this.FlagRegister != that.FlagRegister)
                return false;
            return (this.FlagGroupBits | that.FlagGroupBits) == this.FlagGroupBits;
        }

        public override bool Equals(object obj)
        {
            FlagGroupStorage that = obj as FlagGroupStorage;
            if (that == null)
                return false;
            return this.FlagGroupBits == that.FlagGroupBits;
        }

        public override bool Exceeds(Storage sThat)
        {
            var that = sThat as FlagGroupStorage;
            if (that == null || this.FlagRegister != that.FlagRegister)
                return false;
            return (this.FlagGroupBits & ~that.FlagGroupBits) != 0;
        }

        public IEnumerable<uint> GetFlagBitMasks()
        {
            for (uint bitMask = 1; bitMask <= FlagGroupBits; bitMask <<= 1)
            {
                if ((FlagGroupBits & bitMask) != 0)
                {
                    yield return bitMask;
                }
            }
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ FlagGroupBits.GetHashCode();
        }

        public override int OffsetOf(Storage stgSub)
        {
            FlagGroupStorage f = stgSub as FlagGroupStorage;
            if (f == null)
                return -1;
            return ((f.FlagGroupBits & FlagGroupBits) != 0) ? 0 : -1;
        }

        public override bool OverlapsWith(Storage sThat)
        {
            var that = sThat as FlagGroupStorage;
            if (that == null || this.FlagRegister != that.FlagRegister)
                return false;
            return (this.FlagGroupBits & that.FlagGroupBits) != 0;
        }

        public override SerializedKind Serialize()
        {
            return new FlagGroup_v1(Name);
        }

        public override void Write(TextWriter writer)
        {
            writer.Write(Name);
        }
    }

    /// <summary>
    /// Used to model locations in the x87 FPU stack.
    /// </summary>
    public class FpuStackStorage : Storage
    {
        public FpuStackStorage(int depth, DataType dataType) : base("FpuStack")
        {
            this.FpuStackOffset = depth;
            this.DataType = dataType;
            if (FpuStackOffset >= 0)
            {
                Name = string.Format("FPU +{0}", FpuStackOffset);
            }
            else
            {
                Name = string.Format("FPU -{0}", -FpuStackOffset);
            }
        }

        public override ulong BitSize { get { return (ulong) DataType.BitSize; } }
        public DataType DataType { get; private set; }
        public int FpuStackOffset { get; private set; }

        public override T Accept<T>(StorageVisitor<T> visitor)
        {
            return visitor.VisitFpuStackStorage(this);
        }

        public override T Accept<C, T>(StorageVisitor<C, T> visitor, C context)
        {
            return visitor.VisitFpuStackStorage(this, context);
        }

        public override bool Covers(Storage that)
        {
            return that is FpuStackStorage &&
                this.Number == that.Number;
        }

        public override bool Equals(object obj)
        {
            FpuStackStorage that = obj as FpuStackStorage;
            if (that == null)
                return false;
            return this.FpuStackOffset == that.FpuStackOffset;
        }

        public override bool Exceeds(Storage that)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ FpuStackOffset.GetHashCode();
        }

        public override int OffsetOf(Storage stgSub)
        {
            return -1;
        }

        public override bool OverlapsWith(Storage sThat)
        {
            var that = sThat as FpuStackStorage;
            return that != null &&
                this.FpuStackOffset == that.FpuStackOffset;
        }

        public override void Write(TextWriter writer)
        {
            if (FpuStackOffset >= 0)
            {
                writer.Write("FPU +{0}", FpuStackOffset);
            }
            else
            {
                writer.Write("FPU -{0}", -FpuStackOffset);
            }
        }
    }

    /// <summary>
    /// Storage is some unspecified part of global memory.
    /// </summary>
    public class MemoryStorage : Storage
    {
        public MemoryStorage(string name, StorageDomain domain) : base(name)
        {
            this.Name = name;
            this.Domain = domain;
        }

        public override T Accept<T>(StorageVisitor<T> visitor)
        {
            return visitor.VisitMemoryStorage(this);
        }

        public override T Accept<C, T>(StorageVisitor<C, T> visitor, C context)
        {
            return visitor.VisitMemoryStorage(this, context);
        }

        public override bool Covers(Storage that)
        {
            return true;
        }

        public override bool Exceeds(Storage that)
        {
            return false;
        }

        public override int OffsetOf(Storage stgSub)
        {
            return -1;
        }

        public override bool OverlapsWith(Storage that)
        {
            return that is MemoryStorage;
        }

        public override void Write(TextWriter writer)
        {
            writer.Write(Name);
        }

        public static MemoryStorage Instance { get; private set; }

        static MemoryStorage()
        {
            Instance = new MemoryStorage("Global", StorageDomain.Memory);
        }
    }

    /// <summary>
    /// Storage for registers or other identifiers that are live-out of a procedure.
    /// </summary>
    public class OutArgumentStorage : Storage
    {
        public OutArgumentStorage(Identifier originalId) : base("out")
        {
            this.OriginalIdentifier = originalId;
        }

        public Identifier OriginalIdentifier { get; private set; }

        public override T Accept<T>(StorageVisitor<T> visitor)
        {
            return visitor.VisitOutArgumentStorage(this);
        }

        public override T Accept<C, T>(StorageVisitor<C, T> visitor, C context)
        {
            return visitor.VisitOutArgumentStorage(this, context);
        }

        public override bool Covers(Storage that)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            OutArgumentStorage oas = obj as OutArgumentStorage;
            if (oas == null)
                return false;
            return oas.OriginalIdentifier.Equals(OriginalIdentifier);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ OriginalIdentifier.GetHashCode();
        }

        public override int OffsetOf(Storage stgSub)
        {
            return -1;
        }

        public override bool OverlapsWith(Storage that)
        {
            throw new NotImplementedException();
        }

        public override SerializedKind Serialize()
        {
            return OriginalIdentifier.Storage.Serialize();
        }

        public override void Write(TextWriter writer)
        {
            writer.Write("Out:");
            OriginalIdentifier.Storage.Write(writer);
        }
    }

    /// <summary>
    /// Used to represent a machine register.
    /// </summary>
	public class RegisterStorage : Storage
    {
        private RegisterStorage(string kind) : base(kind)
        {
        }

        public RegisterStorage(string regName, int number, uint bitAddress, PrimitiveType dt) : base("Register")
        {
            this.Name = regName;
            this.Number = number;
            this.BitAddress = bitAddress;
            this.DataType = dt;
            this.Domain = (StorageDomain)(number + (int)StorageDomain.Register);
            int bitSize = dt.BitSize;
            if (bitSize == 64)
            {
                BitMask = ~0ul;
            }
            else
            {
                BitMask = ((1ul << bitSize) - 1) << (int)bitAddress;
            }
        }

        public static RegisterStorage Reg8(string name, int number)
        {
            return new RegisterStorage(name, number, 0, PrimitiveType.Byte);
        }

        public static RegisterStorage Reg16(string name, int number)
        {
            return new RegisterStorage(name, number, 0, PrimitiveType.Word16);
        }

        public static RegisterStorage Reg32(string name, int number)
        {
            return new RegisterStorage(name, number, 0, PrimitiveType.Word32);
        }

        public static RegisterStorage Reg64(string name, int number)
        {
            return new RegisterStorage(name, number, 0, PrimitiveType.Word64);
        }

        public override ulong BitSize {
            get { return (ulong)DataType.BitSize; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Bitmask used to extract subregister values from a larger backing register.
        /// </summary>
        public ulong BitMask { get; private set; }

        /// <summary>
        /// The size and domain of the register.
        /// </summary>
        /// <remarks>
        /// General-purpose registers can use the Domain.Word </remarks>
        public PrimitiveType DataType { get; private set; }

        public override T Accept<T>(StorageVisitor<T> visitor)
        {
            return visitor.VisitRegisterStorage(this);
        }

        public override T Accept<C, T>(StorageVisitor<C, T> visitor, C context)
        {
            return visitor.VisitRegisterStorage(this, context);
        }

        public override bool Covers(Storage sThat)
        {
            var that = sThat as RegisterStorage;
            if (that == null || that.Domain != this.Domain)
                return false;
            return (this.BitMask | that.BitMask) == this.BitMask;
        }

        public override bool Exceeds(Storage sThat)
        {
            var that = sThat as RegisterStorage;
            if (that == null || that.Domain != this.Domain)
                return false;
            return (this.BitMask & ~that.BitMask) != 0;
        }

        public override bool Equals(object obj)
        {
            var that = obj as RegisterStorage;
            if (that == null)
                return false;
            return this.Domain == that.Domain &&
                this.BitAddress == that.BitAddress &&
                this.BitSize == that.BitSize;
        }

        public override int GetHashCode()
        {
            return (int)Domain * 17 ^ BitAddress.GetHashCode() ^ BitSize.GetHashCode();
        }

        public override BitRange GetBitRange()
        {
            int bitOffset = (int)BitAddress;
            int bitSize = (int)BitSize;
            return new BitRange(bitOffset, bitOffset + bitSize);
        }

        /// <summary>
        /// Returns true if this register is strictly contained by reg2.
        /// </summary>
        /// <param name="reg1"></param>
        /// <param name="reg2"></param>
        /// <returns></returns>
        public bool IsSubRegisterOf(RegisterStorage reg2)
        {
            if (this.BitSize >= reg2.BitSize)
                return false;
            return this.OverlapsWith(reg2);
        }

        public override int OffsetOf(Storage stgSub)
        {
            var regSub = stgSub as RegisterStorage;
            if (regSub == null)
                return -1;
            if (!OverlapsWith(regSub))
                return -1;
            return (int)stgSub.BitAddress;
        }

        public override bool OverlapsWith(Storage sThat)
        {
            var that = sThat as RegisterStorage;
            if (that == null || this.Domain != that.Domain)
                return false;
            var thisStart = this.BitAddress;
            var thisEnd = this.BitAddress + this.BitSize;
            var thatStart = that.BitAddress;
            var thatEnd = that.BitAddress + that.BitSize;
            return thisStart < thatEnd && thatStart < thisEnd;
        }

        /// <summary>
		/// Given a register, sets/resets the bits corresponding to the register
		/// and any other registers it aliases.
		/// </summary>
		/// <param name="iReg">Register to set</param>
		/// <param name="bits">BitSet to modify</param>
		/// <param name="f">truth value to set</param>
		public virtual void SetAliases(ISet<RegisterStorage> bitset, bool f)
        {
            if (f)
                bitset.Add(this);
            else
                bitset.Remove(this);
        }

        public override SerializedKind Serialize()
        {
            return new Register_v1(Name);
        }

        public virtual void SetRegisterStateValues(Expression value, bool isValid, Dictionary<Storage, Expression> ctx)
        {
            ctx[this] = value;
        }

        public int SubregisterOffset(RegisterStorage subReg)
        {
            var sub = subReg as RegisterStorage;
            if (sub != null)
            {
                if (Number == sub.Number)
                    return 0;
            }
            return -1;
        }

        public override void Write(TextWriter writer)
        {
            writer.Write(Name);
        }

        public static RegisterStorage None { get { return none; } }

        private static RegisterStorage none =
            new RegisterStorage("None")
            {
                Name = "None",
                Number = -1,
                Domain = StorageDomain.None,
                BitAddress = 0,
                DataType = PrimitiveType.Create(Types.Domain.Any, 0)
            };

        public Expression GetSlice(Expression value)
        {
            var c = value as Constant;
            if (c != null && c.IsValid)
            {
                var newValue = (c.ToUInt64() & this.BitMask) >> (int)this.BitAddress;
                return Constant.Create(this.DataType, newValue);
            }
            else
                return Constant.Invalid;
        }
    }

	public class SequenceStorage : Storage
	{
		public SequenceStorage(Storage head, Storage tail) 
			: base("Sequence")		
		{
			this.Head = head;
			this.Tail = tail;
		}

        public Storage Head { get; private set; }
        public Storage Tail { get; private set; }

        public override T Accept<T>(StorageVisitor<T> visitor)
        {
            return visitor.VisitSequenceStorage(this);
        }

        public override T Accept<C, T>(StorageVisitor<C, T> visitor, C context)
        {
            return visitor.VisitSequenceStorage(this, context);
        }

        public override bool Covers(Storage that)
        {
            if (this == that)
                return true;
            if (this.Head.Domain == that.Domain)
            {
                return this.Head.Covers(that);
            }
            if (this.Tail.Domain == that.Domain)
            {
                return this.Tail.Covers(that);
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            SequenceStorage ss = obj as SequenceStorage;
            if (ss == null)
                return false;
            return Head.Equals(ss.Head) && Tail.Equals(ss.Tail);
        }

        public override bool Exceeds(Storage that)
        {
            if (this == that)
                return true;
            if (this.Head.Domain == that.Domain)
            {
                return this.Head.Exceeds(that);
            }
            if (this.Tail.Domain == that.Domain)
            {
                return this.Tail.Exceeds(that);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ Head.GetHashCode() ^ (3 * Tail.GetHashCode());
        }

		public override int OffsetOf(Storage stgSub)
		{
			int off = Tail.OffsetOf(stgSub);
			if (off != -1)
				return off;
			off = Head.OffsetOf(stgSub);
			if (off != -1)
				return off + (int)Tail.BitSize;
			return -1;
		}

        public override bool OverlapsWith(Storage that)
        {
            if (this == that)
                return true;
            if (this.Head.Domain == that.Domain)
            {
                return this.Head.OverlapsWith(that);
            }
            if (this.Tail.Domain == that.Domain)
            {
                return this.Tail.OverlapsWith(that);
            }
            return false;
        }

        public override SerializedKind Serialize()
        {
            return new SerializedSequence(this);
        }

        public override void Write(TextWriter writer)
        {
            writer.Write("Sequence {0}:{1}", Head.Name, Tail.Name);
        }
    }

    public abstract class StackStorage : Storage
    {
        public StackStorage(string kind, int cbOffset, DataType dt)
            : base(kind)
        {
            this.StackOffset = cbOffset;
            this.DataType = dt;
            this.Domain = StorageDomain.Stack + cbOffset;
        }

        public DataType DataType { get; private set; }

        /// <summary>
        /// Offset from stack pointer as it was when the procedure was entered.
        /// </summary>
        /// <remarks>
        /// If the architecture stores the return address on the stack, the return address will be at offset 0 and
        /// any stack arguments will have offsets > 0. If the architecture passes the return address in a
        /// register or a separate return stack, there may be stack arguments with offset 0. Depending on which
        /// direction the stack grows, there may be negative stack offsets for parameters, although most popular
        /// general purpose processors (x86, PPC, m68K) grown their stacks down toward lower memory addresses.
        /// </remarks>
        public int StackOffset { get; private set; }

        public override bool OverlapsWith(Storage that)
        {
            if (this.Domain != that.Domain || this.Number != that.Number)
                return false;
            var thisStart = this.BitAddress;
            var thisEnd = this.BitAddress + this.BitSize;
            var thatStart = that.BitAddress;
            var thatEnd = that.BitAddress + that.BitSize;
            return thisStart < thatEnd && thatStart < thisEnd;
        }
    }

    public class StackArgumentStorage : StackStorage
    {
        public StackArgumentStorage(int cbOffset, DataType dataType) : base("Stack", cbOffset, dataType)
        {
            this.BitSize = (uint) dataType.BitSize;
        }

        public override T Accept<T>(StorageVisitor<T> visitor)
        {
            return visitor.VisitStackArgumentStorage(this);
        }

        public override T Accept<C, T>(StorageVisitor<C, T> visitor, C context)
        {
            return visitor.VisitStackArgumentStorage(this, context);
        }

        public override bool Covers(Storage that)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            StackArgumentStorage sas = obj as StackArgumentStorage;
            if (sas == null)
                return false;
            return StackOffset == sas.StackOffset;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ StackOffset;
        }

        public override int OffsetOf(Storage stgSub)
        {
            StackArgumentStorage arg = stgSub as StackArgumentStorage;
            if (arg == null)
                return -1;
            if (arg.StackOffset >= StackOffset && arg.StackOffset + arg.DataType.Size <= StackOffset + DataType.Size)
                return (arg.StackOffset - StackOffset) * DataType.BitsPerByte;
            return -1;
        }

        public override SerializedKind Serialize()
        {
            return new StackVariable_v1();
        }

        public override void Write(TextWriter writer)
        {
            writer.Write("{0} +{1:X4}", Kind, StackOffset);
        }
    }

    public class StackLocalStorage : StackStorage
    {
        public StackLocalStorage(int cbOffset, DataType dataType)
            : base("Local", cbOffset, dataType)
        {
        }

        public override T Accept<T>(StorageVisitor<T> visitor)
        {
            return visitor.VisitStackLocalStorage(this);
        }

        public override T Accept<C, T>(StorageVisitor<C, T> visitor, C context)
        {
            return visitor.VisitStackLocalStorage(this, context);
        }

        public override bool Covers(Storage that)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            StackLocalStorage sas = obj as StackLocalStorage;
            if (sas == null)
                return false;
            return StackOffset == sas.StackOffset;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ StackOffset;
        }


        public override int OffsetOf(Storage stgSub)
        {
            StackLocalStorage local = stgSub as StackLocalStorage;
            if (local == null)
                return -1;
            if (local.StackOffset >= StackOffset && local.StackOffset + local.DataType.Size <= StackOffset + DataType.Size)
                return (local.StackOffset - StackOffset) * DataType.BitsPerByte;
            return -1;
        }

        public override void Write(TextWriter writer)
        {
            writer.Write("{0} -{1:X4}", base.Kind, Math.Abs(StackOffset));
        }
    }

    /// <summary>
    /// Temporary storage is used for expressing intermediate results that become exposed
    /// when complex machine instructions are broken down into simpler RTL sequences.
    /// </summary>
    /// <remarks>
    /// An example is the x86 instruction <code>shr ds:[0x41],3</code> which is 
    /// rewritten into rtl as:
    /// <code>
    /// tmp = Mem0[0x0041] >> 3
    /// Mem[0x0041] = tmp
    /// SCZ = Cond(tmp)
    /// </code>
    /// </remarks>
	public class TemporaryStorage : Storage
	{
		public TemporaryStorage(string name, int number, DataType dt) : base("Temporary")
        {
            Domain = StorageDomain.Temporary + number;
            Name = name;
            DataType = dt;
        }

        public DataType DataType { get; private set; }

        public override T Accept<T>(StorageVisitor<T> visitor)
        {
            return visitor.VisitTemporaryStorage(this);
        }

        public override T Accept<C, T>(StorageVisitor<C, T> visitor, C context)
        {
            return visitor.VisitTemporaryStorage(this, context);
        }

        public override bool Covers(Storage that)
        {
            return ReferenceEquals(this, that);
        }

        public override bool Exceeds(Storage that)
        {
            return false;
        }

        public override int OffsetOf(Storage stgSub)
        {
            return -1;
        }

        public override bool OverlapsWith(Storage that)
        {
            return ReferenceEquals(this, that);
        }

        public override void Write(TextWriter writer)
        {
            writer.Write(Name);
        }
    }
}
