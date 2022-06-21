using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlenderToUnity
{
    [System.Serializable]
    public class Structure : IStructField
    {
        // TODO - I'm happy with the defintions, but none of the structures work. 
        // TODO - !!!! ITS BECAUSE THE STRUCTURES DO NOT MATCH THE DNATYPES. Parsing GLOB parses the entirely wrong STRUCT TYPE!!!!
        [field: SerializeField]
        public string Type { get; private set; }

        [field: SerializeReference]
        public List<IField> Fields { get; private set; }

        /// <summary>
        /// Structure is the parsed data of a FileBlock.
        /// </summary>
        /// <param name="partialBody">Section of the block body representing 1 structure.</param>
        /// <param name="definition">Structure definition associated with the block body</param>
        public Structure(byte[] structBody, DNAStruct dnaStruct, BlenderFile file)
        {
            Type = dnaStruct.TypeName;
            f.print($"\tParsing Structure: {Type} index: {dnaStruct.TypeIndex}. bytes: {structBody.Length} fields: {dnaStruct.DnaFields.Count} sdnaIndex: {dnaStruct.TypeIndex}");

            List<IField> fields = new List<IField>();
            Fields = ParseFields(structBody, dnaStruct, file);
        }

        private List<IField> ParseFields(byte[] structBody, DNAStruct dnaStruct, BlenderFile file)
        {
            List<IField> fields = new List<IField>();

            // Start reading from index 0 of the structBody.
            int startReadPosition = 0;

            for (int i = 0; i < dnaStruct.NumberOfFields; i++)
            {
                // Get the dnaField for this particular Field.
                DNAField dnaField = dnaStruct.DnaFields[i];

                int fieldSize = dnaField.FieldSize;

                f.print($"\t\tParsing Field: {dnaField.FieldName} size: {fieldSize} bytes: {startReadPosition} / {structBody.Length}");

                // Read the field from the structBody.
                byte[] fieldBody = new byte[fieldSize];
                for(int j = 0; j < fieldSize; j++)
                { 
                    fieldBody[j] = structBody[startReadPosition + j];
                }

                startReadPosition += fieldSize;

                var field = new Field<int>(0,fieldBody, dnaField);

                fields.Add(field);
            }
            return fields;
        }

        private byte[] FieldBodyFromStructBody(byte[] structBody, ref int startReadPosition, short fieldBodyLength)
        {
            byte[] fieldBody = new byte[fieldBodyLength];

            return fieldBody;
        }

        // /// <summary>
        // /// Return a the value of a field that is a primitive and a value.
        // /// </summary>
        // private IField ReadPrimitiveValue(byte[] fieldBody, DNAField dnaField, BlenderFile file)
        // {
        //     // void check.
        //     if(fieldBody.Length == 0)
        //     {
        //         return null;
        //     }
        //     var fieldType = dnaField.Type;
        //     if (fieldType == "char")
        //     {
        //         f.print("creating a char!");
        //         char value = System.Text.Encoding.ASCII.GetChars(fieldBody)[0];
        //         var field = new Field<char>(value, dnaField);
        //         return field;
        //     }

        //     return null;
        // }
    }
}