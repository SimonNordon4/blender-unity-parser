using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class TESTS : MonoBehaviour
{
    [Button]
    public void colorPrint()
    {
        UnityEngine.Debug.Log("<color=red>red</color>");
    }
}
