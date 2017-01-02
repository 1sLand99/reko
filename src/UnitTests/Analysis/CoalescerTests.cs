#region License
/* 
 * Copyright (C) 1999-2016 John K�ll�n.
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
using Reko.Analysis;
using Reko.Core.Code;
using Reko.Core.Types;
using Reko.UnitTests.Mocks;
using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Reko.UnitTests.Analysis
{
	[TestFixture]
	public class CoalescerTests : AnalysisTestBase
	{
		protected override void RunTest(Program program, TextWriter fut)
		{
            IImportResolver importResolver = null;
            var listener = new FakeDecompilerEventListener();
			DataFlowAnalysis dfa = new DataFlowAnalysis(program, importResolver, listener);
			var ssts = dfa.UntangleProcedures();
			
			foreach (Procedure proc in program.Procedures.Values)
			{
                var sst = ssts.Single(s => s.SsaState.Procedure == proc);
				SsaState ssa = sst.SsaState;
				
                ConditionCodeEliminator cce = new ConditionCodeEliminator(ssa, program.Platform);
				cce.Transform();
				DeadCode.Eliminate(ssa);

                ValuePropagator vp = new ValuePropagator(program.Architecture, ssa, listener);
				vp.Transform();
				DeadCode.Eliminate(ssa);
				Coalescer co = new Coalescer(ssa);
				co.Transform();

				ssa.Write(fut);
				proc.Write(false, fut);
				fut.WriteLine();

                ssa.CheckUses(s => Assert.Fail(s));
            }
        }

		[Test]
        [Category(Categories.IntegrationTests)]
		public void Coa3Converge()
		{
			RunFileTest_x86_real("Fragments/3converge.asm", "Analysis/Coa3Converge.txt");
		}

		[Test]
        [Category(Categories.IntegrationTests)]
        public void CoaAsciiHex()
		{
			RunFileTest_x86_real("Fragments/ascii_hex.asm", "Analysis/CoaAsciiHex.txt");
		}

		[Test]
        [Category(Categories.IntegrationTests)]
		public void CoaDataConstraint()
        {
			RunFileTest_x86_real("Fragments/data_constraint.asm", "Analysis/CoaDataConstraint.txt");
		}

		[Test]
        [Category(Categories.IntegrationTests)]
		public void CoaMoveChain()
        {
			RunFileTest_x86_real("Fragments/move_sequence.asm", "Analysis/CoaMoveChain.txt");
		}

		[Test]
        [Ignore(Categories.AnalysisDevelopment)]
        [Category(Categories.AnalysisDevelopment)]
        public void CoaFactorialReg()
		{
			RunFileTest_x86_real("Fragments/factorial_reg.asm", "Analysis/CoaFactorialReg.txt");
		}

		[Test]
		public void CoaMemoryTest()
		{
			RunFileTest_x86_real("Fragments/simple_memoperations.asm", "Analysis/CoaMemoryTest.txt");
		}

		[Test]
		public void CoaSmallLoop()
		{
			RunFileTest_x86_real("Fragments/small_loop.asm", "Analysis/CoaSmallLoop.txt");
		}

		[Test]
        [Ignore("scanning-development")]
        public void CoaAddSubCarries()
		{
			RunFileTest_x86_real("Fragments/addsubcarries.asm", "Analysis/CoaAddSubCarries.txt");
		}

		[Test]
		public void CoaConditionals()
		{
			RunFileTest_x86_real("Fragments/multiple/conditionals.asm", "Analysis/CoaConditionals.txt");
		}

		[Test]
		public void CoaSliceReturn()
		{
			RunFileTest_x86_real("Fragments/multiple/slicereturn.asm", "Analysis/CoaSliceReturn.txt");
		}

		[Test]
		public void CoaReg00002()
		{
			RunFileTest_x86_real("Fragments/regression00002.asm", "Analysis/CoaReg00002.txt");
		}

		[Test]
		public void CoaWhileGoto()
		{
			RunFileTest_x86_real("Fragments/while_goto.asm", "Analysis/CoaWhileGoto.txt");
		}

        [Test]
        public void CoaSideEffectCalls()
        {
            RunFileTest_x86_real("Fragments/multiple/sideeffectcalls.asm", "Analysis/CoaSideEffectCalls.txt");
        }

        [Test]
        public void CoaCallUses()
        {
            var m = new ProcedureBuilder("foo");
            var r2 = m.Register(2);
            var r3 = m.Register(3);
            var r4 = m.Register(4);
            m.Assign(r4, m.Fn(r2));
            m.Call(r3, 4);
            m.Return();
            RunFileTest(m, "Analysis/CoaCallUses.txt");
        }

        [Test]
        public void CoaCallCallee()
        {
            var m = new ProcedureBuilder("foo");
            var r2 = m.Register(2);
            var r3 = m.Register(3);
            m.Assign(r3, m.Fn(r2));
            m.Assign(r3, m.IAdd(r3, 4));
            m.Call(r3, 4);
            m.Return();
            RunFileTest(m, "Analysis/CoaCallCallee.txt");
        }
    }
}
