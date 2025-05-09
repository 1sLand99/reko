#region License
/* 
 * Copyright (C) 1999-2025 John Källén.
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reko.Gui.Services
{
    /// <summary>
    /// Use this to save user settings to a platform-appropriate 
    /// storage location.
    /// </summary>
    public interface ISettingsService
    {
        object? Get(string settingName, object? defaultValue);
        string[] GetList(string settingName);

        void SetList(string name, IEnumerable<string> values);
        void Set(string name, object? value);
        void Delete(string name);

        void Load();        // Load settings from their persistent location
        void Save();        // Save settings to their persistent location
    }
}
