using System;
using UnityEngine;


namespace BlenderToUnity
{
    [System.Serializable]
    public class Field : IField
    {
        [field: SerializeField]
        public string TypeName { get; private set; }

        public Field(FieldDefinition fieldDefinition)
        {
            TypeName = fieldDefinition.Type.Name;
        }
    }
}