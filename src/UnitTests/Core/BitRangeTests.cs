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

using NUnit.Framework;
using Reko.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reko.UnitTests.Core
{
    [TestFixture]
    public class BitRangeTests
    {
        [Test]
        public void Bitr_Union()
        {
            var a = new BitRange(0, 16);
            var b = new BitRange(0, 8);
            var c = a | b;
            Assert.AreEqual(new BitRange(0, 16), c);
        }

        [Test]
        public void Bitr_Difference_LowOverlap()
        {
            var a = new BitRange(0, 16);
            var b = new BitRange(0, 8);
            var c = a - b;
            Assert.AreEqual(new BitRange(8, 16), c);
        }

        [Test]
        public void Bitr_Difference_HighOverlap()
        {
            var a = new BitRange(0, 16);
            var b = new BitRange(8, 16);
            var c = a - b;
            Assert.AreEqual(new BitRange(0, 8), c);
        }

        [Test]
        public void Bitr_Difference_Contains()
        {
            var a = new BitRange(0, 32);
            var b = new BitRange(8, 16);
            var c = a - b;
            Assert.AreEqual(new BitRange(0, 32), c);
        }
    }
}
