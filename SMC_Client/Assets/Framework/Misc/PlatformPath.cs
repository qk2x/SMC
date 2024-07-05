using UnityEngine;

namespace Framework.Misc
{
	public static class PlatformPath
	{
#if UNITY_EDITOR
		public static string STREAMING_ASSETS_PATH = Application.streamingAssetsPath;
		public static string PERSISTENT_DATA_PATH = Application.dataPath + "/../PersistentAssets";
		public static readonly string PACKAGE_CACHE_PATH = Application.dataPath + "/../PersistentAssets/PackageCache";
#elif UNITY_STANDALONE_WIN
			public static string STREAMING_ASSETS_PATH = Application.streamingAssetsPath;
			public static string PERSISTENT_DATA_PATH = Application.dataPath + "/PersistentAssets";
			public static readonly string PACKAGE_CACHE_PATH = Application.dataPath + "/PersistentAssets/PackageCache";
#elif UNITY_IOS
			public static string STREAMING_ASSETS_PATH = Application.streamingAssetsPath;
			public static string PERSISTENT_DATA_PATH = Application.persistentDataPath;
			public static readonly string PACKAGE_CACHE_PATH = Application.persistentDataPath + "/PackageCache";
#elif UNITY_ANDROID
			public static string STREAMING_ASSETS_PATH = Application.streamingAssetsPath;
			public static string PERSISTENT_DATA_PATH = Application.persistentDataPath;
			public static readonly string PACKAGE_CACHE_PATH = Application.persistentDataPath + "/PackageCache";
#endif
	}
}
			