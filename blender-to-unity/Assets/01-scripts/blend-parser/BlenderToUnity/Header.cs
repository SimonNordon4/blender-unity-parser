
namespace BlenderToUnity
{
    [System.Serializable]
    public class Header
    {
        // TODO. Check for valid values.
        public Header(int PointerSize, string VersionNumber, char Endian)
        {
            this.PointerSize = PointerSize;
            this.VersionNumber = VersionNumber;
            this.Endian = Endian == 'v' ? "Little" : "Big";
        }

        /// <summary>
        /// Pointer size of the file. 4 for 32-bit, 8 for 64-bit.
        /// <remarks> Important for reader the bytes of the file. </remarks>
        /// </summary>
        public int PointerSize;

        /// <summary>
        /// Version the blend was saved in. Only Valuable as a read-only property.
        /// </summary>
        public string VersionNumber;

        /// <summary>
        /// Endianess of the file. Only Valuable as a read-only property.
        /// </summary>
        public string Endian;
    }
}