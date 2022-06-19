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

        [field: SerializeField]
        public string Type { get; set; }

        [field:SerializeField]
        public FieldContext FieldContext {get; set;}

        [field: SerializeField]
        public DNAField DnaField { get; set; }
    }
}