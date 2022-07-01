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
            //f.print($"\tParsing Structure: {Type} index: {dnaStruct.TypeIndex}. bytes: {structBody.Length} fields: {dnaStruct.DnaFields.Count} sdnaIndex: {dnaStruct.TypeIndex}");

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

                //f.print($"\t\tParsing Field: {dnaField.FieldName} size: {fieldSize} bytes: {startReadPosition} / {structBody.Length}");

                // Read the field from the structBody.
                byte[] fieldBody = new byte[fieldSize];
                for (int j = 0; j < fieldSize; j++)
                {
                    fieldBody[j] = structBody[startReadPosition + j];
                }

                startReadPosition += fieldSize;

                // This where we can create fields based on the dnaField.
                var field = ParseField(fieldBody, dnaField);

                if (dnaField.IsArray)
                {
                    file.DebugFields.Add(field);
                }

                fields.Add(field);
            }

            //f.print($"\t\tParsing Fields Done: {startReadPosition} / {structBody.Length}");
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
                if (dnaField.IsArray)
                {
                    return null;
                }

                //return new FieldULong(fieldBody, dnaField);
            }

            // Field is Primitive Value
            if (dnaField.IsPrimitive)
            {
                // Array
                if (dnaField.IsArray)
                {
                    return ReadPrimitiveArray_NEW(fieldBody, dnaField);
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
                    return new FieldChar(dnaField.FieldName, (char)value);
                case "uchar":
                    return new FieldUChar(dnaField.FieldName, (byte)value);
                case "short":
                    return new FieldShort(dnaField.FieldName, (short)value);
                case "ushort":
                    return new FieldUShort(dnaField.FieldName, (ushort)value);
                case "int":
                    return new FieldInt(dnaField.FieldName, (int)value);
                case "uint":
                    return new FieldUInt(dnaField.FieldName, (uint)value);
                case "float":
                    return new FieldFloat(dnaField.FieldName, (float)value);
                case "double":
                    return new FieldDouble(dnaField.FieldName, (double)value);
                case "long":
                    return new FieldLong(dnaField.FieldName, (long)value);
                case "ulong":
                    return new FieldULong(dnaField.FieldName, (ulong)value);
                case "int64_t":
                    return new FieldLong(dnaField.FieldName, (long)value);
                case "uint64_t":
                    return new FieldULong(dnaField.FieldName, (ulong)value);
            }

            throw new Exception($"Unknown Primitive Type: {typeName}");
        }


        private IField ReadPrimitiveArray_NEW(byte[] fieldBody, DNAField dnaField)
        {
            // 1. First gather all values into a single array.

            IField fieldArrays;

            string typeName = dnaField.TypeName;

            // If we're dealing with a small pointer size, we need change the long and ulong accordingly.
            if (typeName == "long" && dnaField.PointerSize == 4) typeName = "int";
            if (typeName == "ulong" && dnaField.PointerSize == 4) typeName = "uint";

            

            switch (typeName)
            {
                case "char":
                    char[] chars = GetArrayValues<char>(fieldBody, dnaField);
                    // This will return fieldChars if 1D, or fieldArray(s) if multipleD.
                    var new_fieldArray = SplitArray<char[]>(chars, dnaField);
                    fieldArrays = new FieldChars(dnaField.FieldName, chars);
                    break;
                case "uchar":
                    byte[] uchars = GetArrayValues<byte>(fieldBody, dnaField);
                    fieldArrays = new FieldUChars(dnaField.FieldName, uchars);
                    break;
                case "short":
                    short[] shorts = GetArrayValues<short>(fieldBody, dnaField);
                    fieldArrays = new FieldShorts(dnaField.FieldName, shorts);
                    break;
                case "ushort":
                    ushort[] ushorts = GetArrayValues<ushort>(fieldBody, dnaField);
                    fieldArrays = new FieldUShorts(dnaField.FieldName, ushorts);
                    break;
                case "int":
                    int[] ints = GetArrayValues<int>(fieldBody, dnaField);
                    fieldArrays = new FieldInts(dnaField.FieldName, ints);
                    break;
                case "uint":
                    uint[] uints = GetArrayValues<uint>(fieldBody, dnaField);
                    fieldArrays = new FieldUInts(dnaField.FieldName, uints);
                    break;
                case "float":
                    float[] floats = GetArrayValues<float>(fieldBody, dnaField);
                    fieldArrays = new FieldFloats(dnaField.FieldName, floats);
                    break;
                case "double":
                    double[] doubles = GetArrayValues<double>(fieldBody, dnaField);
                    fieldArrays = new FieldDoubles(dnaField.FieldName, doubles);
                    break;
                case "long":
                    long[] longs = GetArrayValues<long>(fieldBody, dnaField);
                    fieldArrays = new FieldLongs(dnaField.FieldName, longs);
                    break;
                case "ulong":
                    ulong[] ulongs = GetArrayValues<ulong>(fieldBody, dnaField);
                    fieldArrays = new FieldULongs(dnaField.FieldName, ulongs);
                    break;
                case "int64_t":
                    long[] int64_ts = GetArrayValues<long>(fieldBody, dnaField);
                    fieldArrays = new FieldLongs(dnaField.FieldName, int64_ts);
                    break;
                case "uint64_t":
                    ulong[] uint64_ts = GetArrayValues<ulong>(fieldBody, dnaField);
                    fieldArrays = new FieldULongs(dnaField.FieldName, uint64_ts);
                    break;
                default:
                    throw new System.Exception($"Unknown Primitive Type: {typeName}");
            }

            // If there's only 1 array, we're done here and can return the field.
            if (dnaField.ArrayDepth == 1) return fieldArrays;

            // Otherwise we have to continue breaking it up into it's multi array.

            // this is to get the starting point of the second array. Is always 0 if it's a 1D array.
            // eg [x][y] => 2 - 2 == 0 == x. [x][y][z][a] => 4 - 2 = 2 == z etc
            int SecondLastArrayIndex = dnaField.ArrayDepth - 2;

            for (int i = SecondLastArrayIndex; i > -1; i--)
            {
                int sizeOfEachArray = dnaField.ArrayLengths[i];
                int numberOfArrays = 1;
                int arrayIndex = i;
                while(arrayIndex > -1)
                {
                    numberOfArrays *= dnaField.ArrayLengths[arrayIndex];
                    arrayIndex--;
                }

                object[] bufferArray;
                for (int j = 0; j < numberOfArrays; j++)
                {
                    Array.Copy(values,j * sizeOfEachArray,bufferArray,0,sizeOfEachArray);
                }

                // we'll need another for loop here.
            }

            return null;
        }

        // Split array into multi dimensional arrys.
        private IField SplitArray<T>(T values, DNAField dNAField)
        {
            // return either a FieldArray or a FieldChars (if 1D).
            return default;
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
                    return new FieldChars(dnaField.FieldName, chars);
                case "uchar":
                    byte[] uchars = GetArrayValues<byte>(fieldBody, dnaField);
                    return new FieldUChars(dnaField.FieldName, uchars);
                case "short":
                    short[] shorts = GetArrayValues<short>(fieldBody, dnaField);
                    return new FieldShorts(dnaField.FieldName, shorts);
                case "ushort":
                    ushort[] ushorts = GetArrayValues<ushort>(fieldBody, dnaField);
                    return new FieldUShorts(dnaField.FieldName, ushorts);
                case "int":
                    int[] ints = GetArrayValues<int>(fieldBody, dnaField);
                    return new FieldInts(dnaField.FieldName, ints);
                case "uint":
                    uint[] uints = GetArrayValues<uint>(fieldBody, dnaField);
                    return new FieldUInts(dnaField.FieldName, uints);
                case "float":
                    float[] floats = GetArrayValues<float>(fieldBody, dnaField);
                    return new FieldFloats(dnaField.FieldName, floats);
                case "double":
                    double[] doubles = GetArrayValues<double>(fieldBody, dnaField);
                    return new FieldDoubles(dnaField.FieldName, doubles);
                case "long":
                    long[] longs = GetArrayValues<long>(fieldBody, dnaField);
                    return new FieldLongs(dnaField.FieldName, longs);
                case "ulong":
                    ulong[] ulongs = GetArrayValues<ulong>(fieldBody, dnaField);
                    return new FieldULongs(dnaField.FieldName, ulongs);
                case "int64_t":
                    long[] int64_ts = GetArrayValues<long>(fieldBody, dnaField);
                    return new FieldLongs(dnaField.FieldName, int64_ts);
                case "uint64_t":
                    ulong[] uint64_ts = GetArrayValues<ulong>(fieldBody, dnaField);
                    return new FieldULongs(dnaField.FieldName, uint64_ts);
            }
            return null;
            throw new Exception($"Unknown Primitive Type: {typeName}");
        }

        private IField ReadPrimitiveArray2D(byte[] fieldBody, DNAField dnaField)
        {
            string typeName = dnaField.TypeName;

            // If we're dealing with a small pointer size, we need change the long and ulong accordingly.
            if (typeName == "long" && dnaField.PointerSize == 4) typeName = "int";
            if (typeName == "ulong" && dnaField.PointerSize == 4) typeName = "uint";

            switch (typeName)
            {
                case "char":
                    char[] chars = GetArrayValues<char>(fieldBody, dnaField);
                    char[][] chars2D = SplitArray(chars, dnaField.ArrayLengths[0]);
                    return new FieldChars2D(dnaField.FieldName, chars2D);
                case "uchar":
                    byte[] uchars = GetArrayValues<byte>(fieldBody, dnaField);
                    byte[][] uchars2D = SplitArray(uchars, dnaField.ArrayLengths[0]);
                    return new FieldUChars2D(dnaField.FieldName, uchars2D);
                case "short":
                    short[] shorts = GetArrayValues<short>(fieldBody, dnaField);
                    short[][] shorts2D = SplitArray(shorts, dnaField.ArrayLengths[0]);
                    return new FieldShorts2D(dnaField.FieldName, shorts2D);
                case "ushort":
                    ushort[] ushorts = GetArrayValues<ushort>(fieldBody, dnaField);
                    ushort[][] ushorts2D = SplitArray(ushorts, dnaField.ArrayLengths[0]);
                    return new FieldUShorts2D(dnaField.FieldName, ushorts2D);
                case "int":
                    int[] ints = GetArrayValues<int>(fieldBody, dnaField);
                    int[][] ints2D = SplitArray(ints, dnaField.ArrayLengths[0]);
                    return new FieldInts2D(dnaField.FieldName, ints2D);
                case "uint":
                    uint[] uints = GetArrayValues<uint>(fieldBody, dnaField);
                    uint[][] uints2D = SplitArray(uints, dnaField.ArrayLengths[0]);
                    return new FieldUInts2D(dnaField.FieldName, uints2D);
                case "float":
                    float[] floats = GetArrayValues<float>(fieldBody, dnaField);
                    float[][] floats2D = SplitArray(floats, dnaField.ArrayLengths[0]);
                    return new FieldFloats2D(dnaField.FieldName, floats2D);
                case "double":
                    double[] doubles = GetArrayValues<double>(fieldBody, dnaField);
                    double[][] doubles2D = SplitArray(doubles, dnaField.ArrayLengths[0]);
                    return new FieldDoubles2D(dnaField.FieldName, doubles2D);
                case "long":
                    long[] longs = GetArrayValues<long>(fieldBody, dnaField);
                    long[][] longs2D = SplitArray(longs, dnaField.ArrayLengths[0]);
                    return new FieldLongs2D(dnaField.FieldName, longs2D);
                case "ulong":
                    ulong[] ulongs = GetArrayValues<ulong>(fieldBody, dnaField);
                    ulong[][] ulongs2D = SplitArray(ulongs, dnaField.ArrayLengths[0]);
                    return new FieldULongs2D(dnaField.FieldName, ulongs2D);
                case "int64_t":
                    long[] int64_ts = GetArrayValues<long>(fieldBody, dnaField);
                    long[][] int64_ts2D = SplitArray(int64_ts, dnaField.ArrayLengths[0]);
                    return new FieldLongs2D(dnaField.FieldName, int64_ts2D);
                case "uint64_t":
                    ulong[] uint64_ts = GetArrayValues<ulong>(fieldBody, dnaField);
                    ulong[][] uint64_ts2D = SplitArray(uint64_ts, dnaField.ArrayLengths[0]);
                    return new FieldULongs2D(dnaField.FieldName, uint64_ts2D);
            }
            return null;
            throw new Exception($"Unknown Primitive Type: {typeName}");
        }

        private IField ReadPrimitiveArray3D(byte[] fieldBody, DNAField dnaField)
        {
            string typeName = dnaField.TypeName;

            // If we're dealing with a small pointer size, we need change the long and ulong accordingly.
            if (typeName == "long" && dnaField.PointerSize == 4) typeName = "int";
            if (typeName == "ulong" && dnaField.PointerSize == 4) typeName = "uint";

            switch (typeName)
            {
                case "char":
                    char[] chars = GetArrayValues<char>(fieldBody, dnaField);
                    char[][] chars2D = SplitArray(chars, dnaField.ArrayLengths[1]);
                    char[][][] chars3D = SplitArray(chars2D, dnaField.ArrayLengths[0]);
                    return new FieldChars3D(dnaField.FieldName, chars3D);
                case "uchar":
                    byte[] uchars = GetArrayValues<byte>(fieldBody, dnaField);
                    byte[][] uchars2D = SplitArray(uchars, dnaField.ArrayLengths[1]);
                    byte[][][] uchars3D = SplitArray(uchars2D, dnaField.ArrayLengths[0]);
                    return new FieldUChars3D(dnaField.FieldName, uchars3D);
                case "short":
                    short[] shorts = GetArrayValues<short>(fieldBody, dnaField);
                    short[][] shorts2D = SplitArray(shorts, dnaField.ArrayLengths[1]);
                    short[][][] shorts3D = SplitArray(shorts2D, dnaField.ArrayLengths[0]);
                    return new FieldShorts3D(dnaField.FieldName, shorts3D);
                case "ushort":
                    ushort[] ushorts = GetArrayValues<ushort>(fieldBody, dnaField);
                    ushort[][] ushorts2D = SplitArray(ushorts, dnaField.ArrayLengths[1]);
                    ushort[][][] ushorts3D = SplitArray(ushorts2D, dnaField.ArrayLengths[0]);
                    return new FieldUShorts3D(dnaField.FieldName, ushorts3D);
                case "int":
                    int[] ints = GetArrayValues<int>(fieldBody, dnaField);
                    int[][] ints2D = SplitArray(ints, dnaField.ArrayLengths[1]);
                    int[][][] ints3D = SplitArray(ints2D, dnaField.ArrayLengths[0]);
                    return new FieldInts3D(dnaField.FieldName, ints3D);
                case "uint":
                    uint[] uints = GetArrayValues<uint>(fieldBody, dnaField);
                    uint[][] uints2D = SplitArray(uints, dnaField.ArrayLengths[1]);
                    uint[][][] uints3D = SplitArray(uints2D, dnaField.ArrayLengths[0]);
                    return new FieldUInts3D(dnaField.FieldName, uints3D);
                case "float":
                    float[] floats = GetArrayValues<float>(fieldBody, dnaField);
                    float[][] floats2D = SplitArray(floats, dnaField.ArrayLengths[1]);
                    float[][][] floats3D = SplitArray(floats2D, dnaField.ArrayLengths[0]);
                    return new FieldFloats3D(dnaField.FieldName, floats3D);
                case "double":
                    double[] doubles = GetArrayValues<double>(fieldBody, dnaField);
                    double[][] doubles2D = SplitArray(doubles, dnaField.ArrayLengths[1]);
                    double[][][] doubles3D = SplitArray(doubles2D, dnaField.ArrayLengths[0]);
                    return new FieldDoubles3D(dnaField.FieldName, doubles3D);
                case "long":
                    long[] longs = GetArrayValues<long>(fieldBody, dnaField);
                    long[][] longs2D = SplitArray(longs, dnaField.ArrayLengths[1]);
                    long[][][] longs3D = SplitArray(longs2D, dnaField.ArrayLengths[0]);
                    return new FieldLongs3D(dnaField.FieldName, longs3D);
                case "ulong":
                    ulong[] ulongs = GetArrayValues<ulong>(fieldBody, dnaField);
                    ulong[][] ulongs2D = SplitArray(ulongs, dnaField.ArrayLengths[1]);
                    ulong[][][] ulongs3D = SplitArray(ulongs2D, dnaField.ArrayLengths[0]);
                    return new FieldULongs3D(dnaField.FieldName, ulongs3D);
                case "int64_t":
                    long[] int64_ts = GetArrayValues<long>(fieldBody, dnaField);
                    long[][] int64_ts2D = SplitArray(int64_ts, dnaField.ArrayLengths[1]);
                    long[][][] int64_ts3D = SplitArray(int64_ts2D, dnaField.ArrayLengths[0]);
                    return new FieldLongs3D(dnaField.FieldName, int64_ts3D);
                case "uint64_t":
                    ulong[] uint64_ts = GetArrayValues<ulong>(fieldBody, dnaField);
                    ulong[][] uint64_ts2D = SplitArray(uint64_ts, dnaField.ArrayLengths[1]);
                    ulong[][][] uint64_ts3D = SplitArray(uint64_ts2D, dnaField.ArrayLengths[0]);
                    return new FieldULongs3D(dnaField.FieldName, uint64_ts3D);

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
            int numberOfValues = 1;

            for (int i = 0; i < dnaField.ArrayDepth; i++) { numberOfValues *= dnaField.ArrayLengths[i]; }

            int fieldTypeSize = dnaField.FieldSize / numberOfValues; // Size of each value in the array (char[64] has fieldsize of 64, but fieldTypeSize of 1 for example).

            T[] values = new T[numberOfValues];

            for (int i = 0; i < numberOfValues; i++)
            {
                byte[] fieldValueBody = fieldBody.Skip(i * fieldTypeSize).Take(fieldTypeSize).ToArray();
                values[i] = (T)PrimitiveFunctionsMap[typeName](fieldValueBody);
            }

            return values;
        }

        private T[][] SplitArray<T>(T[] array, int arrayLength)
        {
            int numberOfArrays = arrayLength;
            T[][] arrays = new T[numberOfArrays][];
            for (int i = 0; i < numberOfArrays; i++)
            {
                arrays[i] = array.Skip(i * arrayLength).Take(arrayLength).ToArray();
            }
            return arrays;
        }

        private T[][][] SplitArray<T>(T[][] array, int arrayLength)
        {
            int numberOfArrays = arrayLength;
            T[][][] arrays = new T[numberOfArrays][][];
            for (int i = 0; i < numberOfArrays; i++)
            {
                arrays[i] = array.Skip(i * arrayLength).Take(arrayLength).ToArray();
            }
            return arrays;
        }
    }
}