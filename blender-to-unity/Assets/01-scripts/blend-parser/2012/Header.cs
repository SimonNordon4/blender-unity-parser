using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class Header
{
    public Header(int pointerSize, char endianness, string versionNumber)
    {
        PointerSize = pointerSize;
        Endian = endianness == 'v' ? "little" : "big";
        VersionNumber = versionNumber;
    }
    public int PointerSize {get; private set;}

    public string Endian {get; private set;}

    public string VersionNumber {get; private set;}

}