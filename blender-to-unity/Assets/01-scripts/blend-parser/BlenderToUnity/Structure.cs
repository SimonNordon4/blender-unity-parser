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

                if (dnaField.IsArray && dnaField.ArrayDepth > 1)
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

            // Pointers
            if (dnaField.IsPointer && !dnaField.IsArray)
            {
                // return pointer.
            }
            if (dnaField.IsPointer && dnaField.IsArray)
            {
                // return pointer array.
            }

            // Primitives
            if (dnaField.IsPrimitive && !dnaField.IsArray)
            {
                return ReadPrimitiveValue(fieldBody, dnaField);
            }
            if (dnaField.IsPrimitive && dnaField.IsArray && dnaField.ArrayDepth == 1)
            {
                return ReadPrimitiveArray(fieldBody, dnaField);
            }
            if (dnaField.IsPrimitive && dnaField.IsArray && dnaField.ArrayDepth > 1)
            {
                return ReadPrimitiveMultiDimensionalArray(fieldBody, dnaField);
            }


            // Structures
            if (!dnaField.IsPointer && !dnaField.IsPrimitive && !dnaField.IsArray)
            {
                // return struct.
            }
            if (!dnaField.IsPointer && !dnaField.IsPrimitive && dnaField.IsArray)
            {
                // return struct array.
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

        public static Dictionary<string, Func<string, object, Field>> PrimitiveArrayFunctionMap = new Dictionary<string, Func<string, object, Field>>()
        {
            ["char"] = (fieldName, values) => { return new FieldChars(fieldName, (char[])values); },
            ["uchar"] = (fieldName, values) => { return new FieldUChars(fieldName, (byte[])values); },
            ["short"] = (fieldName, values) => { return new FieldShorts(fieldName, (short[])values); },
            ["ushort"] = (fieldName, values) => { return new FieldUShorts(fieldName, (ushort[])values); },
            ["int"] = (fieldName, values) => { return new FieldInts(fieldName, (int[])values); },
            ["uint"] = (fieldName, values) => { return new FieldUInts(fieldName, (uint[])values); },
            ["float"] = (fieldName, values) => { return new FieldFloats(fieldName, (float[])values); },
            ["double"] = (fieldName, values) => { return new FieldDoubles(fieldName, (double[])values); },
            ["long"] = (fieldName, values) => { return new FieldLongs(fieldName, (long[])values); },
            ["ulong"] = (fieldName, values) => { return new FieldULongs(fieldName, (ulong[])values); },
            ["int64_t"] = (fieldName, values) => { return new FieldLongs(fieldName, (long[])values); },
            ["uint64_t"] = (fieldName, values) => { return new FieldULongs(fieldName, (ulong[])values); },
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

        private IField ReadPrimitiveMultiDimensionalArray(byte[] fieldBody, DNAField dnaField)
        {
            string typeName = dnaField.TypeName;

            // If we're dealing with a small pointer size, we need change the long and ulong accordingly.
            if (typeName == "long" && dnaField.PointerSize == 4) typeName = "int";
            if (typeName == "ulong" && dnaField.PointerSize == 4) typeName = "uint";

            switch (typeName)
            {
                case "char":
                    return ProcessMultiDimensionalArray<char>(fieldBody, dnaField);
                case "uchar":
                    return ProcessMultiDimensionalArray<byte>(fieldBody, dnaField);
                case "short":
                    return ProcessMultiDimensionalArray<short>(fieldBody, dnaField);
                case "ushort":
                    return ProcessMultiDimensionalArray<ushort>(fieldBody, dnaField);
                case "int":
                    return ProcessMultiDimensionalArray<int>(fieldBody, dnaField);
                case "uint":
                    return ProcessMultiDimensionalArray<uint>(fieldBody, dnaField);
                case "float":
                    return ProcessMultiDimensionalArray<float>(fieldBody, dnaField);
                case "double":
                    return ProcessMultiDimensionalArray<double>(fieldBody, dnaField);
                case "long":
                    return ProcessMultiDimensionalArray<long>(fieldBody, dnaField);
                case "ulong":
                    return ProcessMultiDimensionalArray<ulong>(fieldBody, dnaField);
                case "int64_t":
                    return ProcessMultiDimensionalArray<long>(fieldBody, dnaField);
                case "uint64_t":
                    return ProcessMultiDimensionalArray<ulong>(fieldBody, dnaField);
                default:
                    throw new SystemException($"Unknown Primitive Type: {typeName}");
            }
        }

        private IField ProcessMultiDimensionalArray<T>(byte[] fieldBody, DNAField dnaField)
        {
            var values = GetArrayValues<T>(fieldBody, dnaField);

            if (dnaField.ArrayDepth == 1) return ProccessArrayValues(dnaField, values);

            for (int i = dnaField.ArrayDepth - 2; i > -1; i--)
            {
                f.print("i: " + i + " array length: " + dnaField.ArrayLengths[i]);
                int arraysAtCurrentDepth = 1;
                for (int j = 0; j <= i; j++)
                {
                    arraysAtCurrentDepth *= dnaField.ArrayLengths[j];
                }
                int arraySize = dnaField.ArrayLengths[i];

                var bufferArray = new List<Field>(arraysAtCurrentDepth);

                for (int j = 0; j < arraysAtCurrentDepth; j++)
                {
                    if (i == dnaField.ArrayDepth - 2) // If we're at the first array we want to split up the values.
                    {
                        f.print("Spliting array up  at " + j + " of " + arraysAtCurrentDepth);
                        var partialValues = values.Skip(j * arraySize).Take(arraySize).ToArray();
                        var array = ProccessArrayValues(dnaField, partialValues);
                        bufferArray.Add(array);
                    }
                    // we split up the FieldArrays
                    else
                    {
                        throw new NotImplementedException("Arrays of 3 or more dimensions are not yet supported.");
                    }
                }

                var fieldArray = new FieldArrays(dnaField.FieldName, bufferArray);

                // if it's the final we return.
                if (i == 0) return fieldArray;
            }
            throw new SystemException("How did you get here?");
        }
        
        private Field ProccessArrayValues<T>(DNAField dnaField, T arrayValues)
        {
            string typeName = dnaField.TypeName;
            // If we're dealing with a small pointer size, we need change the long and ulong accordingly.
            if (typeName == "long" && dnaField.PointerSize == 4) typeName = "int";
            if (typeName == "ulong" && dnaField.PointerSize == 4) typeName = "uint";

            return PrimitiveArrayFunctionMap[typeName](dnaField.FieldName, arrayValues);
        }
    }

}