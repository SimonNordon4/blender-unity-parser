using System.IO;

namespace BlenderToUnity
{
    [System.Serializable]
    public class FileBlock
    {
        /// <summary>
        /// Binary stream position at the start of the block in the file.
        /// </summary>
        public long BlockStartPosition = 0;

        public string Code;

        public int LenBody;

        public long OldMemoryAddress;

        public int SDNAIndex;

        public int Count;

        public byte[] Body;

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
    }


}