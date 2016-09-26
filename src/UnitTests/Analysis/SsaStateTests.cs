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
using Reko.Core;
using Reko.Core.Expressions;
using Reko.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Reko.UnitTests.Analysis
{
    [TestFixture]
    public class SsaStateTests
    {
        private SsaState ssa;

        [SetUp]
        public void Setup()
        {
            this.ssa = null;
        }

        private void BuildTest(Action<ProcedureBuilder> builder)
        {
            var pb = new ProcedureBuilder();
            builder(pb);
            var dflow = new ProgramDataFlow();
            var sst = new SsaTransform(pb.Architecture, pb.Procedure, null, dflow);
            this.ssa = sst.Transform();
        }

        private Block FindBlock(string blockName)
        {
            return ssa.Procedure.ControlGraph.Blocks.Single(b => b.Name == blockName);
        }

        private void AssertOutput(string sExp, Dictionary<Block, Expression[]> dict)
        {
            var sb = new StringBuilder();
            foreach (var de in dict.OrderBy(d => d.Key.Name))
            {
                sb.AppendFormat(
                    "{0}: [{1}]", 
                    de.Key,
                    string.Join(",", de.Value.Select(i => i.ToString())));
                sb.AppendLine();
            }
            if (sExp != sb.ToString())
            {
                sb.AppendLine("------");
                ssa.Procedure.Write(false, new StringWriter(sb));
                Debug.WriteLine(sb.ToString());
                Assert.AreEqual(sExp, sb.ToString());
            }
        }

        [Test(Description = "A block with no predecessors should have no phis")]
        public void SsaState_GenerateStrips_NoPredecessors()
        {
            BuildTest(m =>
            {
                m.Label("bloq");
                m.Return();
            });
            var block = FindBlock("bloq");

            var dict = this.ssa.PredecessorPhiIdentifiers(block);
            Assert.AreEqual(0, dict.Count);
        }

        [Test(Description = "A block with no predecessors should have no phis")]
        public void SsaState_GenerateStrips_IfThen()
        {
            BuildTest(m =>
            {
                var r0 = m.Reg32("r0", 0);
                m.Label("m1");
                m.BranchIf(m.Ne0(r0), "mbloq");

                m.Label("m2");
                m.Assign(r0, 1);

                m.Label("mbloq");
                m.Store(m.Word32(0x123400), r0);
                m.Return();
            });
            var block = FindBlock("mbloq");

            var dict = this.ssa.PredecessorPhiIdentifiers(block);
            var sExp =
            #region Expected
@"m1: [r0]
m2: [r0_2]
";
            #endregion
            AssertOutput(sExp, dict);
        }

        [Test(Description = "A block with no predecessors should have no phis")]
        public void SsaState_GenerateStrips_IfThenElseif()
        {
            BuildTest(m =>
            {
                var r0 = m.Reg32("r0", 0);
                m.Label("m1");
                m.BranchIf(m.Ge(r0, 0), "m3");

                m.Label("m2");
                m.Assign(r0, 1);
                m.Goto("mbloq");

                m.Label("m3");
                m.BranchIf(m.Ge(r0, 10), "m5");

                m.Label("m4");
                m.Assign(r0, 2);
                m.Goto("mbloq");

                m.Label("m5");
                m.Assign(r0, 3);

                m.Label("mbloq");
                m.Store(m.Word32(0x123400), r0);
                m.Return();
            });
            var block = FindBlock("mbloq");

            var dict = this.ssa.PredecessorPhiIdentifiers(block);
            var sExp =
            #region Expected
@"m2: [r0_4]
m4: [r0_3]
m5: [r0_2]
";
            #endregion
            AssertOutput(sExp, dict);
        }

        [Test(Description = "All registers modified should be present")]
        public void SsaState_GenerateStrips_MultipleVariables()
        {
            BuildTest(m =>
            {
                var r0 = m.Reg32("r0", 0);
                var r1 = m.Reg32("r1", 1);
                var r2 = m.Reg32("r2", 2);
                m.Label("m1");
                m.BranchIf(m.Ne0(r0), "m3");

                m.Label("m2");
                m.Assign(r1, -33);
                m.Assign(r2, 314159);
                m.Goto("mbloq");

                m.Label("m3");
                m.Assign(r1, 42);
                m.Assign(r2, 99);

                m.Label("mbloq");
                m.Store(m.Word32(0x123400), r0);
                m.Store(m.Word32(0x123404), r1);
                m.Store(m.Word32(0x123408), r2);
                m.Return();
            });
            var block = FindBlock("mbloq");

            var dict = this.ssa.PredecessorPhiIdentifiers(block);
            var sExp =
            #region Expected
@"m2: [r2_5,r1_4]
m3: [r2_3,r1_2]
";
            #endregion
            AssertOutput(sExp, dict);
        }
    }
}
