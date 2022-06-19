using System.Collections.Generic;

namespace BlenderToUnity
{
    [System.Serializable]
    public struct DNAStruct
    {

        public int TypeIndex;

        public string TypeName;

        public int NumberOfFields;
  
        public List<DNAField> DnaFields;

        public DNAStruct(int typeIndex, string typeName, int numberOfFields, List<DNAField> fields)
        {
            this.TypeIndex = typeIndex;
            this.TypeName = typeName;
            this.NumberOfFields = numberOfFields;
            this.DnaFields = fields;
        }
    }
}
