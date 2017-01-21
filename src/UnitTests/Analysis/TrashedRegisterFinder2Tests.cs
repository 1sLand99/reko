﻿#region License
/* 
 * Copyright (C) 1999-2016 John Källén.
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

using Reko.Analysis;
using Reko.Core;
using Reko.Core.Lib;
using Reko.Core.Services;
using Reko.Core.Types;
using Reko.UnitTests.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Rhino.Mocks;
using Reko.Core.Expressions;
using Reko.Core.Serialization;
using Reko.Core.Code;

namespace Reko.UnitTests.Analysis
{
    [TestFixture]
    [Ignore(Categories.WorkInProgress)]
    public class TrashedRegisterFinder2Tests
    {
        private MockRepository mr;
        private ProgramBuilder progBuilder;
        private ExternalProcedure fnExit;
        private IPlatform platform;
        private IImportResolver importResolver;
        private Program program;
        private ProgramDataFlow dataFlow;
        private StringBuilder sbExpected;

        [SetUp]
        public void Setup()
        {
            this.mr = new MockRepository();
            this.platform = mr.Stub<IPlatform>();
            this.importResolver = mr.Stub<IImportResolver>();
            importResolver.Replay();

            this.sbExpected = new StringBuilder();
            this.progBuilder = new ProgramBuilder();
            this.fnExit = new ExternalProcedure(
                "exit",
                FunctionType.Action(new Identifier("code", PrimitiveType.Int32, new StackArgumentStorage(4, PrimitiveType.Int32))),
                new ProcedureCharacteristics
                {
                    Terminates = true,
                });
            this.fnExit.Signature.ReturnAddressOnStack = 4;
        }

        private void Expect(string fnName, string preserved, string trashed, string consts)
        {
            fnName = string.Format("== {0} ====", fnName);
            sbExpected.AppendLine(String.Join(Environment.NewLine, new[] { fnName, preserved, trashed, consts }));
        }

        private void Given_PlatformTrashedRegisters(params RegisterStorage[] regs)
        {
            platform.Stub(p => p.CreateTrashedRegisters()).Return(regs.ToHashSet());
        }

        private void AddProcedure(string fnName, Action<ProcedureBuilder> builder)
        {
            progBuilder.Add(fnName, builder);
        }

        public void RunTest()
        {
            progBuilder.ResolveUnresolved();
            progBuilder.Program.Platform = platform;
            mr.ReplayAll();

            this.program = progBuilder.Program;
            this.dataFlow = new ProgramDataFlow(program);
            var sscf = new SccFinder<Procedure>(new ProcedureGraph(program), ProcessScc);
            foreach (var procedure in program.Procedures.Values)
            {
                sscf.Find(procedure);
            }
            var trf = new TrashedRegisterFinder(program, program.Procedures.Values, dataFlow, NullDecompilerEventListener.Instance);
            trf.Compute();

            var sw = new StringWriter();
            foreach (var procedure in program.Procedures.Values)
            {
                var flow = dataFlow[procedure];
                sw.WriteLine("== {0} ====", procedure.Name);
                sw.Write("Preserved: ");
                sw.WriteLine(string.Join(",", flow.Preserved.OrderBy(p => p.ToString())));
                sw.Write("Trashed: ");
                sw.WriteLine(string.Join(",", flow.Trashed.OrderBy(p => p.ToString())));
                if (flow.Constants.Count > 0)
                {
                    sw.Write("Constants: ");
                    sw.Write(string.Join(
                        ",",
                        flow.Constants
                            .OrderBy(kv => kv.Key.ToString())
                            .Select(kv => string.Format(
                                "{0}:{1}", kv.Key, kv.Value))));
                }
                sw.WriteLine();
            }
            var sExp = sbExpected.ToString();
            var sActual = sw.ToString();
            if (sActual != sExp)
            {
                foreach (var proc in program.Procedures.Values)
                {
                    Debug.Print("------");
                    proc.Dump(true);
                }
                Debug.WriteLine(sActual);
                Assert.AreEqual(sExp, sActual);
            }
        }

        private void ProcessScc(IList<Procedure> scc)
        {
            foreach (var proc in scc)
            {
                var sst = new SsaTransform(
                    program,
                    proc,
                    new HashSet<Procedure>(),
                    importResolver,
                    dataFlow);
                sst.Transform();
                var vp = new ValuePropagator(program.Architecture, sst.SsaState, NullDecompilerEventListener.Instance);
                vp.Transform();
            }
        }

        [Test]
        public void TrfSimple()
        {
            Expect("TrfSimple", "Preserved: r63", "Trashed: r1", "Constants: r1:0x0000002A");
            AddProcedure("TrfSimple", m =>
            {
                var r1 = m.Register("r1");
                m.Assign(r1, m.Word32(42));
                m.Return();
            });
            RunTest();
        }

        [Test(Description = "Tests that registers pushed on stack are preserved")]
        public void TrfStackPreserved()
        {
            Expect(
                "TrfStackPreserved",
                "Preserved: r1,r63",
                "Trashed: ",
                "");
            AddProcedure("TrfStackPreserved", m =>
            {
                var sp = m.Frame.EnsureRegister(m.Architecture.StackRegister);
                var r1 = m.Register("r1");
                m.Assign(sp, m.Frame.FramePointer);
                m.Assign(sp, m.ISub(sp, 4));
                m.Store(sp, r1);        // push r1

                m.Assign(r1, m.LoadDw(m.Word32(0x123400)));
                m.Store(m.Word32(0x123400), r1);

                m.Assign(r1, m.LoadDw(sp)); // pop r1
                m.Assign(sp, m.IAdd(sp, 4));
                m.Return();
            });
            RunTest();
        }

        [Test(Description = "Tests that constants are discovered")]
        public void TrfConstants()
        {
            Expect("TrfConstants", "Preserved: r63", "Trashed: ds", "Constants: ds:0x0C00");
            AddProcedure("TrfConstants", m =>
            {
                var sp = m.Frame.EnsureRegister(m.Architecture.StackRegister);
                var ds = m.Reg16("ds", 10);
                m.Assign(sp, m.Frame.FramePointer);
                m.Assign(sp, m.ISub(sp, 2));
                m.Store(sp, m.Word16(0x0C00));
                m.Assign(ds, m.LoadW(sp));
                m.Assign(sp, m.IAdd(sp, 2));
                m.Return();
            });
            RunTest();
        }

        [Test(Description = "Constant in one branch, not constant in the other")]
        public void TrfConstNonConst()
        {
            Expect("TrfConstNonConst", "Preserved: r63", "Trashed: cl,cx", "");
            AddProcedure("TrfConstNonConst", m =>
            {
                var ax = m.Frame.EnsureRegister(new RegisterStorage("ax", 0, 0, PrimitiveType.Word16));
                var cl = m.Frame.EnsureRegister(new RegisterStorage("cl", 9, 0, PrimitiveType.Byte));
                var cx = m.Frame.EnsureRegister(new RegisterStorage("cx", 1, 0, PrimitiveType.Byte));
                m.BranchIf(m.Eq0(ax), "zero");

                m.Assign(cl, 0);
                m.Assign(cx, m.Dpb(cx, cl, 0));
                m.Return();

                m.Label("zero");
                m.Assign(cx, m.LoadW(ax));
                m.Return();
            });
            RunTest();
        }

        [Test(Description = "Same constant in both branches")]
        [Ignore("Allowing this in Phi functions broke lots of unit tests.")]
        public void TrfConstConst()
        {
            var sExp =
@"Preserved: ax
Trashed: cl,cx
Constants: cl:0x00
";
            AddProcedure(sExp, m =>
            {
                var ax = m.Frame.EnsureRegister(new RegisterStorage("ax", 0, 0, PrimitiveType.Word16));
                var cl = m.Frame.EnsureRegister(new RegisterStorage("cl", 9, 0, PrimitiveType.Byte));
                var cx = m.Frame.EnsureRegister(new RegisterStorage("cx", 1, 0, PrimitiveType.Byte));
                m.BranchIf(m.Eq0(ax), "zero");
                m.Assign(cl, 0);
                m.Assign(cx, m.Dpb(cx, cl, 0));
                m.Goto("done");

                m.Label("zero");
                m.Assign(cl, 0);
                m.Assign(cx, m.Dpb(cx, cl, 0));
                
                m.Label("done");
                m.Return();
            });
            RunTest();
        }

        [Test(Description="Tests propagation between caller and callee.")]
        [Category(Categories.UnitTests)]
        public void TrfSubroutine_WithRegisterParameters()
        {
            Expect(
                "Addition",
                "Preserved: r63",
                "Trashed: r1",
                "");

            // Subroutine does a small calculation in registers
            AddProcedure("Addition", m =>
            {
                var r1 = m.Register(1);
                var r2 = m.Register(2);
                Given_PlatformTrashedRegisters();
                m.Assign(r1, m.IAdd(r1, r2));
                m.Return();
            });

            Expect(
                "main",
                "Preserved: r63",
                "Trashed: r1,r2",
                "");

            AddProcedure("main", m =>
            {
                var r1 = m.Register(1);
                var r2 = m.Register(2);
                m.Assign(r2, m.LoadDw(m.IAdd(r1, 4)));
                m.Assign(r1, m.LoadDw(m.IAdd(r1, 8)));
                m.Call("Addition", 4);
                m.Store(m.Word32(0x123000), r1);
                m.Return();
            });

            RunTest();
        }

        [Test(Description = "Tests detection of trashed variables in the presence of recursion")]
        public void TrfRecursion()
        {
            Expect(
                "fact",
                "Preserved: r2,r63",
                "Trashed: r1",
                "Constants: r1:<invalid>");
            AddProcedure("fact", m =>
            {
                Given_PlatformTrashedRegisters();

                var fp = m.Frame.FramePointer;
                var r1 = m.Register(1);
                var r2 = m.Register(2);
                m.Store(m.ISub(fp, 4), r2);     // save r2
                m.BranchIf(m.Le(r2, 2), "m2Base");

                m.Label("m1Recursive");
                m.Assign(r2, m.ISub(r2, 1));
                m.Call("fact", 0);
                m.Assign(r2, m.LoadDw(m.ISub(fp, 4)));
                m.Assign(r1, m.IMul(r1, r2));   // r1 clobbered as it is the return value.
                m.Goto("m3Done");

                m.Label("m2Base");  // Base case just returns 1.
                m.Assign(r1, 1);

                m.Label("m3Done");
                m.Assign(r2, m.LoadDw(m.ISub(fp, 4)));    // restore r2
                m.Return();
            });
            RunTest();
        }

        [Test(Description = "Test that functions that don't return don't affect register state")]
        public void TrfNonReturningProcedure()
        {
            Expect(
                "callExit",
                "Preserved: ",
                "Trashed: ",
                "");
            AddProcedure("callExit", m =>
            {
                var sp = m.Frame.EnsureIdentifier(m.Architecture.StackRegister);
                var r1 = m.Reg32("r1", 1);
                m.Label("m1");
                m.Assign(sp, m.Frame.FramePointer);
                m.Assign(r1, m.LoadDw(m.IAdd(sp, 4)));
                m.Assign(sp, m.ISub(sp, 4));
                m.Store(sp, r1);
                m.Call(fnExit, 4);
                // No return, so registers are not affected.
            });
            RunTest();
        }

        [Test(Description = "Only registers modified on paths that reach the exit affect register state")]
        public void TrfBranchedNonReturningProcedure()
        {
            AddProcedure("callExitBranch", m =>
            {
                var sp = m.Frame.EnsureIdentifier(m.Architecture.StackRegister);
                var r1 = m.Reg32("r1", 1);
                var r2 = m.Reg32("r2", 2);
                m.Label("m1");
                m.Assign(sp, m.Frame.FramePointer);
                m.Assign(r1, m.LoadDw(m.IAdd(sp, 4)));
                m.BranchIf(m.Eq0(r1), "m3return");

                m.Label("m2exit");
                m.Assign(r2, 3);
                m.Assign(sp, m.ISub(sp, 4));
                m.Store(sp, r2);
                m.Call(fnExit, 4);
                m.ExitThread();         // Never reaches end.

                m.Label("m3return");
                m.Return();
            });
            Expect("callExitBranch", "Preserved: r63", "Trashed: r1", "Constants: r1:<invalid>");

            RunTest();
        }

        [Test(Description="Respect user-provided signatures")]
        public void TrfUserSignature()
        {
            AddProcedure("fnSig", m =>
            {
                var sp = m.Frame.EnsureIdentifier(m.Architecture.StackRegister);
                var r1 = m.Reg32("r1", 1);
                var r2 = m.Reg32("r2", 2);
                var r3 = m.Reg32("r3", 3);
                m.Assign(sp, m.Frame.FramePointer);
                m.Store(m.ISub(sp, 4), r3);
                m.Assign(r2, m.LoadDw(m.Word32(0x123400)));
                m.Assign(r3, m.LoadDw(m.ISub(sp, 4)));
                m.Return();

                m.Procedure.Signature = FunctionType.Func(
                    new Identifier("", PrimitiveType.Word32, r1.Storage),
                    new Identifier("arg1", PrimitiveType.Word32, r1.Storage));
                });
            Expect("fnSig", "Preserved: ", "Trashed: r1", "");
            RunTest();
        }

        [Test(Description = "Exercises a self-recursive function")]
        public void TrfRecursive()
        {
            Expect("fnSig", "Preserved: r1,r63", "Trashed: ", "");
            AddProcedure("fnSig", m =>
            {
                var sp = m.Frame.EnsureIdentifier(m.Architecture.StackRegister);
                Given_PlatformTrashedRegisters((RegisterStorage)sp.Storage);

                var r1 = m.Reg32("r1", 1);


                m.Assign(sp, m.Frame.FramePointer); // establish frame
                m.Assign(sp, m.ISub(sp, 4));        // preserve r1
                m.Store(sp, r1);
                m.BranchIf(m.Eq0(r1), "m3");

                m.Label("m2");
                m.Store(m.Word32(0x123400), r1);    // do something stupid
                m.Assign(r1, m.ISub(r1, 1));
                m.Call("fnSig", 0);                 // recurse

                m.Label("m3");
                m.Assign(r1, m.LoadDw(sp));
                m.Assign(sp, m.IAdd(sp, 4));
                m.Return();
            });
            RunTest();
        }

        [Test]
        public void TrfFpuReturn()
        {
            Expect(
                "TrfFpuReturn",
                "Preserved: ",
                "Trashed: FPU -1,Top",
                "Constants: FPU -1:2.0,Top:0xFF");

            AddProcedure("TrfFpuReturn", m =>
            {
                var ST = new MemoryIdentifier("ST", PrimitiveType.Pointer32, new MemoryStorage("x87Stack", StorageDomain.Register + 400));
                var Top = m.Frame.EnsureRegister(new RegisterStorage("Top", 76, 0, PrimitiveType.Byte));

                m.Assign(Top, 0);
                m.Assign(Top, m.ISub(Top, 1));
                m.Store(ST, Top, Constant.Real64(2.0));
                m.Return();
            });
            RunTest();
        }

        [Test]
        public void TrfFpuReturnTwoValues()
        {
            Expect(
                "TrfFpuReturnTwoValues",
                "Preserved: ",
                "Trashed: FPU -1,FPU -2,Top",
                "Constants: FPU -1:2.0,FPU -2:1.0,Top:0xFE");

            AddProcedure("TrfFpuReturnTwoValues", m =>
            {
                var ST = new MemoryIdentifier("ST", PrimitiveType.Pointer32, new MemoryStorage("x87Stack", StorageDomain.Register + 400));
                var Top = m.Frame.EnsureRegister(new RegisterStorage("Top", 76, 0, PrimitiveType.Byte));

                m.Assign(Top, 0);
                m.Assign(Top, m.ISub(Top, 1));
                m.Store(ST, Top, Constant.Real64(2.0));
                m.Assign(Top, m.ISub(Top, 1));
                m.Store(ST, Top, Constant.Real64(1.0));
                m.Return();
            });
            RunTest();
        }

        [Test(Description = "Pops three values off FPU stack and places one back.")]
        public void TrfFpuMultiplyAdd()
        {
            Expect(
                "TrfFpuMultiplyAdd",
                "Preserved: ",
                "Trashed: FPU +1,FPU +2,Top",
                "Constants: Top:0x02");
            AddProcedure("TrfFpuMultiplyAdd", m =>
            {
                var ST = new MemoryIdentifier("ST", PrimitiveType.Pointer32, new MemoryStorage("x87Stack", StorageDomain.Register + 400));
                var Top = m.Frame.EnsureRegister(new RegisterStorage("Top", 76, 0, PrimitiveType.Byte));
                var dt = PrimitiveType.Real64;
                m.Assign(Top, 0);
                m.Store(ST, m.IAdd(Top, 1), m.FAdd(
                    m.Load(ST, dt, m.IAdd(Top, 1)),
                    m.Load(ST, dt, Top)));
                m.Assign(Top, m.IAdd(Top, 1));
                m.Store(ST, m.IAdd(Top, 1), m.FAdd(
                    m.Load(ST, dt, m.IAdd(Top, 1)),
                    m.Load(ST, dt, Top)));
                m.Assign(Top, m.IAdd(Top, 1));

                m.Return();
            });
            RunTest();
        }
    }
}
