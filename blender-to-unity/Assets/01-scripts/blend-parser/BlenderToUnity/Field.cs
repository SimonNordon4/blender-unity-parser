using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace BlenderToUnity
{
    [System.Serializable]
    public class Field: IField
    {
        [field:SerializeField]
        public string FieldName { get; set; }
        public Field (string fieldName)
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
        public T Value {get;set;}

        public Field(T value)//, byte[] fieldBody, DNAField dnaField)
        {
            Value = value;
            // FieldBody = fieldBody;
            // DnaField = dnaField;
        }
    }
    [System.Serializable]
    public class CharField : Field
    {
        [field:SerializeField]
        public char Char { get; set; }

        public CharField(string fieldName, char value) : base(fieldName)
        {
            Char = value;
        }
    }
    [System.Serializable]
    public class UCharField: Field
    {
        [field:SerializeField]
        public byte UChar { get; set; }

        public UCharField(string fieldName, byte value) : base(fieldName)
        {
            UChar = value;
        }
    }
    [System.Serializable]
    public class ShortField : Field
    {
        [field:SerializeField]
        public short Short { get; set; }

        public ShortField(string fieldName, short value) : base(fieldName)
        {
            Short = value;
        }
    }
    [System.Serializable]
    public class UShortField : Field
    {
        [field:SerializeField]
        public ushort UShort { get; set; }

        public UShortField(string fieldName, ushort value) : base(fieldName)
        {
            UShort = value;
        }
    }
    [System.Serializable]
    public class IntField : Field
    {
        [field:SerializeField]
        public int Int { get; set; }

        public IntField(string fieldName, int value) : base(fieldName)
        {
            Int = value;
        }
    }
    [System.Serializable]
    public class UIntField : Field
    {
        [field:SerializeField]
        public uint UInt { get; set; }

        public UIntField(string fieldName, uint value) : base(fieldName)
        {
            UInt = value;
        }
    }
    [System.Serializable]
    public class LongField : Field
    {
        [field:SerializeField]
        public long Long { get; set; }

        public LongField(string fieldName, long value) : base(fieldName)
        {
            Long = value;
        }
    }
    [System.Serializable]
    public class ULongField : Field
    {
        [field:SerializeField]
        public ulong ULong { get; set; }

        public ULongField(string fieldName, ulong value) : base(fieldName)
        {
            ULong = value;
        }
    }
    [System.Serializable]
    public class FloatField : Field
    {
        [field:SerializeField]
        public float Float { get; set; }

        public FloatField(string fieldName, float value) : base(fieldName)
        {
            Float = value;
        }
    }
    [System.Serializable]
    public class DoubleField : Field
    {
        [field:SerializeField]
        public double Double { get; set; }

        public DoubleField(string fieldName, double value) : base(fieldName)
        {
            Double = value;
        }
    }
}