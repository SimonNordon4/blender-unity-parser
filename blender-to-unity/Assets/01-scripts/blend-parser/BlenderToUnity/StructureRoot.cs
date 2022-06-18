using System;
using UnityEngine;


namespace BlenderToUnity
{
    /// <summary>
    /// Top level Structure Container.
    /// </summary>
    [System.Serializable]
    public class StructureRoot
    {
        [field: SerializeField]
        public Structure[] Structures { get; private set; }

        public StructureRoot(FileBlock block, BlenderFile file)
        {
            if(block.Count > 0)
                this.Structures = ParseStructures(block, file);
        }

        private Structure[] ParseStructures(FileBlock block, BlenderFile file)
        {
            var structures = new Structure[block.Count];
            var definition = file.StructureDNA.StructureDefinitions[block.SDNAIndex];

            if (block.Count > 1)
            {
                for (int i = 0; i < block.Count; i++)
                {
                    int partialLenBody = block.LenBody / block.Count;
                    byte[] partialBody = new byte[partialLenBody];
                    for (int j = 0; j < partialLenBody; j++)
                        partialBody[j] = block.Body[i * partialLenBody + j];
                    structures[i] = new Structure(partialBody, definition);
                }

                return structures;
            }

            structures[0] = new Structure(block.Body, definition);
            return structures;
        }
    }
}