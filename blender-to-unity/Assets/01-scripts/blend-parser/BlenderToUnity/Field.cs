using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace BlenderToUnity
{
    [System.Serializable]
    public class Field : IField
    {
        [field: SerializeField]
        public string FieldName { get; set; }
        public Field(string fieldName)
        {
            FieldName = fieldName;
        }
    }

    [System.Serializable]
    public class Field<T> : IField<T>
    {

        /// <summary>
        /// Parsed Field Value.
        /// </summary>
        [field: SerializeField]
        public T Value { get; set; }

        public Field(T value)//, byte[] fieldBody, DNAField dnaField)
        {
            Value = value;
            // FieldBody = fieldBody;
            // DnaField = dnaField;
        }
    }

    public class FieldArray : IField
    {
        [field: SerializeField]
        public string FieldName { get; set; }

        [field: SerializeReference]
        public List<IField> Fields { get; set; }
        public FieldArray(string fieldName)
        {
            FieldName = fieldName;
            Fields = new List<IField>();
        }
    }

    #region Primitives

    [System.Serializable]
    public class FieldChar : Field
    {
        [field: SerializeField]
        public char Char { get; set; }

        public FieldChar(string fieldName, char value) : base(fieldName)
        {
            Char = value;
        }
    }
    [System.Serializable]
    public class FieldUChar : Field
    {
        [field: SerializeField]
        public byte UChar { get; set; }

        public FieldUChar(string fieldName, byte value) : base(fieldName)
        {
            UChar = value;
        }
    }
    [System.Serializable]
    public class FieldShort : Field
    {
        [field: SerializeField]
        public short Short { get; set; }

        public FieldShort(string fieldName, short value) : base(fieldName)
        {
            Short = value;
        }
    }
    [System.Serializable]
    public class FieldUShort : Field
    {
        [field: SerializeField]
        public ushort UShort { get; set; }

        public FieldUShort(string fieldName, ushort value) : base(fieldName)
        {
            UShort = value;
        }
    }
    [System.Serializable]
    public class FieldInt : Field
    {
        [field: SerializeField]
        public int Int { get; set; }

        public FieldInt(string fieldName, int value) : base(fieldName)
        {
            Int = value;
        }
    }
    [System.Serializable]
    public class FieldUInt : Field
    {
        [field: SerializeField]
        public uint UInt { get; set; }

        public FieldUInt(string fieldName, uint value) : base(fieldName)
        {
            UInt = value;
        }
    }
    [System.Serializable]
    public class FieldLong : Field
    {
        [field: SerializeField]
        public long Long { get; set; }

        public FieldLong(string fieldName, long value) : base(fieldName)
        {
            Long = value;
        }
    }
    [System.Serializable]
    public class FieldULong : Field
    {
        [field: SerializeField]
        public ulong ULong { get; set; }

        public FieldULong(string fieldName, ulong value) : base(fieldName)
        {
            ULong = value;
        }
    }
    [System.Serializable]
    public class FieldFloat : Field
    {
        [field: SerializeField]
        public float Float { get; set; }

        public FieldFloat(string fieldName, float value) : base(fieldName)
        {
            Float = value;
        }
    }
    [System.Serializable]
    public class FieldDouble : Field
    {
        [field: SerializeField]
        public double Double { get; set; }

        public FieldDouble(string fieldName, double value) : base(fieldName)
        {
            Double = value;
        }
    }

    #endregion

    #region Arrays

    [System.Serializable]
    public class FieldChars : Field
    {
        [field: SerializeField]
        public string String { get; set; }
        public char[] Chars { get; set; }

        public FieldChars(string fieldName, char[] value) : base(fieldName)
        {
            Chars = value;
            String = new string(value);
        }
    }

    [System.Serializable]
    public class FieldUChars : Field
    {
        [field: SerializeField]
        public byte[] UChars { get; set; }

        public FieldUChars(string fieldName, byte[] value) : base(fieldName)
        {
            UChars = value;
        }
    }
    [System.Serializable]
    public class FieldShorts : Field
    {
        [field: SerializeField]
        public short[] Shorts { get; set; }

        public FieldShorts(string fieldName, short[] value) : base(fieldName)
        {
            Shorts = value;
        }
    }
    [System.Serializable]
    public class FieldUShorts : Field
    {
        [field: SerializeField]
        public ushort[] UShorts { get; set; }

        public FieldUShorts(string fieldName, ushort[] value) : base(fieldName)
        {
            UShorts = value;
        }
    }
    [System.Serializable]
    public class FieldInts : Field
    {
        [field: SerializeField]
        public int[] Ints { get; set; }

        public FieldInts(string fieldName, int[] value) : base(fieldName)
        {
            Ints = value;
        }
    }
    [System.Serializable]
    public class FieldUInts : Field
    {
        [field: SerializeField]
        public uint[] UInts { get; set; }

        public FieldUInts(string fieldName, uint[] value) : base(fieldName)
        {
            UInts = value;
        }
    }
    [System.Serializable]
    public class FieldLongs : Field
    {
        [field: SerializeField]
        public long[] Longs { get; set; }

        public FieldLongs(string fieldName, long[] value) : base(fieldName)
        {
            Longs = value;
        }
    }
    [System.Serializable]
    public class FieldULongs : Field
    {
        [field: SerializeField]
        public ulong[] ULongs { get; set; }

        public FieldULongs(string fieldName, ulong[] value) : base(fieldName)
        {
            ULongs = value;
        }
    }
    [System.Serializable]
    public class FieldFloats : Field
    {
        [field: SerializeField]
        public float[] Floats { get; set; }

        public FieldFloats(string fieldName, float[] value) : base(fieldName)
        {
            Floats = value;
        }
    }
    [System.Serializable]
    public class FieldDoubles : Field
    {
        [field: SerializeField]
        public double[] Doubles { get; set; }

        public FieldDoubles(string fieldName, double[] value) : base(fieldName)
        {
            Doubles = value;
        }
    }

    #endregion

    #region 2D Arrays

    [System.Serializable]
    public class FieldChars2D : Field
    {
        [field: SerializeField]
        public char[][] Chars2D { get; set; }

        public FieldChars2D(string fieldName, char[][] value) : base(fieldName)
        {
            Chars2D = value;
        }
    }

    [System.Serializable]
    public class FieldUChars2D : Field
    {
        [field: SerializeField]
        public byte[][] UChars2D { get; set; }

        public FieldUChars2D(string fieldName, byte[][] value) : base(fieldName)
        {
            UChars2D = value;
        }
    }
    [System.Serializable]
    public class FieldShorts2D : Field
    {
        [field: SerializeField]
        public short[][] Shorts2D { get; set; }

        public FieldShorts2D(string fieldName, short[][] value) : base(fieldName)
        {
            Shorts2D = value;
        }
    }
    [System.Serializable]
    public class FieldUShorts2D : Field
    {
        [field: SerializeField]
        public ushort[][] UShorts2D { get; set; }

        public FieldUShorts2D(string fieldName, ushort[][] value) : base(fieldName)
        {
            UShorts2D = value;
        }
    }
    [System.Serializable]
    public class FieldInts2D : Field
    {
        [field: SerializeField]
        public int[][] Ints2D { get; set; }

        public FieldInts2D(string fieldName, int[][] value) : base(fieldName)
        {
            Ints2D = value;
        }
    }
    [System.Serializable]
    public class FieldUInts2D : Field
    {
        [field: SerializeField]
        public uint[][] UInts2D { get; set; }

        public FieldUInts2D(string fieldName, uint[][] value) : base(fieldName)
        {
            UInts2D = value;
        }
    }
    [System.Serializable]
    public class FieldLongs2D : Field
    {
        [field: SerializeField]
        public long[][] Longs2D { get; set; }

        public FieldLongs2D(string fieldName, long[][] value) : base(fieldName)
        {
            Longs2D = value;
        }
    }
    [System.Serializable]
    public class FieldULongs2D : Field
    {
        [field: SerializeField]
        public ulong[][] ULongs2D { get; set; }

        public FieldULongs2D(string fieldName, ulong[][] value) : base(fieldName)
        {
            ULongs2D = value;
        }
    }
    [System.Serializable]
    public class FieldFloats2D : Field
    {
        [field: SerializeField]
        public float[][] Floats2D { get; set; }

        public FieldFloats2D(string fieldName, float[][] value) : base(fieldName)
        {
            Floats2D = value;
        }
    }
    [System.Serializable]
    public class FieldDoubles2D : Field
    {
        [field: SerializeField]
        public double[][] Doubles2D { get; set; }

        public FieldDoubles2D(string fieldName, double[][] value) : base(fieldName)
        {
            Doubles2D = value;
        }
    }

    #endregion

    #region 3D Arrays

    [System.Serializable]
    public class FieldChars3D : Field
    {
        [field: SerializeField]
        public char[][][] Chars3D { get; set; }

        public FieldChars3D(string fieldName, char[][][] value) : base(fieldName)
        {
            Chars3D = value;
        }
    }

    [System.Serializable]
    public class FieldUChars3D : Field
    {
        [field: SerializeField]
        public byte[][][] UChars3D { get; set; }

        public FieldUChars3D(string fieldName, byte[][][] value) : base(fieldName)
        {
            UChars3D = value;
        }
    }
    [System.Serializable]
    public class FieldShorts3D : Field
    {
        [field: SerializeField]
        public short[][][] Shorts3D { get; set; }

        public FieldShorts3D(string fieldName, short[][][] value) : base(fieldName)
        {
            Shorts3D = value;
        }
    }
    [System.Serializable]
    public class FieldUShorts3D : Field
    {
        [field: SerializeField]
        public ushort[][][] UShorts3D { get; set; }

        public FieldUShorts3D(string fieldName, ushort[][][] value) : base(fieldName)
        {
            UShorts3D = value;
        }
    }
    [System.Serializable]
    public class FieldInts3D : Field
    {
        [field: SerializeField]
        public int[][][] Ints3D { get; set; }

        public FieldInts3D(string fieldName, int[][][] value) : base(fieldName)
        {
            Ints3D = value;
        }
    }
    [System.Serializable]
    public class FieldUInts3D : Field
    {
        [field: SerializeField]
        public uint[][][] UInts3D { get; set; }

        public FieldUInts3D(string fieldName, uint[][][] value) : base(fieldName)
        {
            UInts3D = value;
        }
    }
    [System.Serializable]
    public class FieldLongs3D : Field
    {
        [field: SerializeField]
        public long[][][] Longs3D { get; set; }

        public FieldLongs3D(string fieldName, long[][][] value) : base(fieldName)
        {
            Longs3D = value;
        }
    }
    [System.Serializable]
    public class FieldULongs3D : Field
    {
        [field: SerializeField]
        public ulong[][][] ULongs3D { get; set; }

        public FieldULongs3D(string fieldName, ulong[][][] value) : base(fieldName)
        {
            ULongs3D = value;
        }
    }
    [System.Serializable]
    public class FieldFloats3D : Field
    {
        [field: SerializeField]
        public float[][][] Floats3D { get; set; }

        public FieldFloats3D(string fieldName, float[][][] value) : base(fieldName)
        {
            Floats3D = value;
        }
    }
    [System.Serializable]
    public class FieldDoubles3D : Field
    {
        [field: SerializeField]
        public double[][][] Doubles3D { get; set; }

        public FieldDoubles3D(string fieldName, double[][][] value) : base(fieldName)
        {
            Doubles3D = value;
        }
    }

    #endregion

}