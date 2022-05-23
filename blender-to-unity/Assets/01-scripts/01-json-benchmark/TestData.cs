using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JsonBenchmark
{

    [Serializable]
    public class TestData
    {
        public string name = "Test";
        public int version = 1;
        public TestExplicit testExplicit = new TestExplicit();

        public TestGeneric testGeneric = new TestGeneric("hi");
        
        public List<TestGeneric> testGenerics = new List<TestGeneric>{new TestGeneric("Test1"), new TestGeneric("Test2"), new TestGeneric("Test3")};

        [SerializeReference]
        public List<Parent> parents = new List<Parent>{new Child(), new Child(), new Child()};
    }

    [Serializable]
    public class TestExplicit
    {
        public float[] easyFloats = new float[] { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f, 10.0f };
        public float[][] hardFloats = new float[][]{
            new float[]{1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f, 10.0f},
            new float[]{1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f, 10.0f},
            new float[]{1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f, 10.0f}
        };

        public Vector3 position = Vector3.zero;
        public Vector3[] positions = new Vector3[0];
    }

    [Serializable]
    public struct TestGeneric
    {
        public string type;

        public TestGeneric(string type)
        {
            this.type = type;
        }
    }

    [Serializable]
    public class Parent{
        //public string name = "Parent";
    }

    [Serializable]
    public class Child : Parent{
        public string name = "Child";
        public string siblings = "Many";

    }
}