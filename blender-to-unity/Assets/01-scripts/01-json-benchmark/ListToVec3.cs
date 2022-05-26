using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.Diagnostics;

[ExecuteInEditMode]
public class ListToVec3 : MonoBehaviour
{
    public const int length = 1000000;
    private float[] list = new float[length*3];
    private Vector3[] vec3 = new Vector3[length];

    [Button]
    private void PopulateList()
    {
        list = new float[length*3];
        vec3 = new Vector3[length];
        var watch = new Stopwatch();
        for (int i = 0; i < length*3; i++)
        {
            list[i] = Random.Range(0.0f, 1.0f);
        }  watch.Stop();
        UnityEngine.Debug.Log("PopList: " + watch.ElapsedMilliseconds);
    }

    [Button]
    private void ListToVecs()
    {
        UnityEngine.Debug.Log($"List Lengt {list.Length}");
        var watch = new Stopwatch();
        for (int i = 0; i < length -1; i++)
        {
           vec3[i].Set(list[i*3], list[i*3+1], list[i*3+2]);
        }
        watch.Stop();
        UnityEngine.Debug.Log("ListToVecs: " + watch.ElapsedMilliseconds);
        UnityEngine.Debug.Log(vec3[Random.Range(0, length - 1)]);
    }
}
