using System.Collections.Generic;

namespace BlenderToUnity
{
    [System.Serializable]
    public struct DNAStruct
    {

        public short TypeIndex;

        public string TypeName;

        public short StructSize;

        public int NumberOfFields;
  
        public List<DNAField> DnaFields;

        public DNAStruct(short typeIndex, string typeName, short structSize, short numberOfFields, List<DNAField> dnaFields)
        {
            TypeIndex = typeIndex;
            TypeName = typeName;
            StructSize = structSize;
            NumberOfFields = numberOfFields;
            DnaFields = dnaFields;
        }
    }
}
