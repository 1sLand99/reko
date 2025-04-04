#region License
/* 
 * Copyright (C) 1999-2025 John Källén.
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
using Reko.Core;
using Reko.Core.Output;
using System;

namespace Reko.UnitTests.Arch.X86.Rewriter
{
    [TestFixture]
    public class RewriterTests2 : RewriterTestBase
    {
        public RewriterTests2()
        {
        }

        private void RunTest(string sourceFile, string outputFile)
        {
            DoRewriteFile(sourceFile);
            using (FileUnitTester fut = new FileUnitTester(outputFile))
            {
                foreach (Procedure proc in program.Procedures.Values)
                    proc.Write(true, fut.TextWriter);

                fut.AssertFilesEqual();
            }
        }

        [Test]
        public void X86Rw_Switch()
        {
            DoRewriteFile("Fragments/switch.asm");
            using (FileUnitTester fut = new FileUnitTester("Arch/X86/RwSwitch.txt"))
            {
                program.Procedures.Values[0].Write(false, fut.TextWriter);
            }
        }

        [Test]
        public void X86Rw_MemOperations()
        {
            DoRewriteFile("Fragments/memoperations.asm");
            using (FileUnitTester fut = new FileUnitTester("Arch/X86/RwMemOperations.txt"))
            {
                program.Procedures.Values[0].Write(false, fut.TextWriter);
                fut.AssertFilesEqual();
            }
        }

        [Test]
        public void X86Rw_CallTable()
        {
            DoRewriteFile("Fragments/multiple/calltables.asm");
            using (FileUnitTester fut = new FileUnitTester("Arch/X86/RwCallTable.txt"))
            {
                Dumper dump = new Dumper(program);
                dump.Dump(new TextFormatter(fut.TextWriter));
                fut.TextWriter.WriteLine();
                program.CallGraph.Write(fut.TextWriter);

                fut.AssertFilesEqual();
            }
        }

        [Test]
        public void X86Rw_StackVariables()
        {
            RunTest("Fragments/stackvars.asm", "Arch/X86/RwStackVariables.txt");
        }

        [Test]
        public void X86Rw_Duff()
        {
            RunTest("Fragments/duffs_device.asm", "Arch/X86/RwDuff.txt");
        }

        [Test]
        public void X86Rw_Factorial()
        {
            RunTest("Fragments/factorial.asm", "Arch/X86/RwFactorial.txt");
        }

        [Test]
        public void X86Rw_Loopne()
        {
            RunTest("Fragments/loopne.asm", "Arch/X86/RwLoopne.txt");
        }

        [Test]
        public void X86Rw_InterprocedureJump()
        {
            RunTest("Fragments/multiple/jumpintoproc.asm", "Arch/X86/RwInterprocedureJump.txt");
        }

        [Test]
        public void X86Rw_PopNoPop()
        {
            RunTest("Fragments/multiple/popnopop.asm", "Arch/X86/RwPopNoPop.txt");
        }

        [Test]
        public void X86Rw_Multiplication()
        {
            RunTest("Fragments/multiplication.asm", "Arch/X86/RwMultiplication.txt");
        }

        [Test]
        public void X86Rw_StackPointerMessing()
        {
            RunTest("Fragments/multiple/stackpointermessing.asm", "Arch/X86/RwStackPointerMessing.txt");
        }

        [Test]
        public void X86Rw_StringInstructions()
        {
            RunTest("Fragments/stringinstr.asm", "Arch/X86/RwStringInstructions.txt");
        }

        [Test]
        public void X86Rw_TestCondition()
        {
            RunTest("Fragments/setcc.asm", "Arch/X86/RwTestCondition.txt");
        }

        [Test]
        public void X86Rw_CopyFile()
        {
            RunTest("Fragments/copy_file.asm", "Arch/X86/RwCopyFile.txt");
        }

        [Test]
        public void X86Rw_ReadFile()
        {
            RunTest("Fragments/multiple/read_file.asm", "Arch/X86/RwReadFile.txt");
        }

        [Test]
        public void X86Rw_ProcIsolation()
        {
            RunTest("Fragments/multiple/procisolation.asm", "Arch/X86/RwProcIsolation.txt");
        }

        [Test]
        public void X86Rw_IntraSegmentFarCall()
        {
            RunTest("Fragments/multiple/intrasegmentfarcall.asm", "Arch/X86/RwIntraSegmentFarCall.txt");
        }
    }
}
