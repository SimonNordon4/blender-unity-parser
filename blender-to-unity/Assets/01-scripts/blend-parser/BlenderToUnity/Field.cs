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
        // /// <summary>
        // /// DNAField this field was created from.
        // /// </summary>
        // [field: SerializeField]
        // public DNAField DnaField { get; set; }

        // /// <summary>
        // /// Raw Field Data.
        // /// </summary>
        // [field: SerializeField]
        // public byte[] FieldBody { get; set; }

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
}