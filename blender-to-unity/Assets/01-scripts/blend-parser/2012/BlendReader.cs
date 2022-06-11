using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class BlenderFile
{

    /// <summary>
    /// Describes the pointer size of the file.
    /// </summary>
    public BlendHeader Header { get; private set; }

    public BlenderFile(string filePath) : this(new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read)))
    {

    }

    public BlenderFile(BinaryReader reader)
    {   
        ReadHeader(reader);
        reader.Close();
    }

    private void ReadHeader(BinaryReader reader)
    {
        reader.ReadBytes(7);
        var PointerSize = Convert.ToChar(reader.ReadByte()) == '_' ? 4 : 8;

        char endianness = Convert.ToChar(reader.ReadByte()); // 'v' = little, 'V' = big

        if ((endianness == 'v' && !BitConverter.IsLittleEndian) || (endianness == 'V' && BitConverter.IsLittleEndian)
            || (endianness != 'v' && endianness != 'V'))
            throw new InvalidDataException("Endianness of computer does not appear to match endianness of file. Open the file in Blender and save it to convert.");

        // read out version number
        var VersionNumber = new string(new[] { Convert.ToChar(reader.ReadByte()), '.', Convert.ToChar(reader.ReadByte()),
            Convert.ToChar(reader.ReadByte()) });

        Header = new BlendHeader(PointerSize,endianness, VersionNumber);
    }
}

[System.Serializable]
public class BlendHeader
{
    public BlendHeader(int pointerSize, char endianness, string versionNumber)
    {
        PointerSize = pointerSize;
        Endian = endianness == 'v' ? "little" : "big";
        VersionNumber = versionNumber;
    }
    public int PointerSize;
    public string Endian;
    public string VersionNumber;
}

