using System;
using System.Collections.Generic;

namespace BlenderToUnity
{
    /// <summary>
    /// Class for generating definitions based upon the sdna data.
    /// </summary>
    public class StructureCreator
    {
        public static bool GenerateSDNAData(ref StructureDNA sdna)
        {
            // Calculate Types.
            int numberOfTypes = sdna.Types.Count;

            sdna.TypeDefintions = new List<TypeDefinition>(numberOfTypes);
            for (int i = 0; i < numberOfTypes; i++)
            {
                TypeDefinition typeDefinition = new TypeDefinition(sdna.Types[i], sdna.TypeSizes[i], sdna);
                sdna.TypeDefintions.Add(typeDefinition);
            }

            // Calculate Structures.
            int NumberOfStructures = sdna.StructureTypeIndices.Count;

            sdna.StructureDefinitions = new List<StructureDefinition>(NumberOfStructures);
            for (int i = 0; i < NumberOfStructures; i++)
            {
                List<FieldDefinition> fields = CreateFieldDefinitions(i,sdna);
                StructureDefinition structureDefinition = new StructureDefinition(sdna.StructureTypeIndices[i], fields, sdna);
                sdna.StructureDefinitions.Add(structureDefinition);
            }

            // next step is to 'initialise' the structure definitions.
            // it looks like it's doing some recursive mumbo jumbo.

            return true;
        }

        /// <summary>
        /// Create FieldDefinitions for a given structure.
        /// </summary>
        /// <param name="index">The index of the Structure being assessed.</param>
        /// <returns>List of generated FieldDefinitions for that particular structure at index</returns>
        private static List<FieldDefinition> CreateFieldDefinitions(int index, StructureDNA sdna)
        {
            StructureTypeFieldContainer fieldContainer = sdna.StructureTypeFieldContainers[index];
            int numberOfFields = fieldContainer.StructureTypeFields.Count;

            List<FieldDefinition> fieldDefinitions = new List<FieldDefinition>(numberOfFields);

            for (int i = 0; i < numberOfFields; i++)
            {
                StructureTypeField structureField = fieldContainer.StructureTypeFields[i];
                FieldDefinition fieldDefinition = new FieldDefinition(structureField.Name, structureField.TypeOfField, sdna);
                fieldDefinitions.Add(fieldDefinition);
            }

            return fieldDefinitions;
        }
    }
}