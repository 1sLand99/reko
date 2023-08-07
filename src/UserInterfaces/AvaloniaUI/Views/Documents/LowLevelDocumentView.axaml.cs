#region License
/* 
 * Copyright (C) 1999-2023 John K�ll�n.
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

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Reko.Gui.ViewModels.Documents;
using Reko.UserInterfaces.AvaloniaUI.ViewModels.Documents;

namespace Reko.UserInterfaces.AvaloniaUI.Views.Documents
{
    public partial class LowLevelDocumentView : UserControl
    {
        public LowLevelDocumentView()
        {
            InitializeComponent();
        }

        public LowLevelViewModel ViewModel => ((LowLevelDocumentViewModel) DataContext!).ViewModel;


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
