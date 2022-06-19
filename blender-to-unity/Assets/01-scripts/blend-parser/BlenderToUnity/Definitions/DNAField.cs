using System;
using System.Linq;

namespace BlenderToUnity
{
    [System.Serializable]
    public struct DNAField
    {
        // TODO - add Array Definition Support.
        // TODO - Separate out array and pointer def. It's either an array of pointers, or a array of values.

        public int TypeIndex;
        public int FieldNameIndex;
        public string Type;
        public string FieldName;
        public short FieldSize;

        public FieldContext Context;

        public DNAField(int typeIndex, int fieldNameIndex, string type, string fieldName, short fieldSize, int pointerSize)
        {
            this.TypeIndex = typeIndex;
            this.FieldNameIndex = fieldNameIndex;
            this.Type = type;
            this.FieldName = fieldName;
            this.FieldSize = fieldSize;

            var context = FieldContext.Value;
            
            int pointerChars = fieldName.Count(c => c == '*');
            switch(pointerChars)
            {
                case 0:
                    context = FieldContext.Value;
                    break;
                case 1:
                    context = FieldContext.Pointer;
                    break;
                case 2:
                    context = FieldContext.Pointer2;
                    break;
                case 3:
                    context = FieldContext.Pointer3;
                    break;
            }

            int arrayChars = fieldName.Count(c => c == '[');
            switch(arrayChars)
            {
                case 1:
                    context |= FieldContext.Array;
                    break;
                case 2:
                    context |= FieldContext.Array2D;
                    break;
                case 3:
                    context |= FieldContext.Array3D;
                    break;
            }

            // TODO set field Size to 4 or 8 depending ont he pointer size..... make sure evrything has access to everything??
            if(context == FieldContext.Pointer)
            {
                FieldSize = (short)pointerSize;
            }

            this.Context = context;
        }
    }

    /// <summary>
    /// Field context helps us establish a fields attributes. Usually in regards to pointers and arrays.
    /// <remarks>worst case: ***someItem[4][3][2]</remarks>
    /// </summary>
    [Flags]
    public enum FieldContext : int
    {
        /// <summary>
        /// Field is the actual value.
        /// </summary>
        Value = 0, 
        /// <summary>
        /// Field is pointer to the value.
        /// </summary>
        Pointer = 1, 
        /// <summary>
        /// Field is pointer to the pointer to the value.
        /// </summary>
        Pointer2 = 2,
        /// <summary>
        /// Field is pointer to the pointer to the pointer to the value.
        /// </summary>
        Pointer3 = 4, 
        /// <summary>
        /// Field is an array.
        /// </summary>
        Array = 8,
        /// <summary>
        /// Field is an array.
        /// </summary>
        Array2D = 16, // 2 dimensional array
        /// <summary>
        /// Field is an array.
        /// </summary>
        Array3D = 32, // 3 dimensional array
    }
}
