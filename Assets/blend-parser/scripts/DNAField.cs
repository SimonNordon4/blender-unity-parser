using System;
using System.Linq;

namespace BlenderToUnity
{
    [System.Serializable]
    public struct DNAField
    {
        /// <summary>
        /// Type Index. Primitive, Void or Structure.
        /// </summary>
        public int TypeIndex;
        public string TypeName;
        public int FieldNameIndex;
        public string FieldName;
        public int FieldSize;

        public bool IsPrimitive;
        public bool IsVoid;

        /// <summary>
        /// Is Field a pointer?
        /// </summary>
        public bool IsPointer;
        /// <summary>
        /// PointerDepth = 2 means **object;
        /// </summary>
        public int PointerDepth;

        /// <summary>
        /// The Files pointer size.
        /// </summary>
        public int PointerSize;

        public bool IsArray;
        public int ArrayDepth;
        public int[] ArrayLengths;
    }

}
