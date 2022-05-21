using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using UnityEngine;


namespace BlenderToUnity
{
    public class UBlendDataConverter : CustomCreationConverter<UComponent>
    {
        public override UComponent Create(Type objectType)
        {
            if(objectType == typeof(UComponent))
            {
                return new UComponent();
            }
            else
            {
                throw new Exception("Unexpected type");
            }
        }
    }
}

public class TestConverter:CustomCreationConverter<Component>
{
    // public override bool CanConvert(Type objectType)
    // {
    //     return typeof(Component).IsAssignableFrom(objectType);
    // }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        object value = Activator.CreateInstance(objectType);
        serializer.Populate(reader, value);

        Component p  = value as Component;
        // if (p.Children != null)
        // {
        //     foreach (Teleport child in p.Children)
        //     {
        //         child.Parent = p;
        //     }
        // }
        Debug.Log(p.type);
        return value;
    }
    public override Component Create(Type objectType)
    {
        var serializer = new JsonSerializer();

        if(objectType == typeof(Component))
        {
            return new Component();
        }
        else if(objectType == typeof(Teleport))
        {
             return new Teleport();
        }
        else if (objectType == typeof(Collider)){
            return new Collider();
        }
        else
        {
            throw new Exception("Unexpected type");
        }
    }
}