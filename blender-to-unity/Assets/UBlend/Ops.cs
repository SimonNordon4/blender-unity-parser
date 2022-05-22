using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace UBlend{
    public class Ops 
    {
        public static JObject GetJObject(string json)
        {
            return JObject.Parse(json);
        }

        public static Data GetUBlend(JObject json)
        {
            Data data = new Data();

            data.assets = json["assets"].ToObject<Assets>();
            data.objects = json["objects"].ToObject<Objects>();
            data.settings = json["settings"].ToObject<Settings>();

            return data;
        }
    }

}