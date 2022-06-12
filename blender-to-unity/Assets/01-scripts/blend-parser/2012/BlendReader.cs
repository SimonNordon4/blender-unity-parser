using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BlenderFile
{
    /// <summary>
    /// Contains header info for the file.
    /// </summary>
    public Header Header { get; private set; }

    /// <summary>
    /// Special 'DNA1' Structure that provides defintions for all other structures.
    /// </summary>
    /// <value></value>
    public StructureDNA StructureDNA { get; private set; }

    /// <summary>
    /// Blocks contained within the file.
    /// </summary>
    public List<FileBlock> FileBlocks { get; private set; } = new List<FileBlock>();

    private Dictionary<ulong, Structure[]> memoryMap = new Dictionary<ulong, Structure[]>();

    public BlenderFile(string filePath) : this(new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read)))
    {

    }

    public BlenderFile(BinaryReader reader)
    {
        ReadHeader(reader);
        ReadFileBlocks(reader);

        memoryMap = CreateStructures();

        // create PopulatedStructures


        reader.Close();
    }

    /// <summary>
    /// Reads the first 12 bytes of the file [0..11] which makes upt he header.
    /// </summary>
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

        Header = new Header(PointerSize, endianness, VersionNumber);
    }

    private void ReadFileBlocks(BinaryReader reader)
    {
        string lastBlockCode = "";

        while (lastBlockCode != "ENDB")
        {
            FileBlock block = FileBlock.Read(reader, Header.PointerSize);
            if (block.Code == "DNA1")
            {
                StructureDNA = (StructureDNA)block;
            }

            FileBlocks.Add(block);
            lastBlockCode = block.Code;
        }
    }

    /// <summary>
    /// Parses the file blocks to create a map of memory addresses to populated structures.
    /// </summary>
    /// <returns>A dictionary mapping memory addresses to the structures held in the corresponding file block.</returns>
    private Dictionary<ulong, Structure[]> CreateStructures()
    {
        Dictionary<ulong, Structure[]> structures = new Dictionary<ulong, Structure[]>();

        int blocksParsed = 0;
        foreach(FileBlock fileBlock in FileBlocks)
        {
            Structure[] temp = Structure.ParseFileBlock(fileBlock,blocksParsed,this);
            blocksParsed++;
        }

        return structures;
    }

    private void p(object obj)
    {
        UnityEngine.Debug.Log(obj);
    }
}



