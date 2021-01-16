using System;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Profiling;

namespace CustomBuildPipeline
{
	internal class Il2cppCustom
	{
		private static string Key => $"{PlayerSettings.productName}.il2cpp.customized";

		internal static bool IsActive {
			get
			{
				return EditorPrefs.GetBool(Key);
			}
			set
			{
				EditorPrefs.SetBool(Key, value);
			}
		}

        [PostProcessBuild(PostprocessBuildEvent.Order.IL2CPP)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
	        if (!IsActive) return;
	        
#if UNITY_EDITOR_WIN

	        switch (target)
	        {
		        case BuildTarget.StandaloneWindows:
		        case BuildTarget.StandaloneWindows64:
			        OnWindows(pathToBuiltProject);
			        break;
		        
		        case BuildTarget.Android:
			        OnAndroid(pathToBuiltProject);
			        break;
		        
		        default:
			        break;
	        }
            

        }
#endif

		private static void OnWindows(string pathToBuiltProject)
		{
			// 솔루션 생성 방식 빌드일 때
			if (UnityEditor.WindowsStandalone.UserBuildSettings.createSolution)
			{
				using (new ProfilerBlock("CustomBuildPipeline.IL2CPPCustom.ConvertMetadata"))
				{
					string metadataPath = pathToBuiltProject +
					                      $"\\build\\bin\\{PlayerSettings.productName}_Data\\il2cpp_data\\Metadata\\global-metadata.dat";

					string outputHeaderPath =
						pathToBuiltProject + "\\Il2CppOutputProject\\IL2CPP\\libil2cpp\\vm\\StaticMetadata.h";

					HeaderFile header = new HeaderFile(metadataPath);

					File.WriteAllText(outputHeaderPath, header.ToString());
				}

				using (new ProfilerBlock("CustomBuildPipeline.IL2CPPCustom.CopyCustomScripts"))
					CopyCustomScripts_Windows(pathToBuiltProject);
			}
			else
			{
                
			}			
		}

		private static void OnAndroid(string pathToBuiltProject)
		{
			if (EditorUserBuildSettings.exportAsGoogleAndroidProject)
			{
		        
			}
			else
			{
				
			}
		}

        private static void CopyCustomScripts_Windows(string pathToBuiltProject)
        {

	        
            string loaderHeaderPath =
                pathToBuiltProject + "\\Il2CppOutputProject\\IL2CPP\\libil2cpp\\vm\\MetadataLoader.h";
            string loaderSourcePath =
                pathToBuiltProject + "\\Il2CppOutputProject\\IL2CPP\\libil2cpp\\vm\\MetadataLoader.cpp";

            string customizedHeaderPath = $"{Application.dataPath}\\Build Pipeline\\Native\\MetadataLoader.h";
            string customizedSourcePath = $"{Application.dataPath}\\Build Pipeline\\Native\\MetadataLoader.cpp";

            FileInfo loaderHeader = new FileInfo(loaderHeaderPath);
            if (loaderHeader.Exists)
            {
                loaderHeader.Delete();
            }

            FileInfo customizedHeader = new FileInfo(customizedHeaderPath);
            if (customizedHeader.Exists)
            {
                customizedHeader.CopyTo(loaderHeaderPath);
            }
                
            FileInfo loaderSource = new FileInfo(loaderSourcePath);
            if (loaderHeader.Exists)
            {
                loaderHeader.Delete();
            }

            FileInfo customizedSource = new FileInfo(customizedSourcePath);
            if (customizedHeader.Exists)
            {
                customizedHeader.CopyTo(loaderSourcePath);
            }
        }

        private static void CopyCustomScripts_Android(string pathToBuildProject)
        {

        }
	}
}