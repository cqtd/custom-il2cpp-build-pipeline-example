using System.IO;
using UnityEditor;
using UnityEngine;

namespace CustomBuildPipeline
{
	internal class Menu
	{
		private const string MENU_NAME = "Build/";
		
		#region Open Folder

		private const string OPEN_FOLDER = "Open Folder/";

		[MenuItem(MENU_NAME + OPEN_FOLDER +"il2cpp", false, 1)]
		private static void OpenIl2cppPath()
		{
			WindowUtility.OpenFolder($"{EditorApplication.applicationContentsPath}\\il2cpp");
		}
		
		[MenuItem(MENU_NAME + OPEN_FOLDER + "il2cpp\\libil2cpp", false, 2)]
		private static void OpenIl2cpplibPath()
		{
			WindowUtility.OpenFolder($"{EditorApplication.applicationContentsPath}\\il2cpp\\libil2cpp");
		}
		
		[MenuItem(MENU_NAME + OPEN_FOLDER + "il2cpp\\libil2cpp\\vm", false, 3)]
		private static void OpenIl2cppVMPath()
		{
			WindowUtility.OpenFolder($"{EditorApplication.applicationContentsPath}\\il2cpp\\libil2cpp\\vm");
		}
		
		[MenuItem(MENU_NAME + OPEN_FOLDER + "Latest Build", false, 20)]
		private static void OpenLatestBuild()
		{
			if (BuildHistory.GetLatestBuildPath(EditorUserBuildSettings.activeBuildTarget, out BuildHistory.Data data))
			{
				WindowUtility.OpenFolder(data.pathToBuiltProject);
			}
			else
			{
				Debug.LogError("Cannot find the latest build folder");
			}
		}
		
		[MenuItem(MENU_NAME + OPEN_FOLDER + "Latest Solution", false, 21)]
		private static void OpenLatestSolution()
		{
			if (BuildHistory.GetLatestSolutionPath(EditorUserBuildSettings.activeBuildTarget, out BuildHistory.Data data))
			{
				WindowUtility.OpenFolder(data.pathToBuiltProject);
			}
			else
			{
				Debug.LogError("Cannot find the latest build folder");
			}
		}
		
		#endregion

		#region Build Config
		
		[MenuItem(MENU_NAME + "Activated", false, 0)]
		private static void ToggleIl2cppCustomize()
		{	
			Il2cppCustom.IsActive = !Il2cppCustom.IsActive;
		}
		
		[MenuItem(MENU_NAME + "Activated", true, 0)]
		private static bool ValidateIl2cppCustomize()
		{
			UnityEditor.Menu.SetChecked(MENU_NAME + "Activated", Il2cppCustom.IsActive);
			return true;
		}

		#endregion

		#region Tools

		private const string TOOL = "Tools/";

		[MenuItem(MENU_NAME + TOOL + "Convert Metadata", false, 100)]
		private static void ConvertMetadata()
		{
			string metaInput = EditorUtility.OpenFilePanel("global-metadata.dat", Application.dataPath, "dat");
			
			if (string.IsNullOrEmpty(metaInput)) return;
			
			string savePath = EditorUtility.SaveFilePanel("encrypted.dat", new FileInfo(metaInput).DirectoryName,
				"StaticMetadata.h", "h");

			HeaderFile header = new HeaderFile(metaInput);

			File.WriteAllText(savePath, header.ToString());
		}

		#endregion
	}
}

