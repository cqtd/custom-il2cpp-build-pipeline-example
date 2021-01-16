using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CustomBuildPipeline
{
    internal class PostprocessBuildEvent
    {
        internal static class Order
        {
            internal const int Event = 0;
            internal const int History = 3;
            internal const int IL2CPP = 5;

            internal const int Version = 10;
        }
        
        [PostProcessBuild(0)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            
        }
    }
}

