using UnityEngine;
namespace BlenderToUnity
{
    [System.Serializable]
    public class Header
    {
        /// <summary>
        /// Pointer size of the file. 4 for 32-bit, 8 for 64-bit.
        /// <remarks> Important for reader the bytes of the file. </remarks>
        /// </summary>
        [field: SerializeField]
         public int PointerSize { get; private set; }

        /// <summary>
        /// Version the blend was saved in. Only Valuable as a read-only property.
        /// </summary>
        [field: SerializeField] 
        public string VersionNumber { get; private set; }

        /// <summary>
        /// Endianess of the file. Only Valuable as a read-only property.
        /// </summary>
        [field: SerializeField] 
        public string Endian { get; private set; }

        public Header(int PointerSize, string VersionNumber, char Endian)
        {
            this.PointerSize = PointerSize;
            this.VersionNumber = VersionNumber;
            this.Endian = Endian == 'v' ? "Little" : "Big";
        }


    }
}