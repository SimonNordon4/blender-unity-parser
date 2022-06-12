using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class BlendHeader
{
    public BlendHeader(int pointerSize, char endianness, string versionNumber)
    {
        PointerSize = pointerSize;
        Endian = endianness == 'v' ? "little" : "big";
        VersionNumber = versionNumber;
    }
    public int PointerSize;
    public string Endian;
    public string VersionNumber;
}