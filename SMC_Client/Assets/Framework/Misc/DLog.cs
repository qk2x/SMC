using System.Diagnostics;
using UnityEngine;

namespace Framework.Misc
{
	public static class DLog
	{
		[Conditional("ENABLE_LOG")]
		public static void Log(string msg, params object[] para)
		{
			UnityEngine.Debug.LogFormat(msg, para);
		}
    
		[Conditional("ENABLE_LOG")]
		public static void Log(string msg, Color color)
		{
			UnityEngine.Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{msg}</color>");
		}
		[Conditional("ENABLE_LOG")]
		public static void Log(string msg)
		{
			UnityEngine.Debug.Log(msg);
		}

		[Conditional("ENABLE_LOG")]
		public static void Warning(string msg, params object[] para)
		{
			UnityEngine.Debug.LogWarningFormat(msg, para);
		}
		[Conditional("ENABLE_LOG")]
		public static void Warning(string msg)
		{
			// string detail = $"{Time.frameCount}, {Time.realtimeSinceStartup}";
			// UnityEngine.Debug.LogWarning($"{detail} -> " + msg);
        
			UnityEngine.Debug.LogWarning(msg);
		}

		[Conditional("ENABLE_LOG")]
		public static void Error(string msg, params object[] para)
		{
			UnityEngine.Debug.LogErrorFormat(msg, para);
		}
		[Conditional("ENABLE_LOG")]
		public static void Error(string msg)
		{
			UnityEngine.Debug.LogError(msg);
		}
	}
}