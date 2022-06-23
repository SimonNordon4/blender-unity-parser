using System;
using System.Collections.Generic;
using System.Text;
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
                for (int j = 0; j < fieldSize; j++)
                {
                    fieldBody[j] = structBody[startReadPosition + j];
                }

                startReadPosition += fieldSize;

                // This where we can create fields based on the dnaField.
                var field = ParseField(fieldBody, dnaField);
                fields.Add(field);
            }

            f.print($"\t\tParsing Fields Done: {startReadPosition} / {structBody.Length}");
            if (startReadPosition - structBody.Length != 0)
                f.print($"\t\tParsing Field Error Unmatch:{dnaStruct.TypeName}");
            return fields;
        }


        private IField ParseField(byte[] fieldBody, DNAField dnaField)
        {
            if (dnaField.IsVoid) return null;

            // Field is Pointer.
            if (dnaField.IsPointer)
            {

            }

            // Field is Primitive Value
            if (dnaField.IsPrimitive)
            {
                // Array
                if (dnaField.IsArray)
                {

                }
                else
                {
                    f.print("Primitive Value Found");
                    return ReadPrimitiveValue(fieldBody, dnaField);
                }
            }

            // Field is Struct Value
            else
            {

            }

            return null;

        }

        private IField ReadPrimitiveValue(byte[] fieldBody, DNAField dnaField)
        {
            string fieldType = dnaField.TypeName;
            switch (fieldType)
            {
                case "char":
                    char charValue = Encoding.ASCII.GetChars(fieldBody)[0];
                    return new Field<char>(charValue, fieldBody, dnaField);
                case "uchar":
                    byte ucharValue = fieldBody[0];
                    return new Field<byte>(ucharValue, fieldBody, dnaField);
                case "short":
                    short shortValue = BitConverter.ToInt16(fieldBody);
                    return new Field<short>(shortValue, fieldBody, dnaField); ;
                case "ushort":
                    ushort ushortValue = BitConverter.ToUInt16(fieldBody);
                    return new Field<ushort>(ushortValue, fieldBody, dnaField); ;
                case "int":
                    int intValue = BitConverter.ToInt32(fieldBody);
                    return new Field<int>(intValue, fieldBody, dnaField); ;
                case "long":
                    if (dnaField.PointerSize == 4)
                    {
                        int longValue = BitConverter.ToInt32(fieldBody);
                        return new Field<int>(longValue, fieldBody, dnaField);
                    }
                    else
                    {
                        long longValue = BitConverter.ToInt64(fieldBody);
                        return new Field<long>(longValue, fieldBody, dnaField);
                    }
                case "ulong":
                    if (dnaField.PointerSize == 4)
                    {
                        uint longValue = BitConverter.ToUInt32(fieldBody);
                        return new Field<uint>(longValue, fieldBody, dnaField);
                    }
                    else
                    {
                        ulong longValue = BitConverter.ToUInt64(fieldBody);
                        return new Field<ulong>(longValue, fieldBody, dnaField);
                    }
                case "float":
                    float floatValue = BitConverter.ToSingle(fieldBody);
                    return new Field<float>(floatValue,fieldBody,dnaField);
                case "double":
                    double doubleValue = BitConverter.ToDouble(fieldBody);
                    return new Field<double>(doubleValue,fieldBody,dnaField);
                case "int64_t":
                    long int64_tValue = BitConverter.ToInt64(fieldBody);
                    return new Field<long>(int64_tValue,fieldBody,dnaField);
                case "uint64_t":
                    ulong uint64_tValue = BitConverter.ToUInt64(fieldBody);
                    return new Field<ulong>(uint64_tValue,fieldBody,dnaField);
            }

            throw new SystemException($"Unknown Primitive Type: {fieldType}");
        }
    }
}