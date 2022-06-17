using UnityEngine;
using System;
using System.Collections.Generic;

namespace BlenderToUnity
{
    [System.Serializable]
    public class BlendFileViewer
    {
        [field: SerializeField]
        public string SourceFilePath { get; private set; }

        [field: SerializeField]
        public HeaderViewer Header { get; private set; }

        [field: SerializeField]
        public FileBlockViewer[] FileBlocks { get; private set; }

        [field:SerializeField]
        public DNA1BlockViewer DNA1Block { get; private set; }

        public BlendFileViewer(BlenderFile blendFile)
        {
            SourceFilePath = blendFile.SourceFilePath;
            Header = new HeaderViewer(blendFile.Header);

            FileBlocks = new FileBlockViewer[blendFile.FileBlocks.Count];

            for (int i = 0; i < blendFile.FileBlocks.Count; i++)
            {
                FileBlocks[i] = new FileBlockViewer(blendFile.FileBlocks[i]);
            }

            DNA1Block = new DNA1BlockViewer(blendFile.DNA1Block);
        }
    }

    [System.Serializable]
    public class HeaderViewer
    {
        public HeaderViewer(Header header)
        {
            this.PointerSize = header.PointerSize;
            this.VersionNumber = header.VersionNumber;
            this.Endian = header.Endian;
        }

        [field: SerializeField]
        public int PointerSize { get; private set; }

        [field: SerializeField]
        public string VersionNumber { get; private set; }

        [field: SerializeField]
        public string Endian { get; private set; }
    }

    [System.Serializable]
    public class FileBlockViewer
    {
        [field: SerializeField]
        public long BlockStartPosition { get; private set; }
        [field: SerializeField]
        public string Code { get; private set; }
        [field: SerializeField]
        public int LenBody { get; private set; }
        [field: SerializeField]
        public long OldMemoryAddress { get; private set; }
        [field: SerializeField]
        public int SDNAIndex { get; private set; }
        [field: SerializeField]
        public int Count { get; private set; }
        public FileBlockViewer(FileBlock fileBlock)
        {
            this.BlockStartPosition = fileBlock.BlockStartPosition;
            this.Code = fileBlock.Code;
            this.LenBody = fileBlock.LenBody;
            this.OldMemoryAddress = fileBlock.OldMemoryAddress;
            this.SDNAIndex = fileBlock.SDNAIndex;
            this.Count = fileBlock.Count;
        }
    }

    [System.Serializable]
    public class DNA1BlockViewer
    {
        
        [field: SerializeField]
        public List<string> Names { get; private set; }
        
        [field: SerializeField]
        public List<string> NameTypes { get; private set; }
        
        [field: SerializeField]
        public List<short> TypeSizes { get; private set; }

        [field: SerializeField]
        public List<StructureTypeViewer> StructureTypes { get; private set; }


        public DNA1BlockViewer(DNA1Block dna1Block)
        {
            this.Names = dna1Block.Names;
            this.NameTypes = dna1Block.NameTypes;
            this.TypeSizes = dna1Block.TypeSizes;
            
            this.StructureTypes  = new List<StructureTypeViewer>();

            foreach (var structureType in dna1Block.StructureTypes)
            {   
                this.StructureTypes.Add(new StructureTypeViewer(structureType));
            }
        }
    }


    [System.Serializable]
    public struct StructureTypeViewer
    {
        public short StructureTypeIndex;
        public List<StructureTypeFieldViewer> StructureTypeFields;
        public StructureTypeViewer(StructureType structureType)
        {
            this.StructureTypeIndex = structureType.StructureTypeIndex;
            this.StructureTypeFields = new List<StructureTypeFieldViewer>();
            foreach (var structureTypeField in structureType.StructureTypeFields)
            {
                this.StructureTypeFields.Add(new StructureTypeFieldViewer(structureTypeField));
            }
        }
    }

    [System.Serializable]
    public struct StructureTypeFieldViewer
    {
        public short TypeOfField;
        public short Name;

        public StructureTypeFieldViewer(StructureTypeField structureTypeField)
        {
            this.TypeOfField = structureTypeField.TypeOfField;
            this.Name = structureTypeField.NameOfField;
        }
    }
}