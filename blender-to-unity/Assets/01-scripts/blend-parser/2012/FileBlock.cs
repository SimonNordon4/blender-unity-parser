using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class FileBlock
{
        /// <summary>
        /// Four-character code indicating the type of data in the FileBlock.
        /// </summary>
        public string Code;
        /// <summary>
        /// Size of the FileBlock's data, in bytes.
        /// </summary>
        public int Size;
        /// <summary>
        /// Index in the structures defined by SDNA used to decode the data in this FileBlock.
        /// </summary>
        public int SDNAIndex;
        /// <summary>
        /// Number of objects to decode.
        /// </summary>
        public int Count;

        /// <summary>
        /// Raw data contained in the FileBlock.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Old memory address of the FileBlock; used for lists and pointer references.
        /// </summary>
        public ulong OldMemoryAddress;

        public static FileBlock Read(BinaryReader reader, int pointerSize)
        {
            // Enforce PointerSize.
            if(pointerSize != 4 && pointerSize != 8) { throw new InvalidDataException("PointerSize must be 4 or 8."); }

            string code = new string(reader.ReadChars(4));
            int size = reader.ReadInt32();
            ulong address = pointerSize == 4 ? reader.ReadUInt32() : reader.ReadUInt64();
            int sdnaIndex = reader.ReadInt32();
            int count = reader.ReadInt32();
            byte[] data = reader.ReadBytes(size);

            FileBlock block = code == "DNA1" ? new StructureDNA(code, size, sdnaIndex, count, data) : // super hacky, just fix with a ReadStructure or something.
                                               new FileBlock(code, size, sdnaIndex, count, data);

            // Blocks start every 4 bytes, in the case we haven't reached the end of the block, we need to continue reading over values.
            while(reader.BaseStream.Position % 4 != 0 && reader.BaseStream.Position < reader.BaseStream.Length)
            {
                reader.ReadByte();
            }

            block.OldMemoryAddress = address;

            return block;
        }

        public FileBlock(string code, int size, int sdnaIndex, int count, byte[] data)
        {
            Code = code;
            Size = size;
            SDNAIndex = sdnaIndex;
            Count = count;
            Data = data;
        }
}