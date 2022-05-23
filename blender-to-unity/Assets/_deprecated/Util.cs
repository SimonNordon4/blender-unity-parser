using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace BlenderToUnity
{
    public class Util
    {
        public static void ValidateImportData(UBlendData data)
        {
            var json = JsonConvert.SerializeObject(data);
            print(json);
        }
        public static void print(object obj)
        {
            Debug.Log(obj);
        }
    }
}