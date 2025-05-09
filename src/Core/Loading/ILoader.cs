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

using Reko.Core.Assemblers;
using Reko.Core.Configuration;
using Reko.Core.Scripts;
using System.Collections.Generic;

namespace Reko.Core.Loading
{
    /// <summary>
    /// Implementors of this interface assume responsibility for loading 
    /// binaries or Reko project files.
    /// </summary>
    public interface ILoader
    {
        /// <summary>
        /// If no image type can be determined, assume the file is in this format.
        /// </summary>
        string? DefaultToFormat { get; set; }

        /// <summary>
        /// Loads the file specified by <paramref name="imageLocation"/>. If it is a Reko project
        /// file, loads that. If not, tries to match the file contents to one of the known
        /// file formats, or uses an explicity provided <paramref name="loader"/>.
        /// </summary>
        /// <param name="imageLocation">The <see cref="ImageLocation"/> from which</param>
        /// <param name="loader">Optional name of a specific image loader to use.</param>
        /// <param name="addrLoad">Optional address at which to load the image.</param>
        /// <returns>
        /// An <see cref="ILoadedImage"/> instance. In particular, if the
        /// file format wasn't recognized an instance of <see cref="Blob"/> is
        /// returned.
        /// </returns>
        ILoadedImage Load(
            ImageLocation imageLocation,
            string? loader = null,
            Address? addrLoad = null);

        /// <summary>
        /// Opens the specified file and reads the contents of the file.
        /// No interpretation of the file data is done.
        /// </summary>
        /// <param name="filename">The file system path of the file from which 
        /// to read the image data.</param>
        /// <returns>An array of bytes containing the file data.</returns>
        //$TODO: Change the output to Stream; the image could be so large it doesn't
        // fit in main memory.
        byte[] LoadFileBytes(string filename);

        /// <summary>
        /// Opens an image from the specified location. The image may be located
        /// in arbitraritly deeply nested <see cref="IArchive"/>s.
        /// </summary>
        /// <param name="binLocation"><see cref="ImageLocation"/> describing 
        /// the location of the requested image.</param>
        /// <returns>
        /// The loaded image.
        /// </returns>
        byte[] LoadImageBytes(ImageLocation binLocation);

        /// <summary>
        /// Given a file image in <paramref name="bytes"/>, determines which file 
        /// format the file has and delegates parsing to a specific image loader.
        /// </summary>
        /// <param name="imageLocation">The <see cref="ImageLocation"/> from where the 
        /// image was loaded.</param>
        /// <param name="bytes">The contents of the executable file.</param>
        /// <param name="loader">The (optional) name of a specific loader. Providing a
        /// non-zero loader will override the file format determination process.</param>
        /// <param name="arch">The (optional) <see cref="IProcessorArchitecture"/> to use
        /// when loading the file.</param>
        /// <param name="loadAddress">Address at which to load the binary. This may be null,
        /// in which case the default address of the image loader will be used.</param>
        /// <returns>
        /// Either a successfully loaded <see cref="ILoadedImage"/>, or a <see cref="Blob"/>
        /// if an appropriate image loader could not be determined or loaded.
        /// </returns>
        ILoadedImage ParseBinaryImage(
            ImageLocation imageLocation,
            byte[] bytes,
            string? loader,
            IProcessorArchitecture? arch,
            Address? loadAddress);

        /// <summary>
        /// Given a file image in <paramref name="bytes"/>, parses the
        /// contents of the file using the provided <paramref name="loadDetails"/>.
        /// </summary>
        /// <param name="imageLocation">Location of the file.</param>
        /// <param name="bytes">The contents of the file.</param>
        /// <param name="loadDetails">Extra information to assist in parsing the file.</param>
        /// <returns></returns>
        ILoadedImage ParseBinaryImage(
            ImageLocation imageLocation,
            byte[] bytes,
            LoadDetails loadDetails);

        /// <summary>
        /// Given a sequence of raw bytes, uses the provided <paramref name="details"/> to
        /// make sense of it. Use this method if the binary has no known file format.
        /// </summary>
        /// <param name="image">The raw contents of the file.</param>
        /// <param name="loadAddress">The address at which the raw contents are to be loaded.</param>
        /// <param name="details">Details about the contents of the file.</param>
        /// <returns>A <see cref="Reko.Core.Program"/>.
        /// </returns>
        Program ParseRawImage(byte[] image, Address? loadAddress, LoadDetails details);

        /// <summary>
        /// Loads a raw image from the specified location. 
        /// </summary>
        /// <param name="raw">Details about the raw file.</param>
        /// <returns>A <see cref="Program"/>.</returns>
        Program LoadRawImage(LoadDetails raw);

        //$TODO: deprecate this method.
        /// <summary>
        /// Loads a raw image from the specified bytes. 
        /// </summary>
        /// <param name="bytes">Raw bytes.</param>
        /// <param name="raw">Details about the raw file.</param>
        /// <returns>A <see cref="Program"/>.</returns>
        Program LoadRawImage(byte[] bytes, LoadDetails raw);

        /// <summary>
        /// Assembles the assembly language source file located at <paramref name="asmfileLocation"/>
        /// into a new <see cref="Program"/>.
        /// </summary>
        /// <param name="asmfileLocation">The location of the assembly language source file.</param>
        /// <param name="asm">Assembler to be used for assembling.</param>
        /// <param name="platform">Platform to be used for assembling.</param>
        /// <param name="loadAddress">Address at which the assembled program should be loaded.</param>
        /// <returns>A <see cref="Program"/> instance ready to be analyzed.</returns>
        Program AssembleExecutable(
            ImageLocation asmfileLocation,
            IAssembler asm,
            IPlatform platform,
            Address loadAddress);

        /// <summary>
        /// Assembles the assembly language source bytes, given a location at <paramref name="asmfileLocation"/>,
        /// into a new <see cref="Program"/>.
        /// </summary>
        /// <param name="asmfileLocation">The location of the assembly language source file.</param>
        /// <param name="bytes">The raw bytes of the assembly language program.</param>
        /// <param name="asm">Assembler to be used for assembling.</param>
        /// <param name="platform">Platform to be used for assembling.</param>
        /// <param name="loadAddress">Address at which the assembled program should be loaded.</param>
        /// <returns>A <see cref="Program"/> instance ready to be analyzed.</returns>
        Program AssembleExecutable(
            ImageLocation asmfileLocation,
            byte[] bytes,
            IAssembler asm,
            IPlatform platform,
            Address loadAddress);

        /// <summary>
        /// Loads a file containing symbolic, type, or other metadata into a <see cref="TypeLibrary"/>.
        /// </summary>
        /// <param name="metadataLocation">The location of the metadata information.</param>
        /// <param name="platform">The operating environment for the file.</param>
        /// <param name="typeLib">A type library into which the metadata will be added.</param>
        /// <returns>The updated <paramref name="typeLib"/> or null if no appropriate loader for the
        /// metadata could be found.
        /// </returns>
        TypeLibrary? LoadMetadata(ImageLocation metadataLocation, IPlatform platform, TypeLibrary typeLib);

        /// <summary>
        /// Loads a file containing script.
        /// </summary>
        /// <param name="scriptLocation">The location of the script.</param>
        /// <returns>
        /// Evaluated script file or null if no appropriate loader for the
        /// script format could be found.
        /// </returns>
        ScriptFile? LoadScript(ImageLocation scriptLocation);
    }

    /// <summary>
    /// Auxiliary details used when the file format itself doesn't provide 
    /// enough information to be loaded correctly.
    /// </summary>
    public class LoadDetails
    {
        /// <summary>
        /// The <see cref="ImageLocation"/>location of the image to be loaded.
        /// </summary>
        public ImageLocation? Location;

        /// <summary>
        /// Name of the loader to use. Loader names are found in the <c>reko.config</c> file,
        /// and can be considered shortcuts. For example loading the well known ELF 
        /// format, use <c>elf</c>.
        /// <para/>
        /// In addition, specifying a fully-qualified class name makes it possible to load
        /// custom file formats without needing to register them in the <c>reko.config</c> file.
        /// </summary>
        public string? LoaderName;

        /// <summary>
        /// Number of zero bytes to prepend to the image before reading it.
        /// </summary>
        public long Offset;

        /// <summary>
        /// Name of the processor architecture to use. Architecture names are found 
        /// in the <c>reko.config</c> file.
        /// </summary>
        public string? ArchitectureName;
        
        /// <summary>
        /// Architecture specific options. Each architecture defines its own
        /// set of options, like endianness, processor models etc.
        /// </summary>
        public Dictionary<string,object>? ArchitectureOptions;

        /// <summary>
        /// Name of the platform to use. Platform names are found in the 
        /// <c>reko.config</c> file.
        /// </summary>
        public string? PlatformName;
        
        /// <summary>
        /// String representation of the address at which the binary file should
        /// be loaded. The address string is parsed by the architecture when loading.
        /// A null or empty value here means the GUI should show the Base Address Finder 
        /// to try to guess where the image starts.
        /// </summary>
        public string? LoadAddress;
        
        /// <summary>
        /// Entry point of the program.
        /// </summary>
        public EntryPointDefinition? EntryPoint;

        /// <summary>
        /// Zero or more user defined segments.
        /// </summary>
        public List<UserSegment>? Segments { get; set; }
    }
}
