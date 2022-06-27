using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace BlenderToUnity
{
    [System.Serializable]
    public class FileBlock
    {
        /// <summary>
        /// Binary stream position at the start of the block in the file.
        /// </summary>
        [field: SerializeField]
        public long BlockStartPosition {get; set;} = 0;
        /// <summary>
        /// The index of the block. The nth index will show up at n blocks in the file.
        /// </summary>
        [field: SerializeField]
        public int BlockIndex {get; set;}

        [field: SerializeField]
        public string Code{get; set;}
        [field: SerializeField]
        public int LenBody{get; set;}
        [field: SerializeField]
        public long OldMemoryAddress{get; set;}

        /// <summary>
        /// The DNAStruct Index of the block.
        /// <remarks>Take note, it's the index of the DNASTRUCT not the DNATYPE. 
        /// SDNAIndex of 309 is DNAStructs[309] = FileGlobal which has a DNAType index of 438
        /// </remarks>
        /// </summary>
        /// <value></value>
        [field: SerializeField]
        public int SDNAIndex{get; set;}
        [field: SerializeField]
        public int Count{get; set;}
        public byte[] Body{get; set;}

        // Structures represent the parsed data of a FileBlock.
        [field: SerializeField]
        public Structure[] Structures {get; set;}

        /// <summary>
        /// Read a single FileBlock from a blend file.
        /// </summary>
        /// <param name="reader">Current Binary Reader</param>
        /// <param name="pointerSize">pointer size of the file.</param>
        /// <param name="code">The Block Code</param>
        /// <param name="fileBlock">Return the file block.</param>
        /// <returns></returns>
        public static FileBlock ReadFileBlock(BinaryReader reader, int pointerSize)
        {
            // Input Checking.
            if (pointerSize != 4 && pointerSize != 8)
            {
                f.printError("Unsupported pointer size: " + pointerSize);
                return null;
            }

            var fileBlock = new FileBlock();

            fileBlock.BlockStartPosition = reader.BaseStream.Position;
            fileBlock.Code = new string(reader.ReadChars(4));
            fileBlock.LenBody = reader.ReadInt32();
            fileBlock.OldMemoryAddress = pointerSize == 4 ? reader.ReadInt32() : reader.ReadInt64();
            fileBlock.SDNAIndex = reader.ReadInt32();
            fileBlock.Count = reader.ReadInt32();
            fileBlock.Body = reader.ReadBytes(fileBlock.LenBody);

            // Blocks end on multiple of 4's so we have to burn empty bytes until we hit it.
            while(reader.BaseStream.Position % 4 != 0)
            {
                reader.ReadByte();
            }

            return fileBlock;
        }
    
        public void ParseFileBlock(BlenderFile file)
        {
            // Warning. FileBlocks are always STRUCTDNAs. SDNAINXEX refers to structures.
            var type = file.StructureDNA.DNAStructs[SDNAIndex].TypeName;

            //f.print($"Block {this.BlockIndex}:{this.Code} sdna: {this.SDNAIndex} ({type})");

            // Nothing to parse.
            if (!BlockIsParseable())
            {
                Structures = null;
                return;
            }

            int numberOfStructs = Count;
            int lenStruct = LenBody / Count;
            Structures = new Structure[numberOfStructs];

            var dnaType = file.StructureDNA.DNAStructs[SDNAIndex];

            //f.print($"Block {BlockIndex}: type {dnaType.TypeName}");

            if (numberOfStructs == 1)
            {
                Structures[0] = new Structure(Body, dnaType, file);
                return;
            }

            for (int i = 0; i < numberOfStructs; i++)
            {
                // intellicode suggested this lol
                var structBody = Body.Skip(i * lenStruct).Take(lenStruct).ToArray();
                var structure = new Structure(structBody, dnaType, file);
                Structures[i] = structure;
            }

            return;

        }

        /// <summary>
        /// Check to see if the provided file block can be parsed as a structure.
        /// </summary>
        private bool BlockIsParseable()
        {
            // Voids.
            if (this.Count <= 0 || this.LenBody <= 0) return false;
            // Special Blocks.
            if (this.Code == "TEST" || this.Code == "REND" || this.Code == "DNA1" || this.Code == "ENDB") return false;
            return true;
        }
    }


}