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

using System;

namespace Reko.Services
{
    /// <summary>
    /// Provides access to the running instance of the decompiler.
    /// </summary>
    public interface IDecompilerService
    {
        /// <summary>
        /// Event is fired when the decompiler is changed for some reason.
        /// </summary>
        event EventHandler DecompilerChanged;

        /// <summary>
        /// Current decompiler instance.
        /// </summary>
        IDecompiler? Decompiler { get; set; }

        /// <summary>
        /// The name of the project being decompiled.
        /// </summary>
        string ProjectName { get; }
    }

    /// <summary>
    /// Implementation of <see cref="IDecompilerService"/> that provides access to the
    /// current decompiler instance.
    /// </summary>
    public class DecompilerService : IDecompilerService
    {
        private IDecompiler? decompiler;

        /// <inheritdoc/>

        public event EventHandler? DecompilerChanged;

        #region IDecompilerService Members

        /// <inheritdoc/>
        public IDecompiler? Decompiler
        {
            get { return decompiler; }
            set
            {
                decompiler = value;
                DecompilerChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //$REVIEW" huh?
        /// <inheritdoc/>
        public string ProjectName
        {
            get
            { 
                if (decompiler is null)
                    return "";
                if (decompiler.Project is null)
                    return "";
                if (decompiler.Project.Programs.Count == 0)
                    return "";
                var projectUri = decompiler.Project.Programs[0].Location;
                if (projectUri is null)
                    return "";
                return projectUri.GetFilename();
            }
        }

        #endregion
    }
}
