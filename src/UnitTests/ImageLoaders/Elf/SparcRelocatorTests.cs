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
using Reko.Core.Configuration;
using Reko.ImageLoaders.Elf;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;

namespace Reko.UnitTests.ImageLoaders.Elf
{
    [TestFixture]
    public class SparcRelocatorTests
    {
        public void SparcRel_Test()
        {
            var sc = new ServiceContainer();
            var cfgSvc = new Mock<IConfigurationService>();
            sc.AddService(typeof(IConfigurationService), cfgSvc);
        }
    }
}
