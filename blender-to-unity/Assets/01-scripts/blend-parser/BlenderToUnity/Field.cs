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

    /// <summary>
    /// Array container for a field that is itself a field.
    /// </summary>
    public class FieldArray : IField<List<IField>>
    {
        [field:SerializeField]
        public string FieldName { get; set; }
         [field:SerializeReference]
        public List<IField> Value { get; set; }
        public FieldArray(string fieldName,List<IField> value)
        {
            FieldName = fieldName;
            Value = value;
        }
    }

    [System.Serializable]
    public class FieldChar : Field
    {
        [field:SerializeField]
        public char Char { get; set; }

        public FieldChar(string fieldName, char value) : base(fieldName)
        {
            Char = value;
        }
    }
    [System.Serializable]
    public class FieldUChar: Field
    {
        [field:SerializeField]
        public byte UChar { get; set; }

        public FieldUChar(string fieldName, byte value) : base(fieldName)
        {
            UChar = value;
        }
    }
    [System.Serializable]
    public class FieldShort : Field
    {
        [field:SerializeField]
        public short Short { get; set; }

        public FieldShort(string fieldName, short value) : base(fieldName)
        {
            Short = value;
        }
    }
    [System.Serializable]
    public class FieldUShort : Field
    {
        [field:SerializeField]
        public ushort UShort { get; set; }

        public FieldUShort(string fieldName, ushort value) : base(fieldName)
        {
            UShort = value;
        }
    }
    [System.Serializable]
    public class FieldInt : Field
    {
        [field:SerializeField]
        public int Int { get; set; }

        public FieldInt(string fieldName, int value) : base(fieldName)
        {
            Int = value;
        }
    }
    [System.Serializable]
    public class FieldUInt : Field
    {
        [field:SerializeField]
        public uint UInt { get; set; }

        public FieldUInt(string fieldName, uint value) : base(fieldName)
        {
            UInt = value;
        }
    }
    [System.Serializable]
    public class FieldLong : Field
    {
        [field:SerializeField]
        public long Long { get; set; }

        public FieldLong(string fieldName, long value) : base(fieldName)
        {
            Long = value;
        }
    }
    [System.Serializable]
    public class FieldULong : Field
    {
        [field:SerializeField]
        public ulong ULong { get; set; }

        public FieldULong(string fieldName, ulong value) : base(fieldName)
        {
            ULong = value;
        }
    }
    [System.Serializable]
    public class FieldFloat : Field
    {
        [field:SerializeField]
        public float Float { get; set; }

        public FieldFloat(string fieldName, float value) : base(fieldName)
        {
            Float = value;
        }
    }
    [System.Serializable]
    public class FieldDouble : Field
    {
        [field:SerializeField]
        public double Double { get; set; }

        public FieldDouble(string fieldName, double value) : base(fieldName)
        {
            Double = value;
        }
    }

    [System.Serializable]
    public class FieldChars : Field
    {
        [field:SerializeField]
        public char[] Chars { get; set; }

        public FieldChars(string fieldName, char[] value) : base(fieldName)
        {
            Chars = value;
        }
    }
}