using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CustomBuildPipeline
{
	internal class BuildHistory
	{
		private const string LATEST_BUILD_KEY = "{0}.LastBuild.{1}";
		private const string LATEST_BUILD_SOLUTION_KEY = "{0}.LastSolution.{1}";

		private static string description = default;

		private static string GetBuildKey(BuildTarget target)
		{
			return string.Format(LATEST_BUILD_KEY, PlayerSettings.productName, target);
		}
        
		private static string GetSolutionKey(BuildTarget target)
		{
			return string.Format(LATEST_BUILD_SOLUTION_KEY, PlayerSettings.productName, target);
		}

		internal static bool GetLatestBuildPath(BuildTarget target, out Data history)
		{
			string key = GetBuildKey(target);
			if (EditorPrefs.HasKey(key))
			{
				string json = EditorPrefs.GetString(key);
				history = JsonUtility.FromJson<Data>(json);
                
				return true;
			}

			history = null;
			return false;
		}
        
		internal static bool GetLatestSolutionPath(BuildTarget target, out Data history)
		{
			string key = GetSolutionKey(target);
			if (EditorPrefs.HasKey(key))
			{
				string json = EditorPrefs.GetString(key);
				history = JsonUtility.FromJson<Data>(json);
                
				return true;
			}

			history = null;
			return false;
		}
        
		[PostProcessBuild(PostprocessBuildEvent.Order.History)]
		public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
		{
			string key;

#if UNITY_EDITOR_WIN
			if (UnityEditor.WindowsStandalone.UserBuildSettings.createSolution)
			{
				key = GetSolutionKey(target);
			}
			else
			{
				key = GetBuildKey(target);
			}
#else
            key = GetBuildKey(target);
#endif

			string serialized = JsonUtility.ToJson(new Data()
			{
				pathToBuiltProject = pathToBuiltProject,
				version = PlayerSettings.bundleVersion,
				description = description
			});
            
			EditorPrefs.SetString(key, serialized);
			Debug.Log("Latest Build History Stored Successfully.");
		}

		[Serializable]
		public class Data
		{
			public string pathToBuiltProject;
			public string version;
			public string description;
		}
	}
}