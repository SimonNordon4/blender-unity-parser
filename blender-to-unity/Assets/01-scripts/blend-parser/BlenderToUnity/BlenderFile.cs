using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BlenderToUnity
{
    [System.Serializable]
    public class BlenderFile
    {
        /// <summary>
        /// Binary reader associated with this file while it's being read.
        /// </summary>

        public BinaryReader Reader { get; private set; }
        /// <summary>
        /// Source File Path of the .blend being read.
        /// </summary>
        [field: SerializeField]
        public string SourceFilePath { get; private set; }
        /// <summary>
        /// Parsed header of the .blend file.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("test")]
        public Header Header { get; private set; }

        /// <summary>
        /// List of uncast FileBlocks in the .blend file. We only have their sdna index and a data blob at this stage.
        /// </summary>
        [field: SerializeField]
        public List<FileBlock> FileBlocks { get; private set; } = new List<FileBlock>();

        /// <summary>
        /// Blend File Structure Definitions generated from the dna1 block. The contents within describe blender Types, Structures and Fields.
        /// </summary>
        [field: SerializeField]
        public StructureDNA StructureDNA { get; private set; }

        public BlenderFile(string path) : this(new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read)))
        {
            if (!File.Exists(path))
            {
                f.printError("Unable to find file: " + path);
                return;
            }

            f.print("Attempting to read Blender File: " + path);

            SourceFilePath = path;
        }

        public BlenderFile(BinaryReader reader)
        {
            Reader = reader;

            using (reader)
            {
                f.startwatch("Parse Blend");


                Header = ReadHeader(reader);

                FileBlocks = ReadFileBlocks(reader);
                
                //get structures and types
                StructureDNA = new StructureDNA();
                StructureDNA.ReadBlenderFile(this);


                for (int i = 0; i < FileBlocks.Count; i++)
                {
                    FileBlocks[i].ParseFileBlock(this);
                }

                reader.Close();

                f.stopwatch("Parse Blend");
            }
        }

        // private List<StructureRoot> GetStructures(BinaryReader reader)
        // {
        //     var structures = new List<StructureRoot>();

        //     foreach(var block in FileBlocks)
        //     {
        //         var structureRoot  = new StructureRoot(block,this);
        //         structures.Add(structureRoot);
        //     }

        //     return structures;
        // }

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

            if ((endianness == 'v' && !BitConverter.IsLittleEndian) || (endianness == 'V' && BitConverter.IsLittleEndian) || (endianness != 'v' && endianness != 'V'))
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
                block.BlockIndex = blocksRead;
                blocksRead++;

                if (block is null)
                {
                    f.printError($"Failed to read block {blocksRead} with code {blockCode}");
                    return null;
                }

                fileBlocks.Add(block);

                blockCode = block.Code; // Neccesary for ending the while loop.
            }

            return fileBlocks;
        }

        //     Dictionary<ulong,Structure[]> memoryMap = new Dictionary<ulong,Structure[]>();

        //     for (int i = 0; i < FileBlocks.Count; i++)
        //     {
        //         FileBlock blockToBeParsed = FileBlocks[i];
        //         Structure[] temp = Structure.ParseFileBlock(blockToBeParsed,i,StructureDNA);
        //     }

        //     return memoryMap;
        // }
    }
}
