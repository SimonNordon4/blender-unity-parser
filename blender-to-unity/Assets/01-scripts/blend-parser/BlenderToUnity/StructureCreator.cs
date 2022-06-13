using System;
using System.Collections.Generic;

namespace BlenderToUnity
{
    public class StructureCreator
    {
        public static bool CreateSDNAStructures(ref StructureDNA sdna)
        {

            int numberOfTypes = sdna.Types.Count;

            for (int i = 0; i < numberOfTypes; i++)
            {
                TypeDefinition typeDefinition = new TypeDefinition(sdna.Types[i], sdna.TypeSizes[i], sdna);
                sdna.TypeDefintions.Add(typeDefinition);
            }

            return true;
        }
    }
}