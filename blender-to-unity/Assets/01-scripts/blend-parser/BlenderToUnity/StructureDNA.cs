using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace BlenderToUnity
{
    /// <summary>
    /// Contains data from a the special "DNA1" File Block. This block contains all the important information about the other file blocks in the .blend file.
    /// </summary>
    [System.Serializable]
    public class StructureDNA
    {
        [field: SerializeField]
        public List<TypeDefinition> TypeDefinitions { get; private set; } = new List<TypeDefinition>();
        
        [field: SerializeField]
        public List<StructureDefinition> StructureDefinitions { get; private set; } = new List<StructureDefinition>();

        public StructureDNA(BlenderFile file)
        {
            var dna1 = file.DNA1Block;
            this.TypeDefinitions = GetTypeDefintiions(dna1);
            this.StructureDefinitions = GetStructureDefinitions(dna1);
        }

        private List<TypeDefinition> GetTypeDefintiions(DNA1Block dna1)
        {
            int numberOfTypes = dna1.TypeNames.Count;

            var typeDefintions = new List<TypeDefinition>(numberOfTypes);
            for (int i = 0; i < numberOfTypes; i++)
            {
                var typeName = dna1.TypeNames[i];
                var typeSize = dna1.TypeSizes[i];

                // type is primitive if it doesn't exist in the DNAstructures
                var isStructureType = dna1.DNAStructs.Any(st => st.TypeIndex == (short)i);
                var typeIsPrimitive = !isStructureType;

                TypeDefinition typeDefinition = new TypeDefinition(typeName, typeSize, typeIsPrimitive);

                typeDefintions.Add(typeDefinition);
            }

            return typeDefintions;
        }

        private List<StructureDefinition> GetStructureDefinitions(DNA1Block dna1)
        {
            int numberOfStructures = dna1.DNAStructs.Count;

            var structureDefinitions = new List<StructureDefinition>(numberOfStructures);

            for (int i = 0; i < numberOfStructures; i++)
            {
                var typeIndex = dna1.DNAStructs[i].TypeIndex;
                var structureTypeDefintion = this.TypeDefinitions[typeIndex];
                List<FieldDefinition> fields = CreateFieldDefinitions(i, dna1);
                var structureDefinition = new StructureDefinition(structureTypeDefintion, fields);
                structureDefinitions.Add(structureDefinition);
            }

            return structureDefinitions;
        }

        /// <summary>
        /// Create FieldDefinitions for a given structure.
        /// </summary>
        /// <param name="index">The index of the Structure being assessed.</param>
        /// <returns>List of generated FieldDefinitions for that particular structure at index</returns>
        private List<FieldDefinition> CreateFieldDefinitions(int structureDefintionIndex, DNA1Block dna1)
        {
            DNAStruct structureType = dna1.DNAStructs[structureDefintionIndex];
            int numberOfFields = structureType.Fields.Count;

            List<FieldDefinition> fieldDefinitions = new List<FieldDefinition>(numberOfFields);

            for (int i = 0; i < numberOfFields; i++)
            {
                var structureField = structureType.Fields[i];
                var fieldName = dna1.Names[structureField.NameIndex];
                var fieldType = this.TypeDefinitions[structureField.TypeIndex];
                var fieldDefinition = new FieldDefinition(fieldName, fieldType);
                fieldDefinitions.Add(fieldDefinition);
            }

            return fieldDefinitions;
        }
    }
}
