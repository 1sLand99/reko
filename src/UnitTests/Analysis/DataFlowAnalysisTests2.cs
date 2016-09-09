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

using NUnit.Framework;
using Reko.Analysis;
using Reko.Arch.X86;
using Reko.Core;
using Reko.Core.Expressions;
using Reko.Core.Serialization;
using Reko.Core.Types;
using Reko.UnitTests.Mocks;
using Reko.UnitTests.TestCode;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Reko.UnitTests.Analysis
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class DataFlowAnalysisTests2
    {
        private MockRepository mr;
        private ProgramBuilder pb;
        private IImportResolver importResolver;

        [SetUp]
        public void Setup()
        {
            mr = new MockRepository();
            this.importResolver = mr.Stub<IImportResolver>();
        }

        private void GivenProgram(ProgramBuilder pb)
        {
        }

        private void AssertProgram(string sExp, Program program)
        {
            var sw = new StringWriter();
            var sep = "";
            foreach (var proc in program.Procedures.Values)
            {
                sw.Write(sep);
                proc.Write(false, sw);
                sep = "===" + Environment.NewLine;
            }
            var sActual = sw.ToString();
            if (sExp != sActual) 
                Debug.WriteLine(sActual);
            Assert.AreEqual(sExp, sw.ToString());
        }

        private void AssertProgramFlow(string sExp, Program program, ProgramDataFlow flow)
        {
            var sw = new StringWriter();
            var sep = "";
            foreach (var proc in program.Procedures.Values)
            {
                sw.Write(sep);
                var pflow = flow.ProcedureFlows[proc];
                sw.WriteLine("// Trashed   {0}", string.Join(", ", pflow.Trashed.OrderBy(s => s.ToString())));
                sw.WriteLine("// Preserved {0}", string.Join(", ", pflow.Preserved.OrderBy(s => s.ToString())));
                sw.WriteLine("// Used      {0}", string.Join(", ", pflow.BitsUsed.Select(s => string.Format("({0}:{1})", s.Key, s.Value)).OrderBy(s => s)));
                proc.Write(false, sw);
                sep = "===" + Environment.NewLine;
            }
            var sActual = sw.ToString();
            if (sExp != sActual)
                Debug.WriteLine(sActual);
            Assert.AreEqual(sExp, sw.ToString());
        }

        private void RunTest(Program program, TextWriter writer)
        {
            mr.ReplayAll();

            var dfa = new DataFlowAnalysis(program, importResolver, new FakeDecompilerEventListener());
            dfa.AnalyzeProgram2();
            foreach (var proc in program.Procedures.Values)
            {
                proc.Write(false, writer);
            }
        }

        [Test]
        [Category(Categories.UnitTests)]
        public void DfaReg00282()
        {
            AnalysisTestBase.RunTest_x86_real("Fragments/regressions/r00282.asm", RunTest, "Analysis/DfaReg00282.txt");
        }

        [Test]
        [Category(Categories.UnitTests)]
        public void Dfa2_Simple()
        {
            var pb = new ProgramBuilder(new FakeArchitecture());
            pb.Add("test", m=>
                {
                    var r1 = m.Reg32("r1", 1);
                    var r2 = m.Reg32("r2", 2);
                    m.Assign(r1, m.LoadDw(m.Word32(0x010000)));
                    m.Assign(r2, m.LoadDw(m.Word32(0x010004)));
                    m.Store(m.Word32(0x010008), m.IAdd(r1, r2));
                    m.Return();
                });
            mr.ReplayAll();

            var dfa = new DataFlowAnalysis(pb.BuildProgram(), importResolver, new FakeDecompilerEventListener());
            dfa.AnalyzeProgram2();
            var sExp = @"// test
// Return size: 0
void test()
test_entry:
	// succ:  l1
l1:
	Mem4[0x00010008:word32] = Mem0[0x00010000:word32] + Mem0[0x00010004:word32]
	return
	// succ:  test_exit
test_exit:
";
            AssertProgram(sExp, pb.Program);
        }

        [Test]
        public void Dfa2_StackArgs()
        {
            var pb = new ProgramBuilder(new FakeArchitecture());
            pb.Add("test", m =>
            {
                var sp = m.Register(m.Architecture.StackRegister);
                var r1 = m.Reg32("r1", 1);
                var r2 = m.Reg32("r2", 2);
                m.Assign(sp, m.Frame.FramePointer);
                m.Assign(r1, m.LoadDw(m.IAdd(sp, 4)));
                m.Assign(r2, m.LoadDw(m.IAdd(sp, 8)));
                m.Assign(r1, m.IAdd(r1, r2));
                m.Store(m.Word32(0x010008), r1);
                m.Return();
            });
            var dfa = new DataFlowAnalysis(pb.BuildProgram(), null, new FakeDecompilerEventListener());
            dfa.AnalyzeProgram2();
            var sExp = @"// test
// Return size: 0
void test(word32 dwArg04, word32 dwArg08)
test_entry:
	// succ:  l1
l1:
	Mem7[0x00010008:word32] = dwArg04 + dwArg08
	return
	// succ:  test_exit
test_exit:
";
            AssertProgram(sExp, pb.Program);
        }

        private ProcedureBase GivenFunction(string name, RegisterStorage ret, params object [] args)
        {
            var frame = pb.Program.Architecture.CreateFrame();
            Identifier idRet = null;
            if (ret != null)
            {
                idRet = frame.EnsureRegister(ret);
            }
            List<Identifier> parameters = new List<Identifier>();
            foreach (int offset in args)
            {
                parameters.Add(frame.EnsureStackArgument(offset, pb.Program.Architecture.WordWidth));
            }
            var proc = new ExternalProcedure(name, FunctionType.Func(
                idRet, 
                parameters.ToArray()));
            return proc;
        }

        [Test]
        public void Dfa2_CallProc()
        {
            pb = new ProgramBuilder();
            pb.Add("test", m =>
            {
                var sp = m.Register(m.Architecture.StackRegister);

                var fooProc = GivenFunction("foo", m.Architecture.GetRegister(1), 4, 8);
                m.Assign(sp, m.Frame.FramePointer);
                m.Assign(sp, m.ISub(sp, 4));
                m.Store(sp, 2);
                m.Assign(sp, m.ISub(sp, 4));
                m.Store(sp, 1);
                m.Call(fooProc, 4);
                m.Assign(sp, m.IAdd(sp, 8));
                m.Return();
            });

            var dfa = new DataFlowAnalysis(pb.BuildProgram(), null, new FakeDecompilerEventListener());
            dfa.AnalyzeProgram2();
            var sExp = @"// test
// Return size: 0
void test()
test_entry:
	// succ:  l1
l1:
	foo(0x00000001, 0x00000002)
	return
	// succ:  test_exit
test_exit:
";
            AssertProgram(sExp, pb.Program);
        }

        [Test]
        public void Dfa2_UserDefinedStackArgs()
        {
            var arch = new X86ArchitectureFlat32();
            var pb = new ProgramBuilder(arch);
            var test = pb.Add(
                new Procedure_v1
                {
                    CSignature = "void test(int a, int b)"
                },
                m => {
                    var sp = m.Register(m.Architecture.StackRegister);
                    var r1 = m.Reg32("r1", 1);
                    var r2 = m.Reg32("r2", 2);
                    var fp = m.Frame.FramePointer;
                    m.Assign(r1, m.LoadDw(m.IAdd(fp, 4)));
                    m.Assign(r2, m.LoadDw(m.IAdd(fp, 8)));
                    m.Assign(r1, m.IAdd(r1, r2));
                    m.Store(m.Word32(0x010008), r1);
                    m.Return();
                });
            var program = pb.BuildProgram();
            var platform = new FakePlatform(null, arch);
            platform.Test_CreateImplicitArgumentRegisters = () =>
                new HashSet<RegisterStorage>();
            platform.Test_CreateProcedureSerializer = (t, d) =>
            {
                var typeLoader = new TypeLibraryDeserializer(platform, false, new TypeLibrary());
                return new X86ProcedureSerializer((IntelArchitecture)program.Architecture, typeLoader, "");
            };

            var importResolver = MockRepository.GenerateStub<IImportResolver>();
            importResolver.Replay();
            program.Platform = platform;
            var dfa = new DataFlowAnalysis(program, importResolver, new FakeDecompilerEventListener());
            dfa.AnalyzeProgram2();
            var sExp = @"// test
// Return size: 4
void test(int32 a, int32 b)
test_entry:
	// succ:  l1
l1:
	Mem9[0x00010008:word32] = a + b
	return
	// succ:  test_exit
test_exit:
";
            AssertProgram(sExp, pb.Program);
        }

        [Test]
        public void Dfa2_Untangle_CallChain()
        {
            pb = new ProgramBuilder();
            pb.Add("main", m =>
            {
                var r1 = m.Register(1);
                m.Assign(r1, m.LoadDw(m.Word32(0x123400)));
                m.Call("level1", 0);
                m.Store(m.Word32(0x123400), r1);
                m.Return();
            });
            pb.Add("level1", m =>
            {
                m.Call("level2", 0);
                m.Return();
            });
            pb.Add("level2", m =>
            {
                var r1 = m.Register(1);
                var r2 = m.Register(2);
                m.Assign(r2, -1);
                m.Assign(r1, m.IAdd(r1, 1));
                m.Return();
            });
            var program = pb.BuildProgram();
            mr.ReplayAll();

            var dfa = new DataFlowAnalysis(program, importResolver, new FakeDecompilerEventListener());
            dfa.AnalyzeProgram2();

            var sExp =
            #region Expected
@"// main
// Return size: 0
define main
main_entry:
	// succ:  l1
l1:
	word32 r1_3
	call level1 (retsize: 0;)
		uses: Mem0[0x00123400:word32]
		defs: r1_3,r2_4
	Mem5[0x00123400:word32] = r1_3
	return
	// succ:  main_exit
main_exit:
===
// level1
// Return size: 0
define level1
level1_entry:
	// succ:  l1
l1:
	word32 r1_2
	word32 r2_3
	call level2 (retsize: 0;)
		uses: r1
		defs: r1_2,r2_3
	return
	// succ:  level1_exit
level1_exit:
===
// level2
// Return size: 0
define level2
level2_entry:
	// succ:  l1
l1:
	word32 r2_1 = 0xFFFFFFFF
	word32 r1_3 = r1 + 0x00000001
	return
	// succ:  level2_exit
level2_exit:
";
            #endregion
            AssertProgram(sExp, pb.Program);
        }

        [Test(Description = "One level calls the next")]
        public void Dfa2_StackArgs_Nested()
        {
            pb = new ProgramBuilder();
            pb.Add("main", m =>
            {
                var r1 = m.Register(1);
                var sp = m.Frame.EnsureRegister(m.Architecture.StackRegister);
                m.Assign(sp, m.Frame.FramePointer);
                m.Assign(sp, m.ISub(sp, 4));
                m.Store(sp, m.LoadDw(m.Word32(0x123400)));
                m.Call("level1", 4);
                m.Assign(sp, m.IAdd(sp, 4));
                m.Store(m.Word32(0x123404), r1);
                m.Return();
            });
            pb.Add("level1", m =>
            {
                var r1 = m.Register(1);
                var sp = m.Frame.EnsureRegister(m.Architecture.StackRegister);
                m.Assign(sp, m.Frame.FramePointer);
                m.Assign(r1, m.LoadDw(m.IAdd(sp, 4)));
                m.Assign(sp, m.ISub(sp, 4));
                m.Store(sp, r1);
                m.Call("level2", 4);
                m.Assign(sp, m.IAdd(sp, 4));
                m.Return();
            });
            pb.Add("level2", m =>
            {
                var r1 = m.Register(1);
                var r2 = m.Register(2);
                var sp = m.Frame.EnsureRegister(m.Architecture.StackRegister);
                m.Assign(sp, m.Frame.FramePointer);
                m.Assign(r1, m.LoadDw(m.IAdd(sp, 4)));
                m.Assign(r1, m.IAdd(r1, 1));
                m.Return();
            });
            var program = pb.BuildProgram();
            mr.ReplayAll();

            var dfa = new DataFlowAnalysis(program, importResolver, new FakeDecompilerEventListener());
            dfa.AnalyzeProgram2();

            var sExp =
            #region Expected
@"// main
// Return size: 0
void main()
main_entry:
	// succ:  l1
l1:
	Mem8[0x00123404:word32] = level1(Mem0[0x00123400:word32])
	return
	// succ:  main_exit
main_exit:
===
// level1
// Return size: 0
word32 level1(word32 dwArg04)
level1_entry:
	// succ:  l1
l1:
	word32 r1_7 = level2(dwArg04)
	return r1_7
	// succ:  level1_exit
level1_exit:
===
// level2
// Return size: 0
word32 level2(word32 dwArg04)
level2_entry:
	// succ:  l1
l1:
	word32 r1_5 = dwArg04 + 0x00000001
	return r1_5
	// succ:  level2_exit
level2_exit:
";
            #endregion
            AssertProgram(sExp, pb.Program);
        }

        [Test]
        public void Dfa2_Byte_Arg()
        {
            pb = new ProgramBuilder();
            pb.Add("main", m =>
            {
                var r1 = m.Register(1);
                var sp = m.Frame.EnsureRegister(m.Architecture.StackRegister);
                m.Assign(sp, m.Frame.FramePointer);
                m.Assign(r1, m.LoadDw(m.IAdd(sp, 4)));
                m.Call("level1", 4);
                m.Return();
            });
            pb.Add("level1", m =>
            {
                var r1 = m.Register(1);
                var sp = m.Frame.EnsureRegister(m.Architecture.StackRegister);
                m.Assign(sp, m.Frame.FramePointer);
                m.Store(m.Word32(0x1234), m.Cast(PrimitiveType.Byte, r1));
                m.Return();
            });
            var program = pb.BuildProgram();
            mr.ReplayAll();

            var dfa = new DataFlowAnalysis(program, importResolver, new FakeDecompilerEventListener());
            dfa.AnalyzeProgram2();

            var sExp =
            #region Expected
@"// Trashed   r1
// Preserved r63
// Used      (Stack +0004:32)
// main
// Return size: 0
void main(word32 dwArg04)
main_entry:
	// succ:  l1
l1:
	level1(dwArg04)
	return
	// succ:  main_exit
main_exit:
===
// Trashed   
// Preserved r63
// Used      (r1:8)
// level1
// Return size: 0
void level1(word32 r1)
level1_entry:
	// succ:  l1
l1:
	Mem4[0x00001234:byte] = (byte) r1
	return
	// succ:  level1_exit
level1_exit:
";
            #endregion
            AssertProgramFlow(sExp, pb.Program, dfa.ProgramDataFlow);

        }
    }
}
