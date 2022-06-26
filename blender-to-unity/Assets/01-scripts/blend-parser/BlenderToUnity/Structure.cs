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
                    if(dnaField.ArrayDepth == 1)
                        return ReadPrimitiveArray(fieldBody, dnaField);
                    if(dnaField.ArrayDepth == 2)
                        //return ReadPrimitive2DArray(fieldBody, dnaField);
                    if(dnaField.ArrayDepth == 3)
                        //return ReadPrimitive3DArray(fieldBody, dnaField);
                    throw new System.Exception("This parser only supports up to 3Dimensional Arrays");
                }

                // Primitive Value.
                return ReadPrimitiveValue(fieldBody, dnaField);
            }

            // Field is Struct Value
            else
            {

            }

            return null;

        }


        public static Dictionary<string, Func<byte[], object>> PrimitiveFunctionsMap = new Dictionary<string, Func<byte[], object>>()
        {
            ["char"] = body => { return (char)Encoding.ASCII.GetChars(body)[0]; },
            ["uchar"] = body => { return (byte)body[0]; },
            ["short"] = body => { return (short)BitConverter.ToInt16(body); },
            ["ushort"] = body => { return (ushort)BitConverter.ToUInt16(body); },
            ["int"] = body => { return (int)BitConverter.ToInt32(body); },
            ["uint"] = body => { return (uint)BitConverter.ToUInt32(body); },
            ["float"] = body => { return (float)BitConverter.ToSingle(body); },
            ["double"] = body => { return (double)BitConverter.ToDouble(body); },
            ["long"] = body => { return (long)BitConverter.ToInt64(body); },
            ["ulong"] = body => { return (ulong)BitConverter.ToUInt64(body); },
            ["int64_t"] = body => { return (long)BitConverter.ToInt64(body); },
            ["uint64_t"] = body => { return (ulong)BitConverter.ToUInt64(body); },
        };

        private IField ReadPrimitiveValue(byte[] fieldBody, DNAField dnaField)
        {
            string typeName = dnaField.TypeName;

            // If we're dealing with a small pointer size, we need change the long and ulong accordingly.
            if (typeName == "long" && dnaField.PointerSize == 4) typeName = "int";
            if (typeName == "ulong" && dnaField.PointerSize == 4) typeName = "uint";

            var value = PrimitiveFunctionsMap[typeName](fieldBody);

            switch (typeName)
            {
                case "char":
                    return new Field<char>((char)value);
                case "uchar":
                    return new Field<byte>((byte)value);
                case "short":
                    return new Field<short>((short)value);
                case "ushort":
                    return new Field<ushort>((ushort)value);
                case "int":
                    return new Field<int>((int)value);
                case "uint":
                    return new Field<uint>((uint)value);
                case "float":
                    return new Field<float>((float)value);
                case "double":
                    return new Field<double>((double)value);
                case "long":
                    return new Field<long>((long)value);
                case "ulong":
                    return new Field<ulong>((ulong)value);
                case "int64_t":
                    return new Field<long>((long)value);
                case "uint64_t":
                    return new Field<ulong>((ulong)value);
            }

            throw new Exception($"Unknown Primitive Type: {typeName}");
        }

        private IField ReadPrimitiveArray(byte[] fieldBody, DNAField dnaField)
        {
            string typeName = dnaField.TypeName;

            // If we're dealing with a small pointer size, we need change the long and ulong accordingly.
            if (typeName == "long" && dnaField.PointerSize == 4) typeName = "int";
            if (typeName == "ulong" && dnaField.PointerSize == 4) typeName = "uint";

            switch (typeName)
            {
                case "char":
                    char[] chars = GetArrayValues<char>(fieldBody, dnaField);
                    return new Field<char[]>(chars);
                case "uchar":
                    byte[] uchars = GetArrayValues<byte>(fieldBody, dnaField);
                    return new Field<byte[]>(uchars);
                case "short":
                    short[] shorts = GetArrayValues<short>(fieldBody, dnaField);
                    return new Field<short[]>(shorts);
                case "ushort":
                    ushort[] ushorts = GetArrayValues<ushort>(fieldBody, dnaField);
                    return new Field<ushort[]>(ushorts);
                case "int":
                    int[] ints = GetArrayValues<int>(fieldBody, dnaField);
                    return new Field<int[]>(ints);
                case "uint":
                    uint[] uints = GetArrayValues<uint>(fieldBody, dnaField);
                    return new Field<uint[]>(uints);
                case "float":
                    float[] floats = GetArrayValues<float>(fieldBody, dnaField);
                    return new Field<float[]>(floats);
                case "double":
                    double[] doubles = GetArrayValues<double>(fieldBody, dnaField);
                    return new Field<double[]>(doubles);
                case "long":
                    long[] longs = GetArrayValues<long>(fieldBody, dnaField);
                    return new Field<long[]>(longs);
                case "ulong":
                    ulong[] ulongs = GetArrayValues<ulong>(fieldBody, dnaField);
                    return new Field<ulong[]>(ulongs);
                case "int64_t":
                    long[] int64_ts = GetArrayValues<long>(fieldBody, dnaField);
                    return new Field<long[]>(int64_ts);
                case "uint64_t":
                    ulong[] uint64_ts = GetArrayValues<ulong>(fieldBody, dnaField);
                    return new Field<ulong[]>(uint64_ts);
            }

            throw new Exception($"Unknown Primitive Type: {typeName}");
        }

        /// <summary>
        /// Read an Array of Types from a byte array.
        /// </summary>
        /// <returns>Type Array to create a field with.</returns>
        private T[] GetArrayValues<T>(byte[] fieldBody, DNAField dnaField)
        {
            string typeName = dnaField.TypeName;
            int numberOfValues = dnaField.ArrayLengths[0]; // Total number of values in the 1D array.
            int fieldTypeSize = dnaField.FieldSize / numberOfValues; // Size of each value in the array (char[64] has fieldsize of 64, but fieldTypeSize of 1 for example).

            T[] values = new T[numberOfValues];

            for (int i = 0; i < numberOfValues; i++)
            {
                byte[] fieldValueBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
                values[i] = (T)PrimitiveFunctionsMap[typeName](fieldValueBody);
            }

            return values;
        }


        // private IField ReadPrimitiveArray(byte[] fieldBody, DNAField dnaField)
        // {
        //     string fieldType = dnaField.TypeName;
        //     int numberOfValues = dnaField.ArrayLengths[0]; // Total number of values in the 1D array.
        //     int fieldTypeSize = dnaField.FieldSize / numberOfValues; // Size of each value in the array (char[64] has fieldsize of 64, but fieldTypeSize of 1 for example).

        //     switch (fieldType)
        //     {
        //         case "char":
        //             char[] charValues = new char[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 char charValue = Encoding.ASCII.GetChars(partialFieldBody)[0];
        //                 charValues[i] = charValue;
        //             }
        //             return new Field<char[]>(charValues, fieldBody, dnaField);
        //         case "uchar":
        //             byte[] uCharValues = new byte[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 byte ucharValue = partialFieldBody[0];
        //                 uCharValues[i] = ucharValue;
        //             }
        //             return new Field<byte[]>(uCharValues, fieldBody, dnaField);
        //         case "short":
        //             short[] shortValues = new short[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 short shortValue = BitConverter.ToInt16(partialFieldBody);
        //                 shortValues[i] = shortValue;
        //             }
        //             return new Field<short[]>(shortValues, fieldBody, dnaField); ;
        //         case "ushort":

        //             ushort[] ushortValues = new ushort[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 ushort ushortValue = BitConverter.ToUInt16(partialFieldBody);
        //                 ushortValues[i] = ushortValue;
        //             }
        //             return new Field<ushort[]>(ushortValues, fieldBody, dnaField); ;
        //         case "int":
        //             int[] intValues = new int[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 int intValue = BitConverter.ToInt32(partialFieldBody);
        //                 intValues[i] = intValue;
        //             }
        //             return new Field<int[]>(intValues, fieldBody, dnaField); ;
        //         case "long":
        //             if (dnaField.PointerSize == 4)
        //             {
        //                 int[] longValues = new int[numberOfValues];
        //                 for (int i = 0; i < numberOfValues; i++)
        //                 {
        //                     byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                     int longValue = BitConverter.ToInt32(partialFieldBody);
        //                     longValues[i] = longValue;
        //                 }
        //                 return new Field<int[]>(longValues, fieldBody, dnaField);

        //             }
        //             else
        //             {
        //                 long[] longValues = new long[numberOfValues];
        //                 for (int i = 0; i < numberOfValues; i++)
        //                 {
        //                     byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                     long longValue = BitConverter.ToInt64(partialFieldBody);
        //                     longValues[i] = longValue;
        //                 }
        //                 return new Field<long[]>(longValues, fieldBody, dnaField);
        //             }
        //         case "ulong":
        //             if (dnaField.PointerSize == 4)
        //             {
        //                 uint[] longValues = new uint[numberOfValues];
        //                 for (int i = 0; i < numberOfValues; i++)
        //                 {
        //                     byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                     uint longValue = BitConverter.ToUInt32(partialFieldBody);
        //                     longValues[i] = longValue;
        //                 }
        //                 return new Field<uint[]>(longValues, fieldBody, dnaField);
        //             }
        //             else
        //             {
        //                 ulong[] longValues = new ulong[numberOfValues];
        //                 for (int i = 0; i < numberOfValues; i++)
        //                 {
        //                     byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                     ulong longValue = BitConverter.ToUInt64(partialFieldBody);
        //                     longValues[i] = longValue;
        //                 }
        //                 return new Field<ulong[]>(longValues, fieldBody, dnaField);
        //             }
        //         case "float":
        //             float[] floatValues = new float[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 float floatValue = BitConverter.ToSingle(partialFieldBody);
        //                 floatValues[i] = floatValue;
        //             }
        //             return new Field<float[]>(floatValues, fieldBody, dnaField); ;
        //         case "double":

        //             double[] doubleValues = new double[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 double doubleValue = BitConverter.ToDouble(partialFieldBody);
        //                 doubleValues[i] = doubleValue;
        //             }
        //             return new Field<double[]>(doubleValues, fieldBody, dnaField);
        //         case "int64_t":
        //             long[] longValues64 = new long[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 long longValue = BitConverter.ToInt64(partialFieldBody);
        //                 longValues64[i] = longValue;
        //             }
        //             return new Field<long[]>(longValues64, fieldBody, dnaField);
        //         case "uint64_t":

        //             ulong[] ulongValues64 = new ulong[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 ulong ulongValue = BitConverter.ToUInt64(partialFieldBody);
        //                 ulongValues64[i] = ulongValue;
        //             }
        //             return new Field<ulong[]>(ulongValues64, fieldBody, dnaField);
        //     }

        //     throw new SystemException($"Unknown Primitive Array Type: {fieldType}");
        // }

        // private IField ReadPrimitive2DArray(byte[] fieldBody, DNAField dnaField)
        // {
        //     string fieldType = dnaField.TypeName;
        //     int numberOfValues = dnaField.ArrayLengths[0] * dnaField.ArrayLengths[1]; // Total number of values in the 2D array.
        //     int fieldTypeSize = dnaField.FieldSize / numberOfValues; // Size of each value in the array (char[64] has fieldsize of 64, but fieldTypeSize of 1 for example).

        //     switch (fieldType)
        //     {
        //         case "char":
        //             char[][] charValues = new char[dnaField.ArrayLengths[0]][];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 char charValue = Encoding.ASCII.GetChars(partialFieldBody)[0];
        //                 charValues[i] = charValue;
        //             }
        //             return new Field<char[]>(charValues, fieldBody, dnaField);
        //         case "uchar":
        //             byte[] uCharValues = new byte[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 byte ucharValue = partialFieldBody[0];
        //                 uCharValues[i] = ucharValue;
        //             }
        //             return new Field<byte[]>(uCharValues, fieldBody, dnaField);
        //         case "short":
        //             short[] shortValues = new short[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 short shortValue = BitConverter.ToInt16(partialFieldBody);
        //                 shortValues[i] = shortValue;
        //             }
        //             return new Field<short[]>(shortValues, fieldBody, dnaField); ;
        //         case "ushort":

        //             ushort[] ushortValues = new ushort[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 ushort ushortValue = BitConverter.ToUInt16(partialFieldBody);
        //                 ushortValues[i] = ushortValue;
        //             }
        //             return new Field<ushort[]>(ushortValues, fieldBody, dnaField); ;
        //         case "int":
        //             int[] intValues = new int[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 int intValue = BitConverter.ToInt32(partialFieldBody);
        //                 intValues[i] = intValue;
        //             }
        //             return new Field<int[]>(intValues, fieldBody, dnaField); ;
        //         case "long":
        //             if (dnaField.PointerSize == 4)
        //             {
        //                 int[] longValues = new int[numberOfValues];
        //                 for (int i = 0; i < numberOfValues; i++)
        //                 {
        //                     byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                     int longValue = BitConverter.ToInt32(partialFieldBody);
        //                     longValues[i] = longValue;
        //                 }
        //                 return new Field<int[]>(longValues, fieldBody, dnaField);

        //             }
        //             else
        //             {
        //                 long[] longValues = new long[numberOfValues];
        //                 for (int i = 0; i < numberOfValues; i++)
        //                 {
        //                     byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                     long longValue = BitConverter.ToInt64(partialFieldBody);
        //                     longValues[i] = longValue;
        //                 }
        //                 return new Field<long[]>(longValues, fieldBody, dnaField);
        //             }
        //         case "ulong":
        //             if (dnaField.PointerSize == 4)
        //             {
        //                 uint[] longValues = new uint[numberOfValues];
        //                 for (int i = 0; i < numberOfValues; i++)
        //                 {
        //                     byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                     uint longValue = BitConverter.ToUInt32(partialFieldBody);
        //                     longValues[i] = longValue;
        //                 }
        //                 return new Field<uint[]>(longValues, fieldBody, dnaField);
        //             }
        //             else
        //             {
        //                 ulong[] longValues = new ulong[numberOfValues];
        //                 for (int i = 0; i < numberOfValues; i++)
        //                 {
        //                     byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                     ulong longValue = BitConverter.ToUInt64(partialFieldBody);
        //                     longValues[i] = longValue;
        //                 }
        //                 return new Field<ulong[]>(longValues, fieldBody, dnaField);
        //             }
        //         case "float":
        //             float[] floatValues = new float[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 float floatValue = BitConverter.ToSingle(partialFieldBody);
        //                 floatValues[i] = floatValue;
        //             }
        //             return new Field<float[]>(floatValues, fieldBody, dnaField); ;
        //         case "double":

        //             double[] doubleValues = new double[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 double doubleValue = BitConverter.ToDouble(partialFieldBody);
        //                 doubleValues[i] = doubleValue;
        //             }
        //             return new Field<double[]>(doubleValues, fieldBody, dnaField);
        //         case "int64_t":
        //             long[] longValues64 = new long[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 long longValue = BitConverter.ToInt64(partialFieldBody);
        //                 longValues64[i] = longValue;
        //             }
        //             return new Field<long[]>(longValues64, fieldBody, dnaField);
        //         case "uint64_t":

        //             ulong[] ulongValues64 = new ulong[numberOfValues];
        //             for (int i = 0; i < numberOfValues; i++)
        //             {
        //                 byte[] partialFieldBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
        //                 ulong ulongValue = BitConverter.ToUInt64(partialFieldBody);
        //                 ulongValues64[i] = ulongValue;
        //             }
        //             return new Field<ulong[]>(ulongValues64, fieldBody, dnaField);
        //     }

        //     throw new SystemException($"Unknown Primitive Array Type: {fieldType}");
        // }
    }
}