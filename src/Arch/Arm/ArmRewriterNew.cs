﻿#region License
/* 
 * Copyright (C) 1999-2017 John Källén.
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

using Reko.Core;
using Reko.Core.NativeInterface;
using Reko.Core.Rtl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Reko.Arch.Arm
{
    public class ArmRewriterNew : IEnumerable<RtlInstructionCluster>
    {
        private EndianImageReader rdr;
        private IStorageBinder frame;
        private IRewriterHost host;

        public ArmRewriterNew(Arm32ProcessorArchitecture arch, EndianImageReader rdr, ArmProcessorState state, IStorageBinder frame, IRewriterHost host)
        {
            this.rdr = rdr;
            this.frame = frame;
            this.host = host;
        }

        public IEnumerator<RtlInstructionCluster> GetEnumerator()
        {
            var bytes = rdr.Bytes;
            var offset = (int)rdr.Offset;
            var addr = rdr.Address.ToLinear();
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Enumerator : IEnumerator<RtlInstructionCluster>
        {
            private INativeRewriter native;
            private byte[] bytes;
            private GCHandle hBytes;
            private RtlEmitter m;
            private RtlNativeEmitter rtlEmitter;
            private ArmNativeRewriterHost host;
            private IntPtr iRtlEmitter;
            private IntPtr iHost;

            public Enumerator(ArmRewriterNew outer)
            {
                this.bytes = outer.rdr.Bytes;
                ulong addr = outer.rdr.Address.ToLinear();
                this.hBytes = GCHandle.Alloc(bytes, GCHandleType.Pinned);

                this.m = new RtlEmitter(new List<RtlInstruction>());

                this.rtlEmitter = new RtlNativeEmitter(m, outer.host);
                this.host = new ArmNativeRewriterHost(outer.frame, outer.host, rtlEmitter);

                this.iRtlEmitter = GetCOMInterface(rtlEmitter, IID_IRtlEmitter);
                this.iHost = GetCOMInterface(host, IID_INativeRewriterHost);
                this.native = CreateNativeRewriter(hBytes.AddrOfPinnedObject(), bytes.Length, (int)outer.rdr.Offset, addr, iRtlEmitter, iHost);
            }

            public RtlInstructionCluster Current { get; private set; }

            object IEnumerator.Current { get { return Current; } }

            private IntPtr GetCOMInterface(object o, Guid iid)
            {
                var iUnknown = Marshal.GetIUnknownForObject(o);
                IntPtr intf;
                var hr2 = Marshal.QueryInterface(iUnknown, ref iid, out intf);
                return intf;
            }

            public void Dispose()
            {
                if (this.native != null)
                {
                    int n = native.GetCount();
                    Marshal.ReleaseComObject(this.native);
                    this.native = null;
                }
                if (iHost != null)
                {
                    Marshal.Release(iHost);
                }
                if (iRtlEmitter != null)
                { 
                Marshal.Release(iRtlEmitter);
            }
                if (this.hBytes != null && this.hBytes.IsAllocated)
                {
                    this.hBytes.Free();
                }
            }

            public bool MoveNext()
            {
                m.Instructions = new List<RtlInstruction>();
                int n = native.GetCount();
                if (native.Next() == 1)
                    return false;
                this.Current = this.rtlEmitter.ExtractCluster();
                return true;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        static ArmRewriterNew()
        {
            IID_INativeRewriterHost = typeof(INativeRewriterHost).GUID;
            IID_IRtlEmitter = typeof(IRtlNativeEmitter).GUID;
        }

        private static Guid IID_INativeRewriterHost;
        private static Guid IID_IRtlEmitter;

        [DllImport("ArmNative.dll",CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateNativeRewriter")]
        public static extern INativeRewriter CreateNativeRewriter(IntPtr rawbytes, int length, int offset, ulong address, IntPtr rtlEmitter, IntPtr host);
    }
}
