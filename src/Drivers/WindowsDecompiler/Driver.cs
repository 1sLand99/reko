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

using Reko.Core.Services;
using Reko.Gui;
using Reko.Gui.Services;
using Reko.UserInterfaces.WindowsForms;
using Reko.UserInterfaces.WindowsForms.Controls;
using Reko.UserInterfaces.WindowsForms.Forms;
using System;
using System.ComponentModel.Design;
using System.Runtime.Versioning;

namespace Reko.WindowsDecompiler
{
    public class Driver
    {
        [STAThread]
        [SupportedOSPlatform("windows")]
        public static void Main(string[] args)
        {
            var services = new ServiceContainer();
            var mainForm = new MainForm();
            var factory = new WindowsFormsTextSpanFactory();
            services.AddService<IServiceFactory>(new WindowsServiceFactory(services, mainForm, factory));
            services.AddService<IDialogFactory>(new WindowsFormsDialogFactory(services));
            services.AddService<IWindowPaneFactory>(new WindowsFormsPaneFactory(services, factory));
            services.AddService<IRegistryService>(new WindowsFormsRegistryService());
            services.AddService<ISettingsService>(new WindowsFormsSettingsService(services));
            services.AddService<ISelectionService>(new SelectionService());
            services.AddService<ISelectedAddressService>(new SelectedAddressService());
            services.AddService<IFileSystemService>(new FileSystemService());
            services.AddService<IPluginLoaderService>(new PluginLoaderService());
            mainForm.Attach(services);
            System.Windows.Forms.Application.Run(mainForm);
        }
	}
}
