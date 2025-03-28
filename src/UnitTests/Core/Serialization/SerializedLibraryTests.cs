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

using Reko.Core.Serialization;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Reko.Core.Services;

namespace Reko.UnitTests.Core.Serialization
{
	[TestFixture]
	public class SerializedLibraryTests
	{
        private IFileSystemService fsSvc;

        [SetUp]
        public void Setup()
        {
            fsSvc = new FileSystemService();
        }

		[Test]
		public void SlibWriteOneProcedure()
		{
			var slib = new SerializedLibrary();
			slib.Procedures.Add(MkMalloc());
			Verify(slib, "Core/SlibWriteOneProcedure.txt");
		}

		[Test]
		public void SlibReadOneProcedure()
		{
			XmlSerializer ser = SerializedLibrary.CreateSerializer_v1(typeof (SerializedLibrary));
			SerializedLibrary lib;
			using (Stream stm = fsSvc.CreateFileStream(
                FileUnitTester.MapTestPath("Core/SlibOneProcedure.xml"),
                FileMode.Open,
                FileAccess.Read))
			{
				lib = (SerializedLibrary) ser.Deserialize(stm);
			}
			Assert.AreEqual(1, lib.Procedures.Count);
			Procedure_v1 proc = (Procedure_v1) lib.Procedures[0];
			Assert.AreEqual("malloc", proc.Name);
			Assert.AreEqual(1, proc.Signature.Arguments.Length);
			Assert.AreEqual("int", proc.Signature.Arguments[0].Type.ToString());
		}

        [Test(Description = "Validates that the realmodeintservices file (in format 1) can be read properly")]
        public void SlibReadRealModeIntServices_v1()
        {
            var ser = SerializedLibrary.CreateSerializer_v1(typeof(SerializedLibrary));
            SerializedLibrary lib;
            var contents =
@"<?xml version=""1.0"" encoding=""utf-16"" ?>
<library xmlns=""http://schemata.jklnet.org/Decompiler"">
  <service name=""msdos_rename_file"">
    <syscallinfo>
      <vector>21</vector>
      <regvalue reg=""ah"">56</regvalue>
    </syscallinfo>
    <signature>
      <return>
        <flag>C</flag>
      </return>
      <arg name=""oldName""><seq><reg>ds</reg><reg>dx</reg></seq></arg>
      <arg name=""newName""><seq><reg>es</reg><reg>di</reg></seq></arg>
      <arg name=""errorCode"" out=""true""><reg>ax</reg></arg>
    </signature>
  </service>
</library>
";
            lib = (SerializedLibrary)ser.Deserialize(new StringReader(contents));
            Assert.AreEqual(1, lib.Procedures.Count);
            var svc = (SerializedService)lib.Procedures[0];
            Assert.AreEqual("msdos_rename_file", svc.Name);
            Assert.AreEqual("21", svc.SyscallInfo.Vector);
            Assert.AreEqual("ah", svc.SyscallInfo.RegisterValues[0].Register);
            Assert.AreEqual("56", svc.SyscallInfo.RegisterValues[0].Value);
            Assert.AreEqual("arg()", svc.Signature.ReturnValue.ToString());
            Assert.AreEqual(3, svc.Signature.Arguments.Length);
            Assert.AreEqual("arg(oldName,)", svc.Signature.Arguments[0].ToString());
            Assert.AreEqual("arg(newName,)", svc.Signature.Arguments[1].ToString());
            Assert.AreEqual("arg(errorCode,)", svc.Signature.Arguments[2].ToString());
        }

        [Test]
        public void SlibLoadVoidFn()
        {

        }

		private Procedure_v1 MkMalloc()
		{
			Procedure_v1 proc = new Procedure_v1
			{
				Name = "malloc",
				Signature = new SerializedSignature
				{
					Convention = "cdecl",
					ReturnValue = new Argument_v1
					{
						Kind = new Register_v1("eax"),
					},

					Arguments = new Argument_v1[] {
						new Argument_v1 {
							Name = "cb",
							Kind = new StackVariable_v1(),
						}
					}
				}
			};
			return proc;
		}

		private void Verify(SerializedLibrary slib, string outputFile)
		{
			using (FileUnitTester fut = new FileUnitTester(outputFile))
			{
				XmlTextWriter writer = new FilteringXmlWriter(fut.TextWriter);
				writer.Formatting = Formatting.Indented;
                XmlSerializer ser = SerializedLibrary.CreateSerializer_v1(slib.GetType());
				ser.Serialize(writer, slib);

				fut.AssertFilesEqual();
			}
		}

        [Test]
        public void SlibCodeWrite()
        {
            var proj = new Project_v5
            {
                InputFiles = {
                    new DecompilerInput_v5 {
                        User = new UserData_v4
                        {
                            GlobalData = {
                                new GlobalDataItem_v2
                                {
                                     Address = "00100000",
                                     Name = "foo",
                                     DataType = new ArrayType_v1 {
                                         ElementType = new PointerType_v1 {
                                              PointerSize = 4,
                                              DataType = new CodeType_v1()
                                         },
                                         Length = 10,
                                     }
                                }
                            }
                        }
                    }
                }
            };
            var ser = SerializedLibrary.CreateSerializer_v5(typeof(Project_v5));
            var sw = new StringWriter();
            var writer = new FilteringXmlWriter(sw);
            writer.Formatting = Formatting.Indented;
            ser.Serialize(writer, proj);

            Debug.Print(sw.ToString());
            var sExp = @"<?xml version=""1.0"" encoding=""utf-16""?>
<project xmlns=""http://schemata.jklnet.org/Reko/v5"">
  <input>
    <user>
      <global>
        <Address>00100000</Address>
        <arr length=""10"">
          <ptr size=""4"">
            <code />
          </ptr>
        </arr>
        <Name>foo</Name>
      </global>
      <extractResources>false</extractResources>
    </user>
  </input>
</project>";
            Assert.AreEqual(sExp, sw.ToString());
        }

        [Test]
        public void SlibCodeRead()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-16""?>
<project xmlns=""http://schemata.jklnet.org/Reko/v5"">
  <input>
    <user>
      <global>
        <addrCur>00100000</addrCur>
        <arr length=""10"">
          <ptr size=""4"">
            <code />
          </ptr>
        </arr>
        <Name>foo</Name>
      </global>
      <extractResources>false</extractResources>
    </user>
  </input>
</project>";

            var ser = SerializedLibrary.CreateSerializer_v5(typeof(Project_v5));
            var sr = new StringReader(src);
            var rdr = new XmlTextReader(sr);
            var proj = (Project_v5)ser.Deserialize(rdr);

            Assert.AreEqual(1, proj.InputFiles.Count);
            var input = (DecompilerInput_v5)proj.InputFiles[0];
            Assert.AreEqual(1, input.User.GlobalData.Count);
            Assert.AreEqual("arr(ptr(code),10)", input.User.GlobalData[0].DataType.ToString());
        }
    }
}

