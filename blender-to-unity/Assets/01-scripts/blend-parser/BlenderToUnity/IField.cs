using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BlenderToUnity
{
    /// <summary>
    /// Represents a single weakly-typed field in a structure. 
    /// </summary>
    public interface IField
    {
        //string Type {get;}
    }

    /// <summary>
    /// Represents a single strongly-typed field in Blender. Use IStructField if the field contains a struct.
    /// </summary>
    /// <typeparam name="T">T should be specified as a primitive; where the type of <pre>string</pre> is a special case
    /// denoting a Blender-defined struct; in which case the value should be the SDNA type name.</typeparam>
    public interface IField<T> : IField
    {
         
    }

     /// <summary>
    /// Represents a field that contains a struct. The struct is represented as a list of <pre>IField</pre>s.
    /// </summary>
    public interface IStructField : IField<string>
    {

    }
}
