using System.Diagnostics;
using System.IO;
using UnityEngine.Assertions;

namespace CustomBuildPipeline
{
	internal class WindowUtility
	{
		[Conditional("UNITY_EDITOR_WIN")]
		public static void OpenFolder(string path)
		{
#if UNITY_EDITOR_WIN
			DirectoryInfo di = new DirectoryInfo(path);
			
			Assert.IsTrue(di.Exists);
			Process.Start(@$"{path}");
#endif
		}
	}
}