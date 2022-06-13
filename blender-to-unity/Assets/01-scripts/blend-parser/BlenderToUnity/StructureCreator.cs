using System;
using System.Collections.Generic;

namespace BlenderToUnity
{
    /// <summary>
    /// Class for generating definitions based upon the sdna data.
    /// </summary>
    public class StructureCreator
    {
        public static bool CreateSDNAStructures(ref StructureDNA sdna)
        {
            int numberOfTypes = sdna.Types.Count;

            sdna.TypeDefintions = new List<TypeDefinition>(numberOfTypes);
            for (int i = 0; i < numberOfTypes; i++)
            {
                TypeDefinition typeDefinition = new TypeDefinition(sdna.Types[i], sdna.TypeSizes[i], sdna);
                sdna.TypeDefintions.Add(typeDefinition);
            }

            int NumberOfStructures = sdna.StructureTypeIndices.Count;

            sdna.StructureDefinitions = new List<StructureDefinition>(NumberOfStructures);
            for (int i = 0; i < NumberOfStructures; i++)
            {
                List<FieldDefinition> fields = CreateFieldDefinitions(i,sdna);
            }

            return true;
        }

        private static List<FieldDefinition> CreateFieldDefinitions(int index, StructureDNA sdna)
        {
            int numberOfStructureFields = sdna.StructureTypeFieldContainers[index].StructureTypeFields.Count;

            List<FieldDefinition> fieldDefinitions = new List<FieldDefinition>(numberOfStructureFields);

            for (int i = 0; i < numberOfStructureFields; i++)
            {
                
            }


            return fieldDefinitions;
        }
    }
}