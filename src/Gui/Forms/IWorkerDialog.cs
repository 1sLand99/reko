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

using Reko.Gui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Reko.Gui.Forms
{
    public interface IWorkerDialog : IDialog
    {
        event EventHandler Load;
        event EventHandler Closed;

        BackgroundWorker Worker { get; }
        IButton CancellationButton { get; }
        Services.DialogResult DialogResult { get; set; }
        IProgressBar ProgressBar { get; }
        ILabel Caption { get; }
        ILabel Detail { get; }

        void Invoke(Action action);     // Performs the action on the UI thread.
        void Close();
    }
}
