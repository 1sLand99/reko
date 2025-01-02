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

using Reko.Core;
using Reko.Gui;
using Reko.Gui.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reko.UserInterfaces.WindowsForms.Forms
{
    public partial class ProcedureDialog : Form, IDialog<UserProcedure>
    {
        private ProcedureDialogInteractor interactor;

        public ProcedureDialog(ProcedureDialogInteractor i)
        {
            InitializeComponent();
            this.interactor = i;
        }

        public TextBox ProcedureName
        {
            get { return txtName; }
        }

        public TextBox Signature
        {
            get { return txtSignature; }
        }

        public TextBox Comment => txtComment;

        public CheckBox Decompile
        {
            get { return chkDecompile; }
        }

        public CheckBox Allocator
        {
            get { return chkMalloc; }
        }

        public CheckBox IsAlloca
        {
            get { return chkIsAlloca; }
        }

        public CheckBox Terminates
        {
            get { return chkTerminates; }
        }

        public TextBox VarargsFormatParser => this.txtVarargsFormatParser;

        public Button OkButton { get { return btnOK; } }

        public UserProcedure Value
        {
            get { return this.interactor.ApplyChanges(); }
        }
    }
}
