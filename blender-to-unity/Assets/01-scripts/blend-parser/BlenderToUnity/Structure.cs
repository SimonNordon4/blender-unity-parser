using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlenderToUnity
{
    [System.Serializable]
    public class Structure
    {
        public string Name;

        public Structure[] Structures;

        public static Structure[] ParseFileBlock(BlenderFile file, int blockIndex)
        {
            var block = file.FileBlocks[blockIndex];
            var structDefinition = file.StructureDNA.StructureDefinitions[block.SDNAIndex];

            var structures = new Structure[block.Count];

            // if there are multiple block
            if(block.Count > 1)
            {
                for (int i = 0; i < block.Count; i++)
                {
                    int structLen = block.LenBody / block.Count;
                    byte[] body = new byte[structLen];

                    // Get the body which is a partial chunk of the original block body.
                    for (int j = 0; j < structLen; j++)
                    {
                        body[j] = block.Body[i * structLen + j];
                    }

                    var structure = new Structure(body, structDefinition);
                    structures[i] = structure;
                }

                return structures;
            }

            structures[0] = new Structure(block.Body, structDefinition);
            return structures;

        }

        public Structure(FileBlock block, BlenderFile file)
        {
            var structureDefinition = file.StructureDNA.StructureDefinitions[block.SDNAIndex];
            var body = block.Body;
            
        }

        public Structure(byte[] blockBody, StructureDefinition defnition)
        {
            this.Name = defnition.StructureTypeName;
        }
    }
}