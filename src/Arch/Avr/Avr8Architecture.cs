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

using Reko.Arch.Avr.Avr8;
using Reko.Core;
using Reko.Core.Expressions;
using Reko.Core.Lib;
using Reko.Core.Machine;
using Reko.Core.Memory;
using Reko.Core.Rtl;
using Reko.Core.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Reko.Arch.Avr
{
    public class Avr8Architecture : ProcessorArchitecture
    {

        public Avr8Architecture(IServiceProvider services, string archId, Dictionary<string, object> options)
            : base(services, archId, options, null, null)
        {
            this.Endianness = EndianServices.Little;
            this.PointerType = PrimitiveType.Ptr16;
            this.WordWidth = PrimitiveType.Word16;
            this.FramePointerType = PrimitiveType.UInt8;
            this.InstructionBitSize = 16;
            this.StackRegister = Registers.StackRegister;
        }
        
        static Avr8Architecture()
        {
        }


		public override IEnumerable<MachineInstruction> CreateDisassembler(EndianImageReader rdr)
        {
            return new Avr8Disassembler(this, rdr);
        }

        public override IEqualityComparer<MachineInstruction> CreateInstructionComparer(Normalize norm)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Address> CreatePointerScanner(SegmentMap map, EndianImageReader rdr, IEnumerable<Address> knownAddresses, PointerScannerFlags flags)
        {
            throw new NotImplementedException();
        }

        public override ProcessorState CreateProcessorState()
        {
            return new Avr8State(this);
        }

        public override IEnumerable<RtlInstructionCluster> CreateRewriter(EndianImageReader rdr, ProcessorState state, IStorageBinder binder, IRewriterHost host)
        {
            return new Avr8Rewriter(this, rdr, state, binder, host);
        }

        public override FlagGroupStorage GetFlagGroup(string name)
        {
            throw new NotImplementedException();
        }

        public override FlagGroupStorage GetFlagGroup(RegisterStorage flagRegister, uint grf)
        {
            var fl = new FlagGroupStorage(Registers.sreg, grf, GrfToString(flagRegister, "", grf));
            return fl;
        }

        public override SortedList<string, int> GetMnemonicNames()
        {
            throw new NotImplementedException();
        }

        public override int? GetMnemonicNumber(string name)
        {
            throw new NotImplementedException();
        }


        public override RegisterStorage GetRegister(StorageDomain domain, BitRange range)
        {
            var reg= Registers.regs[domain - StorageDomain.Register];
            if (domain == Registers.z.Domain)
            {
                if (range.Lsb == 0)
                    return Registers.regs[30];
                else
                    return Registers.regs[31];
            }
            return reg;
        }

        public RegisterStorage GetRegister(int i)
        {
            return Registers.regs[i];
        }

        public override RegisterStorage[] GetRegisters()
        {
            return Registers.regs;
        }

        public override string GrfToString(RegisterStorage flagRegister, string prefix, uint grf)
        {
            var s = new StringBuilder();
            foreach (var tpl in Registers.grfToString)
            {
                if ((grf & (uint)tpl.Item1) != 0)
                {
                    s.Append(tpl.Item2);
                }
            }
            return s.ToString();
        }

        public override Address MakeAddressFromConstant(Constant c, bool codeAlign)
        {
            var uAddr = c.ToUInt32();
            if (codeAlign)
                uAddr &= ~1u;
            return Address.Ptr16((ushort)uAddr);
        }

        public override Address? ReadCodeAddress(int size, EndianImageReader rdr, ProcessorState? state)
        {
            throw new NotImplementedException();
        }

        public override bool TryGetRegister(string name, [MaybeNullWhen(false)] out RegisterStorage reg)
        {
            throw new NotImplementedException();
        }

        public override bool TryParseAddress(string? txtAddr, [MaybeNullWhen(false)] out Address addr)
        {
            return Address.TryParse16(txtAddr, out addr);
        }

        /* I/O registers
         * 0x08 ACSR
         * 0x07 ADMUX
         * 0x06 ADCSRA
         * 0x05 ADCH
         * 0x04 ADCL
         * 0x03 ADCSRB
         * 0x14 DIDR0
         * 0x13 GPIOR2
         * 0x12 GPIOR1
         * 0x11 GPIOR0
         * 0x10 USIBR
         * 0x0F USIDR
         * 0x0E USISR
         * 0x0D USICR
         * 0x15 PCMSK   - Pin change mask register
         * 0x16 PINB    - Port B input pins address
         * 0x17 DDRB    - Data direction register for port B
         * 0x18 PORTB   - Port B data register
         * 0x1C EECR    - EEPROM control register
         * 0x1D EEDR    - EEPROM data register
         * 0x1E EEARL   - EEPROM address register
         * 0x1F EEARH
         * 0x20 PRR     - Power reduction register
         * 0x21 WDTCR   - Watchdog timer control register
         * 0x22 DWDR
         * 0x23 DTPS1   – Timer/Counter1 Dead Time Prescaler Register 1
         * 0x24 DT1B
         * 0x25 DT1A
         * 0x26 CLKPR   - Clock prescale register
         * 0x27 PLLCSR  - PLL control and status register
         * 0x28 OCR0B   - Output compare register B
         * 0x29 OCR0A   - Output compare register A
         * 0x2A TCCR0A
         * 0x2B OCR1B   - Timer / Counter 1 Output compare register B
         * 0x2C GTCCR   - General Timer / Counter 1 control register
         * 0x2D OCR1C   - Timer / Counter 1 Output compare register C
         * 0x2E OCR1A   - Timer / Counter 1 Output compare register A
         * 0x2F TCNT1   - Timer / Counter 1 register
         * 0x30 TCCR1   - Timer / Counter 1 control register
         * 0x31 OSCCAL  - Oscillator calibration register
         * 0x32 TCNT0   - Timer / Counter register
         * 0x33 TCCR0B
         * 0x34 MCUSR   - MCU status register
         * 0x35 MCUCR   - MCU control register
         * 0x37 SPMCSR
         * 0x38 TIFR    - Timer/Counter flag register
         * 0x39 TIMSK   - Timer/Counter interrupt mask register
         * 0x3A GIFR
         * 0x3B GIMSK   - General interrupt mask register
         * 0x003F SREG - status reg - flag register for 
         * 0x003D SPL
         * 0x003D SPH   - aliases for SP
         */
         /* Interrupt vectors
1 0x0000 RESET External Pin, Power-on Reset,
Brown-out Reset, Watchdog Reset
2 0x0001 INT0 External Interrupt Request 0
3 0x0002 PCINT0 Pin Change Interrupt Request 0
4 0x0003 TIMER1_COMPA Timer/Counter1 Compare Match A
5 0x0004 TIMER1_OVF Timer/Counter1 Overflow
6 0x0005 TIMER0_OVF Timer/Counter0 Overflow
7 0x0006 EE_RDY EEPROM Ready
8 0x0007 ANA_COMP Analog Comparator
9 0x0008 ADC ADC Conversion Complete
10 0x0009 TIMER1_COMPB Timer/Counter1 Compare Match B
11 0x000A TIMER0_COMPA Timer/Counter0 Compare Match A
12 0x000B TIMER0_COMPB Timer/Counter0 Compare Match B
13 0x000C WDT Watchdog Time-out
14 0x000D USI_START USI START
15 0x000E USI_OVF USI Overflow
         */
    }

    [Flags]
    public enum FlagM
    {
        CF = 1,
        ZF = 2,
        NF = 4,
        VF = 8,
        SF = 16,
        HF = 32,
        TF = 64,
        IF = 128
    }
}
 