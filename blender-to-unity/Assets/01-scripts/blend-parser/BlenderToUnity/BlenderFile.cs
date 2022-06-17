using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BlenderToUnity
{
    public class BlenderFile
    {
        /// <summary>
        /// Binary reader associated with this file while it's being read.
        /// </summary>
        public BinaryReader Reader { get; private set; }
        /// <summary>
        /// Source File Path of the .blend being read.
        /// </summary>
        public string SourceFilePath {get; private set; }
        /// <summary>
        /// Parsed header of the .blend file.
        /// </summary>
        public Header Header {get; private set; }

        /// <summary>
        /// List of uncast FileBlocks in the .blend file. We only have their sdna index and a data blob at this stage.
        /// </summary>
        public List<FileBlock> FileBlocks {get; private set; } = new List<FileBlock>();

        /// <summary>
        /// The special DNA1 Block of the FileBlocks. DNA1 is the second last block and contains descriptions for all other blocks.
        /// </summary>
        /// <value></value>
        public DNA1Block DNA1Block {get; private set;}

        /// <summary>
        /// Blend File Structure Definitions generated from the dna1 block. The contents within describe blender Types, Structures and Fields.
        /// </summary>
        public StructureDNA StructureDNA {get; private set; }

        /// <summary>
        /// A dictionary mapping memory addresses to the structures held in the corresponding file block.
        /// </summary>
        public Dictionary<ulong,Structure[]> MemoryMap {get; private set; } = new Dictionary<ulong,Structure[]>();

        public BlenderFile(string path) : this(new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read)))
        {
            if(!File.Exists(path))
            {
                f.printError("Unable to find file: " + path);
                return;
            }

            SourceFilePath = path;
        }

        public BlenderFile(BinaryReader reader)
        {
            Reader = reader;

            f.startwatch("Parse Blend");

            #region Get Header
            f.startwatch("Read Header");

            Header = ReadHeader(reader);
            if(Header is null)
            {
                f.printError("Failed to read header. Aborting.");
                reader.Close();
                return;
            }

            f.stopwatch("Read Header");
            #endregion

            #region Get File Blocks
            f.startwatch("Read File Blocks");

            FileBlocks = ReadFileBlocks(reader);

            if(FileBlocks is null)
            {
                f.printError("Failed to read FileBlocks. Aborting.");
                reader.Close();
                return;
            }

            f.stopwatch("Read File Blocks");
            #endregion

            #region Get SDNAStructure
            f.startwatch("Read SDNAStructure");

            DNA1Block = DNA1Block.ReadDNA1Block(this);

            f.stopwatch("Read SDNAStructure");
            #endregion

            #region Create SDNAStructure
            f.startwatch("Generate SDNAStructure data");
            
            StructureDNA = new StructureDNA(this);

            f.stopwatch("Generate SDNAStructure data");
            #endregion

            // #region Create Memory Map
            // f.startwatch("Create Memory Map");

            // MemoryMap = CreateMemoryMap();
            // if(MemoryMap is null)
            // {
            //     f.printError("Failed to create MemoryMap. Aborting.");
            //     reader.Close();
            //     return;
            // }

            // f.stopwatch("Create Memory Map");
            // #endregion

            reader.Close();

            f.stopwatch("Parse Blend");
        }

        /// <summary>
        /// Read and set the header. Returns null if the header is invalid.
        /// </summary>
        private Header ReadHeader(BinaryReader reader)
        {
            reader.ReadBytes(7); // read out 'BLENDER', this can be used to determine if the file is gzipped

            var pointerSize = Convert.ToChar(reader.ReadByte()) == '_' ? 4 : 8; // '_' = 4, '-' = 8

            if (pointerSize != 4 && pointerSize != 8)
            {
                f.printError("Invalid pointer size: " + pointerSize);
                return null;
            }

            char endianness = Convert.ToChar(reader.ReadByte()); // 'v' = little, 'V' = big

            if (endianness != 'v' && endianness != 'V')
            {
                f.printError("Invalid endianness: " + endianness);
                return null;
            }

            if ((endianness == 'v' && !BitConverter.IsLittleEndian) || (endianness == 'V' && BitConverter.IsLittleEndian)|| (endianness != 'v' && endianness != 'V'))
            {
                f.printError("Endianness of computer does not appear to match endianness of file. Open the file in Blender and save it to convert.");
                return null;
            }

            var vn = reader.ReadBytes(3);
            var versionNumber = new string(vn.Select(x => (char)x).ToArray());

            // Set Header.
            var header = new Header(pointerSize, versionNumber, endianness);

            return header;
        }
    
        /// <summary>
        /// Read and set the FileBlocks. Returns null if any file block is invalid.
        /// Will also collect the DNA1 Block.
        /// </summary>
        private List<FileBlock> ReadFileBlocks(BinaryReader reader)
        {
            var fileBlocks = new List<FileBlock>();
            var blockCode = "";

            int blocksRead = 0;
            // ENDB is the last block in the file.
            while (blockCode != "ENDB")
            {
                FileBlock block = FileBlock.ReadFileBlock(reader, Header.PointerSize);

                blocksRead++;

                if(block is null)
                {
                    f.printError($"Failed to read block {blocksRead} with code {blockCode}");
                    return null;
                }
                fileBlocks.Add(block);

                blockCode = block.Code; // Neccesary for ending the while loop.
            }

            return fileBlocks;
        }

        private Dictionary<ulong,Structure[]> CreateMemoryMap()
        {
            Dictionary<ulong,Structure[]> memoryMap = new Dictionary<ulong,Structure[]>();

            for (int i = 0; i < FileBlocks.Count; i++)
            {
                FileBlock blockToBeParsed = FileBlocks[i];
                Structure[] temp = Structure.ParseFileBlock(blockToBeParsed,i,StructureDNA);
            }

            return memoryMap;
        }
    }
}
