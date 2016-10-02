﻿#region License
/* 
 * Copyright (C) 1999-2016 Pavel Tomin.
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

using NUnit.Framework;
using Reko.Analysis;
using Reko.Core;
using Reko.Core.Expressions;
using Reko.Core.Serialization;
using Reko.Core.Types;
using Reko.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.IO;

namespace Reko.UnitTests.Analysis
{
    [TestFixture]
    public class IndirectCallRewriterTests : AnalysisTestBase
    {
        private FunctionType sig;
        private IDictionary<string, DataType> types;
        private Identifier eax;
        private Identifier ecx;
        private Identifier esp;

        [SetUp]
        public void Setup()
        {
            types = new Dictionary<string, DataType>();
            this.eax = new Identifier("eax", PrimitiveType.Word32, new RegisterStorage("eax", 0, 0, PrimitiveType.Word32));
            this.ecx = new Identifier("ecx", PrimitiveType.Word32, new RegisterStorage("ecx", 1, 0, PrimitiveType.Word32));
            this.esp = new Identifier("esp", PrimitiveType.Word32, new RegisterStorage("esp", 5, 0, PrimitiveType.Word32));
        }

        private void indirect_call_no_arguments(ProcedureBuilder m)
        {
            var esp = m.Frame.EnsureIdentifier(m.Architecture.StackRegister);
            var eax = m.Frame.EnsureIdentifier(this.eax.Storage);
            var ecx = m.Frame.EnsureIdentifier(this.ecx.Storage);

            m.Assign(esp, m.Frame.FramePointer);
            m.Assign(eax, m.LoadDw(m.IAdd(esp, 4)));
            m.Assign(ecx, m.LoadDw(eax));
            m.Call(m.LoadDw(ecx), 4);
            m.Return();
        }

        private void indirect_call_one_argument(ProcedureBuilder m)
        {
            var esp = m.Frame.EnsureIdentifier(m.Architecture.StackRegister);
            var eax = m.Frame.EnsureIdentifier(this.eax.Storage);
            var ecx = m.Frame.EnsureIdentifier(this.ecx.Storage);

            m.Assign(esp, m.Frame.FramePointer);
            m.Assign(eax, m.LoadDw(m.IAdd(esp, 4)));
            m.Assign(ecx, m.LoadDw(eax));
            m.Assign(esp, m.ISub(esp, 4));
            m.Store(esp, 0x000A);
            var c = m.Call(m.LoadDw(m.IAdd(ecx, 4)), 4);
            c.CallSite.StackDepthOnEntry = 8;
            m.Return();
        }

        private void indirect_call_two_arguments(ProcedureBuilder m)
        {
            var esp = m.Frame.EnsureIdentifier(m.Architecture.StackRegister);
            var eax = m.Frame.EnsureIdentifier(this.eax.Storage);
            var ecx = m.Frame.EnsureIdentifier(this.ecx.Storage);

            m.Assign(esp, m.Frame.FramePointer);
            m.Assign(eax, m.LoadDw(m.IAdd(esp, 4)));
            m.Assign(ecx, m.LoadDw(eax));
            m.Assign(esp, m.ISub(esp, 4));
            m.Store(esp, 0x000B);
            m.Assign(esp, m.ISub(esp, 4));
            m.Store(esp, 0x000A);
            var c = m.Call(m.LoadDw(m.IAdd(ecx, 8)), 4);
            c.CallSite.StackDepthOnEntry = 12;
            m.Return();
        }

        private void Given_Signature(DataType argType)
        {
            this.sig = new FunctionType(
                null,
                new Identifier(
                    "a", 
                    new Pointer(argType, 4),
                    new StackArgumentStorage(4, PrimitiveType.Word32)));
        }

        private void Given_Typedef(string name, DataType dt)
        {
            types[name] = dt;
        }

        private DataType Ptr32(DataType pointee)
        {
            return new Pointer(pointee, 4);
        }

        private DataType Int32()
        {
            return PrimitiveType.Int32;
        }

        private DataType FnPtr32(
            Identifier returnValue,
            params Identifier[] parameters)
        {
            return Ptr32(new FunctionType(returnValue, parameters));
        }

        private Identifier VoidId()
        {
            return null;
        }

        private Identifier StackId(int offset, DataType dt)
        {
            return new Identifier(
                "",
                dt,
                new StackArgumentStorage(offset, dt));
        }

        private DataType VtblStr(params DataType[] methods)
        {
            var vtbl_str = new StructureType();
            int offset = 0;
            foreach(var method in methods)
            {
                vtbl_str.Fields.Add(offset, method);
                offset += 4;
            }
            return new StructureType()
            {
                Fields =
                {
                    { 0, Ptr32(vtbl_str), "vtbl" },
                }
            };
        }

        private DataType TestStr()
        {
            var fn0000 = FnPtr32(
                VoidId());
            var fn0004 = FnPtr32(
                VoidId(),
                StackId(4, Int32()));
            var fn0008 = FnPtr32(
                VoidId(),
                StackId(4, Int32()),
                StackId(8, Int32()));
            return VtblStr(fn0000, fn0004, fn0008);
        }

        private DataType TestStrEcx()
        {
            var fn0000 = FnPtr32(
                eax,
                ecx,
                StackId(4, Int32()));
            var fn0004 = FnPtr32(
                eax,
                ecx,
                StackId(4, Int32()));
            var fn0008 = FnPtr32(
                eax,
                ecx,
                StackId(4, Int32()),
                StackId(8, Int32()));
            return VtblStr(fn0000, fn0004, fn0008);
        }

        private DataType TestStrNoFuncs()
        {
            return VtblStr(Ptr32(Int32()), Ptr32(Int32()), Ptr32(Int32()));
        }

        private void SetSignatures(Program program)
        {
            foreach (var proc in program.Procedures.Values)
            {
                proc.Signature = sig;
            }
        }

        private void InitProgram(Program program)
        {
            SetSignatures(program);
        }

        protected override void RunTest(Program program, TextWriter fut)
        {
            program.Platform = new FakePlatform(null, program.Architecture)
            {
                Test_CreateTrashedRegisters = () => 
                    new HashSet<RegisterStorage>()
                {
                    (RegisterStorage)this.eax.Storage,
                    (RegisterStorage)this.ecx.Storage,
                    program.Architecture.StackRegister,
                }
            };
            InitProgram(program);
            IImportResolver importResolver = null;
            var eventListener = new FakeDecompilerEventListener();

            var programFlow = new ProgramDataFlow();
            var addr = program.Procedures.Keys[0];
            var proc = program.Procedures.Values[0];
            var usb = new UserSignatureBuilder(program);
            usb.ApplySignatureToProcedure(addr, proc.Signature, proc);
            var sst = new SsaTransform(program, proc, importResolver, programFlow);
            var ssa = sst.Transform();
            var vp = new ValuePropagator(program.Architecture, ssa);
            vp.Transform();
            sst.RenameFrameAccesses = true;
            sst.Transform();
            var icrw = new IndirectCallRewriter(program, ssa, eventListener);
            icrw.Rewrite();

            ssa.Write(fut);
            ssa.Procedure.Write(false, fut);
            fut.WriteLine();
        }

        [Test]
        public void Icrw_NoArguments()
        {
            Given_Signature(TestStr());
            RunFileTest("Analysis/IcrwNoArguments.txt", indirect_call_no_arguments);
        }

        [Test]
        public void Icrw_InvalidArguments()
        {
            Given_Signature(TestStrEcx());
            RunFileTest("Analysis/IcrwInvalidArguments.txt", indirect_call_no_arguments);
        }

        [Test]
        public void Icrw_OneArgument()
        {
            Given_Signature(TestStr());
            RunFileTest("Analysis/IcrwOneArgument.txt", indirect_call_one_argument);
        }

        [Test]
        public void Icrw_OneArgumentNoFuncs()
        {
            Given_Signature(TestStrNoFuncs());
            RunFileTest("Analysis/IcrwOneArgumentNoFuncs.txt", indirect_call_one_argument);
        }

        [Test]
        public void Icrw_OneArgumentPassEcx()
        {
            Given_Signature(TestStrEcx());
            RunFileTest("Analysis/IcrwOneArgumentPassEcx.txt", indirect_call_one_argument);
        }

        [Test]
        public void Icrw_TwoArguments()
        {
            Given_Signature(TestStr());
            RunFileTest("Analysis/IcrwTwoArguments.txt", indirect_call_two_arguments);
        }

        [Test(Description = "If there are no virtual functions, don't rewrite the call")]
        public void Icrw_TwoArgumentsNoFuncs()
        {
            Given_Signature(TestStrNoFuncs());
            RunFileTest("Analysis/IcrwTwoArgumentsNoFuncs.txt", indirect_call_two_arguments);
        }
    }
}
