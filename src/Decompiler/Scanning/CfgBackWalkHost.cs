#region License
/* 
 * Copyright (C) 1999-2023 John Källén.
 .
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
using Reko.Core.Expressions;
using Reko.Core.Rtl;
using Reko.Scanning;
using System.Collections.Generic;
using System.Linq;

namespace Reko.Scanning
{
    /// <summary>
    /// Implmentation of the <see cref="IBackWalkHost{TBlock, TInstr}"/> 
    /// interface for use with the <see cref="ScanResultsV2"/> class.
    /// </summary>
    public class CfgBackWalkHost : IBackWalkHost<RtlBlock, RtlInstruction>
    {
        private readonly ScanResultsV2 cfg;
        private readonly Dictionary<Address,List<Address>> backEdges;

        public CfgBackWalkHost(
            Program program,
            IProcessorArchitecture arch,
            ScanResultsV2 cfg, 
            Dictionary<Address, List<Address>> backEdges)
        {
            this.Program = program;
            this.SegmentMap = program.SegmentMap;
            this.Architecture = arch;
            this.cfg = cfg;
            this.backEdges = backEdges;
        }

        public IProcessorArchitecture Architecture { get; }

        public Core.Program Program { get; }

        public SegmentMap SegmentMap { get; }

        public (Expression?, Expression?) AsAssignment(RtlInstruction instr)
        {
            if (instr is RtlAssignment ass)
                return (ass.Dst, ass.Src);
            return (null, null);
        }

        public Expression? AsBranch(RtlInstruction instr)
        {
            if (instr is RtlBranch bra)
                return bra.Condition;
            return null;
        }

        public int BlockInstructionCount(RtlBlock rtlBlock)
        {
            return rtlBlock.Instructions.Sum(i => i.Instructions.Length);
        }

        public IEnumerable<(Address?, RtlInstruction?)> GetBlockInstructions(RtlBlock block)
        {
            return block.Instructions.SelectMany(c => c.Instructions.Select(i => (c.Address, i)))!;
        }

        public List<RtlBlock> GetPredecessors(RtlBlock block)
        {
            if (!backEdges.TryGetValue(block.Address, out var preds))
                return new List<RtlBlock>();
            var pp = preds.Select(p => cfg.Blocks[p]).ToList();
            return pp;
        }

        public RtlBlock? GetSinglePredecessor(RtlBlock block)
        {
            throw new System.NotImplementedException();
        }

        public RegisterStorage GetSubregister(RegisterStorage rIdx, BitRange range)
        {
            throw new System.NotImplementedException();
        }

        public bool IsFallthrough(RtlInstruction instr, RtlBlock block)
        {
            throw new System.NotImplementedException();
        }

        public bool IsStackRegister(Storage storage)
        {
            throw new System.NotImplementedException();
        }

        public bool IsValidAddress(Address addr)
        {
            throw new System.NotImplementedException();
        }

        public Address? MakeAddressFromConstant(Constant c)
        {
            throw new System.NotImplementedException();
        }

        public Address MakeSegmentedAddress(Constant selector, Constant offset)
        {
            throw new System.NotImplementedException();
        }
    }
}