using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Diagnostics;
using System.Reflection;

namespace BlenderToUnity
{
    public class f
    {
        private static Dictionary<string,Stopwatch> stopwatches = new Dictionary<string, Stopwatch>();

        public static void print(object obj)
        {
            UnityEngine.Debug.Log(obj);
        }

        public static void printError(object obj)
        {
            UnityEngine.Debug.LogError(obj);
        }

        public static void startwatch(string name)
        {
            var watch = new Stopwatch();
            watch.Start();
            f.stopwatches[name] = watch;
        }

        public static void stopwatch(string name)
        {
            if(!stopwatches.ContainsKey(name))
            {
                printError("Stopwatch " + name + " was never started.");
                return;
            }

            var watch = f.stopwatches[name];
            watch.Stop();
            UnityEngine.Debug.Log(name + ": " + watch.ElapsedMilliseconds + "ms");
            stopwatches.Remove(name);
        }
    }
}