#region License
/* 
 * Copyright (C) 1999-2019 John Källén.
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
using Reko.Gui;
using System;
using System.ComponentModel.Design;

namespace Reko.UserInterfaces.WindowsForms.Forms
{
    public class CallHierarchyInteractor : ICallHierarchyService, ICommandTarget, IWindowPane
    {
        private CallHierarchyView view;

        public CallHierarchyInteractor(CallHierarchyView view)
        {
            this.view = view;
        }

        public IWindowFrame Frame { get; set; }

        public void Close()
        {
        }

        public object CreateControl()
        {
            return view;
        }

        public bool Execute(CommandID cmdId)
        {
            throw new NotImplementedException();
        }

        public bool QueryStatus(CommandID cmdId, CommandStatus status, CommandText text)
        {
            throw new NotImplementedException();
        }

        public void SetSite(IServiceProvider services)
        {
            throw new NotImplementedException();
        }

        public void Show(Program program, Procedure proc)
        {
            throw new NotImplementedException();
        }
    }
}
