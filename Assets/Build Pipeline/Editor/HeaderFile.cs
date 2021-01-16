using System.IO;
using System.Text;

namespace CustomBuildPipeline
{
	internal sealed class HeaderFile
	{
		private readonly StringBuilder sb;
		int indent = 0;

		public HeaderFile(string metadata)
		{
			sb = new StringBuilder();
			
			BinaryReader reader = new BinaryReader(new FileStream(metadata, FileMode.Open));

			long size = reader.BaseStream.Length;
			reader.BaseStream.Seek(0, SeekOrigin.Begin);

			byte[] bytes = new byte[size];
			reader.Read(bytes, 0, (int) size);
			
			WriteLine("#pragma once");
			WriteLine();

			PrintSignature(metadata, size);

			WriteLine();
			Write($"unsigned char hexData[{size}] = ");
			WriteLine("{");
			WriteLine();

			indent++;

			for (int i = 0; i < bytes.Length; i++)
			{
				if (i % 15 == 0)
				{
					WriteIndent();
				}

				byte b = bytes[i];
				string hex = b.ToString("X");
				if (hex.Length == 1)
				{
					hex = "0" + hex;
				}

				Write($"0x{hex}");

				if (i != bytes.Length - 1)
				{
					Write(", ");

					if (i % 15 == 14)
					{
						WriteLine();
					}
				}
			}

			indent--;

			Write("\n};");
		}


		public override string ToString()
		{
			return sb.ToString();
		}

		private void PrintSignature(string path, long size)
		{
			WriteLine("//------------------------------------------------------------");
			WriteLine("//-----------        Created with Cqnity GMH       -----------");
			WriteLine("//------             https://github.com/cqtd            ------");
			WriteLine("//");
			WriteLine($"// File    : {path}");
			WriteLine("// Address : 0 (0x0)");
			WriteLine($"// Size    : {size} (0x{size:X})");
			WriteLine("//------------------------------------------------------------");
		}

		private void WriteLine(string line)
		{
			WriteIndent();

			sb.Append(line);
			WriteLine();
		}

		private void WriteLine()
		{
			sb.Append("\n");
		}

		private void Write(string text)
		{
			sb.Append(text);
		}

		private void WriteIndent()
		{
			for (int i = 0; i < indent; i++)
			{
				sb.Append("\t");
			}
		}
	}
}

