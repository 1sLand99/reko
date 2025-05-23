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
using Reko.Analysis;
using Reko.Arch.X86;
using Reko.Arch.X86.Assembler;
using Reko.Core;
using Reko.Core.Configuration;
using Reko.Core.Expressions;
using Reko.Core.Operators;
using Reko.Core.Services;
using Reko.Core.Types;
using Reko.Environments.Msdos;
using Reko.Environments.Windows;
using Reko.Loading;
using Reko.Scanning;
using Reko.UnitTests.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using Reko.Core.Loading;
using Reko.Core.Serialization;
using Reko.Services;

namespace Reko.UnitTests.Decompiler.Typing
{
    /// <summary>
    /// Base class for all typing tests.
    /// </summary>
    public abstract class TypingTestBase
	{
        private IDecompilerEventListener eventListener;

        public TypingTestBase()
        {
        }

        protected Program RewriteFile16(string relativePath) { return RewriteFile(relativePath, Address.SegPtr(0xC00, 0), (s, a) => new MsdosPlatform(s,a)); }

		protected Program RewriteFile32(string relativePath) { return RewriteFile(relativePath, Address.Ptr32(0x00100000), (s, a) => new Win32Platform(s, a)); }

		protected Program RewriteFile(
            string relativePath,
            Address addrBase,
            Func<IServiceProvider, IProcessorArchitecture, IPlatform> mkPlatform)
		{
            var sc = new ServiceContainer();
            PopulateServiceContainer(sc);
            var arch = new X86ArchitectureReal(sc, "x86-real-16", new Dictionary<string, object>());
            ILoader ldr = new Loader(sc);
            var program = ldr.AssembleExecutable(
                ImageLocation.FromUri(FileUnitTester.MapTestPath(relativePath)),
                new X86TextAssembler(arch),
                null,
                addrBase);
            program.Platform = mkPlatform(sc, program.Architecture);

            var ep = ImageSymbol.Procedure(arch, program.SegmentMap.BaseAddress);
            var project = new Project { Programs = { program } };
            var scan = new Scanner(
                program,
                project.LoadedMetadata,
                new DynamicLinker(project, program, eventListener),
                sc);
			scan.EnqueueImageSymbol(ep, true);
			scan.ScanImage();

            var dynamicLinker = new DynamicLinker(project, program, eventListener);
            var dfa = new DataFlowAnalysis(program, dynamicLinker, sc);
			dfa.AnalyzeProgram();
            return program;
		}

        protected virtual void PopulateServiceContainer(ServiceContainer sc)
        {
            var config = new FakeDecompilerConfiguration();
            eventListener = new FakeDecompilerEventListener();
            sc.AddService<IConfigurationService>(config);
            sc.AddService<IDecompiledFileService>(new FakeDecompiledFileService());
            sc.AddService<IEventListener>(eventListener);
            sc.AddService<IDecompilerEventListener>(eventListener);
            sc.AddService<IFileSystemService>(new FileSystemService());
            sc.AddService<IPluginLoaderService>(new PluginLoaderService());
        }

        protected void RunHexTest(string hexFile, string outputFile)
        {
            var svc = new ServiceContainer();
            var cfg = new FakeDecompilerConfiguration();
            var eventListener = new FakeDecompilerEventListener();
            svc.AddService<IConfigurationService>(cfg);
            svc.AddService<IEventListener>(eventListener);
            svc.AddService<IDecompilerEventListener>(eventListener);
            svc.AddService<IDecompiledFileService>(new FakeDecompiledFileService());
            ILoader ldr = new Loader(svc);
            var fileUri = ImageLocation.FromUri(FileUnitTester.MapTestPath(hexFile));
            var imgLoader = new DchexLoader(svc, fileUri, null);
            var program = imgLoader.LoadProgram(null);
            var project = new Project { Programs = { program } };
            var ep = ImageSymbol.Procedure(program.Architecture, program.ImageMap.BaseAddress);
            var dynamicLinker = new DynamicLinker(project, program, eventListener);
            var scan = new Scanner(program, project.LoadedMetadata, dynamicLinker, svc);
            scan.EnqueueImageSymbol(ep, true);
            scan.ScanImage();

            var dfa = new DataFlowAnalysis(program, null, svc);
            dfa.AnalyzeProgram();
            RunTest(program, outputFile);
        }

        protected void RunTest16(string srcfile, string outputFile)
        {
            RunTest(RewriteFile16(srcfile), outputFile);
        }

        protected void RunTest32(string srcfile, string outputFile)
        {
            RunTest(RewriteFile32(srcfile), outputFile);
        }
        
        protected void RunTest(ProgramBuilder mock, string outputFile)
        {
            var sc = new ServiceContainer();
            PopulateServiceContainer(sc);
            Program program = mock.BuildProgram();
            var dynamicLinker = new Mock<IDynamicLinker>();
            DataFlowAnalysis dfa = new DataFlowAnalysis(program, dynamicLinker.Object, sc);
            var ssts = dfa.UntangleProcedures();
            dfa.BuildExpressionTrees(ssts);
            RunTest(program, outputFile);
        }

        protected virtual void RunTest(Action<ProcedureBuilder> pg, string outputFile)
        {
            ProcedureBuilder m = new ProcedureBuilder();
            pg(m);
            ProgramBuilder program = new ProgramBuilder();
            program.Add(m);
            RunTest(program, outputFile);
        }

        protected abstract void RunTest(Program program, string outputFile);


		protected MemoryAccess MemLoad(Identifier id, int offset, DataType size)
		{
			return new MemoryAccess(MemoryStorage.GlobalMemory, 
				new BinaryExpression(Operator.IAdd, PrimitiveType.Word32, id, Constant.Word32(offset)),
				size);
		}
	}
}
