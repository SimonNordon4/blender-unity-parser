// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;

// namespace BlenderToUnity
// {
//     [System.Serializable]
//     public class Structure : IField
//     {
//         [field: SerializeField]
//         public string TypeName {get; private set;}

//         /// <summary>
//         /// Structure is the parsed data of a FileBlock.
//         /// </summary>
//         /// <param name="partialBody">Section of the block body representing 1 structure.</param>
//         /// <param name="definition">Structure definition associated with the block body</param>
//         public Structure(byte[] partialBody, StructureDefinition definition)
//         {
//            TypeName = definition.StructureTypeName;
//            List<IField> fields = new List<IField>();
//            fields = ParseFields(partialBody, definition);
//         }

//         private List<IField> ParseFields(byte[] partialBody, StructureDefinition definition)
//         {
//             List<IField> fields = new List<IField>();

//             foreach (var fieldDefinition in definition.FieldDefinitions)
//             {
//                 // If it's an array, pointer or primitive we just keep going down.

//                 // Everything is eventually a primitive. 
//                 if(fieldDefinition.IsArray || fieldDefinition.IsPointer || fieldDefinition.IsPrimitive)
//                 {
//                     var field = new Field(fieldDefinition);
//                 }

//                 // if it's a struct we want to return that struct as a field, so that it can continue.
//                 else
//                 {
                    
//                     //var field = new Structure(partialBody,fieldDefinition);
//                 }
//             }

//             return fields;
//         }


//     }
// }