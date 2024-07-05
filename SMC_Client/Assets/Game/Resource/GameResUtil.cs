using Framework.Misc;
using Game.Resource.ResItem;
using UnityEditor;
using UnityEngine;

namespace Game.Resource
{
	public static class GameResUtil
	{
		public static Color GetColorByResType(ResType resType)
		{
			switch (resType)
			{
				case ResType.S:
					return System.Drawing.Color.Gold.ToUnityColor();
				case ResType.A:
					return System.Drawing.Color.Peru.ToUnityColor();
				case ResType.B:
					return System.Drawing.Color.Brown.ToUnityColor();
				case ResType.C:
					return System.Drawing.Color.Gray.ToUnityColor();
			}
			
			return default;
		}
	}
}