using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlenderToUnity
{
    [System.Serializable]
    public class Structure : IStructField
    {

        [field: SerializeField]
        public string Type { get; private set; }

        [field: SerializeReference]
        public List<IField> Fields { get; private set; }

        /// <summary>
        /// Structure is the parsed data of a FileBlock.
        /// </summary>
        /// <param name="partialBody">Section of the block body representing 1 structure.</param>
        /// <param name="definition">Structure definition associated with the block body</param>
        public Structure(byte[] structBody, DNAType dnaType, BlenderFile file)
        {
            Type = dnaType.DnaStruct.TypeName;
            f.print($"\tParsing Structure: {Type}. bytes: {structBody.Length} fields: {dnaType.DnaStruct.DnaFields.Count}");
            List<IField> fields = new List<IField>();
            Fields = ParseFields(structBody, dnaType, file);
        }

        private List<IField> ParseFields(byte[] structBody, DNAType dnaType, BlenderFile file)
        {
            List<IField> fields = new List<IField>();

            int startReadPosition = 0;
            for (int i = 0; i < dnaType.DnaStruct.NumberOfFields; i++)
            {
                DNAField dnaField = dnaType.DnaStruct.DnaFields[i];
                short fieldSize = dnaField.FieldSize;
                 f.print($"\t\tParsing Field: {dnaField.Type}. bytes: {fieldSize}");

                    // Pointer fields are pointers to other structures.
                // Get the byte[] containg the value of this field.
                byte[] fieldBody = new byte[fieldSize];

                // IF ITS A POINTER WE SKIP THE POINTER SIZE !!!! DONT READ THE OBJECT. 
                for(int j = 0; j < fieldSize; j++)
                {
                    fieldBody[j] = structBody[startReadPosition + j];
                }
                //Array.Copy(structBody, startReadPosition, fieldBody, 0, fieldSize);
                // increment the read position to the next field.
                startReadPosition += fieldSize;

                var field = new Field
                {
                    FieldName = dnaField.FieldName,
                    Type = dnaField.Type,
                    FieldSize = fieldSize,
                    FieldContext = dnaField.Context,
                    DnaField = dnaField,
                    FieldBody = fieldBody
                };

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