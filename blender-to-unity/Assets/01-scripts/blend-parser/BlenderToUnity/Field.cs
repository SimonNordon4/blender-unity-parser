using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace BlenderToUnity
{
    [System.Serializable]
    public class Field<T> : IField<T>
    {
        [field: SerializeField]
        public string FieldName { get; private set; }

        [field: SerializeField]
        public string Type { get; private set; }

        [field:SerializeField]
        public FieldContext FieldContext {get; private set;}

        [field:SerializeField]
        public T Value { get; private set; }

        public Field(T value, DNAField dnaField)
        {
            this.FieldName = dnaField.FieldName;
            this.Type = dnaField.Type;
            this.FieldContext = dnaField.Context;
            this.Value = value;
        }
    }
}