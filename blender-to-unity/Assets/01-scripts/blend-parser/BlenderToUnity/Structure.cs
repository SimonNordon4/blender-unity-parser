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

                // Get the byte[] containg the value of this field.
                byte[] fieldBody = new byte[fieldSize];
                Array.Copy(structBody, startReadPosition, fieldBody, 0, fieldSize);
                // increment the read position to the next field.
                startReadPosition += fieldSize;

                // Create the field and add it to the list, only if it's a primitive for now.
                if (dnaField.Context == FieldContext.Value)
                {
                    var field = ReadPrimitiveValue(fieldBody, dnaField, file);
                    fields.Add(field);
                }
            }
            return fields;
        }

        private byte[] FieldBodyFromStructBody(byte[] structBody, ref int startReadPosition, short fieldBodyLength)
        {
            byte[] fieldBody = new byte[fieldBodyLength];

            return fieldBody;
        }

        /// <summary>
        /// Return a the value of a field that is a primitive and a value.
        /// </summary>
        private IField ReadPrimitiveValue(byte[] fieldBody, DNAField dnaField, BlenderFile file)
        {
            // void check.
            if(fieldBody.Length == 0)
            {
                return null;
            }
            var fieldType = dnaField.Type;
            if (fieldType == "char")
            {
                f.print("creating a char!");
                char value = System.Text.Encoding.ASCII.GetChars(fieldBody)[0];
                var field = new Field<char>(value, dnaField);
                return field;
            }

            return null;
        }
    }
}