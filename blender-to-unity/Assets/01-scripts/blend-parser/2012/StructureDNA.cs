using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// DNA1, also known as "Structure DNA", is a special block in
/// .blend file, which contains machine-readable specifications of
/// all other structures used in this .blend file.
/// </summary>
[System.Serializable]
public class StructureDNA : FileBlock
{      
         /// <summary>
        /// List of all the names contained in SDNA.
        /// </summary>
        public List<string> NameList { get; private set; }
        
        /// <summary>
        /// List of all the types and their sizes contained in SDNA.
        /// </summary>
        //public List<TypeDefinition> TypeList { get; private set; }
        
        /// <summary>
        /// List of all the names of the types in SDNA; used primarily for BlenderType and BlenderField's constructors.
        /// </summary>
        public List<string> TypeNameList { get; private set; }
        
        /// <summary>
        /// List of all structures defined in SDNA.
        /// </summary>
        //public List<StructureDefinition> StructureList { get; private set; }
        
        /// <summary>
        /// List of all of the structures' types by index in TypeList/TypeNameList; used primarily for BlenderType and BlenderField's constructors.
        /// </summary>
        public List<short> StructureTypeIndices { get; private set; }

        public StructureDNA(string code, int size, int sdna, int count, byte[] data) : base(code,size,sdna,count,data)
        {
            int position = 0;
            position += 4; // skips over SDNA (already assumed since we're parsing this block.)

            // get a list of names.
            ReadNameList(ref position);
        }

        private void ReadNameList(ref int position)
        {
            char[] type;
            List<char> tempCharList;
            
            // Check name is valid.
            type = new char[4]
            {
                Encoding.ASCII.GetChars(Data,position++,1)[0],
                Encoding.ASCII.GetChars(Data,position++,1)[0],
                Encoding.ASCII.GetChars(Data,position++,1)[0],
                Encoding.ASCII.GetChars(Data,position++,1)[0]
            };

            if(type[0] != 'N' || type[1] != 'A' || type[2] != 'M' || type[3] != 'E')
            {
                throw new InvalidOperationException("Failed reading SDNA: names could not be read.");
            }

            int numberOfNames = BitConverter.ToInt32(Data,position);

            // Get to end of current parse.
            position += 4;
            while( position % 4 !=0 ) position++;

            NameList = new List<string>(numberOfNames);
            tempCharList = new List<char>();

            for (int i = 0; i < numberOfNames; i++)
            {
                char c;
                do
                {
                    c = Encoding.ASCII.GetChars(Data,position++,1)[0];
                    tempCharList.Add(c);   
                }
                while (c != '\0');

                tempCharList.RemoveAt(tempCharList.Count - 1); // remove terminator.
                NameList.Add(new string(tempCharList.ToArray()));
                tempCharList.Clear();
            }
        }
}