using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BlenderToUnity;
using System.Reflection;
using System;
using Newtonsoft.Json;

public class Tester : MonoBehaviour
{
    [Button]
    public void Test()
    {
        Type _type = typeof(UBlendData);
        //print(_type.GetProperties());
        FieldInfo[] _properties = _type.GetFields();
        foreach (FieldInfo _property in _properties)
        {
            //print(_property.Name);
            //print(_property.Attributes);
            foreach (var _attribute in _property.GetCustomAttributes(true))
            {
                if (_attribute.GetType() == typeof(JsonPropertyAttribute))
                {
                    //print(((JsonPropertyAttribute)_attribute).PropertyName);
                }
            }
        }

    }

    //TODO: You can do some crazy reflection stuff here I think.
    public class UBlendDataProperties
    {
        public static string uMeshes = ((JsonPropertyAttribute)typeof(UBlendData).GetField("uMeshes").GetCustomAttribute(typeof(JsonPropertyAttribute), true)).PropertyName;
    }

}





