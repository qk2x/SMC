using Game.Map.Entity;
using UnityEngine;

namespace Game.Map
{
	public static class BigMapColor
	{
		public static Color GetColor(BigMapResData data)
		{
			if (data.Total == 0)
			{
				return Color.clear;
			}
			else
			{
				return Color.grey;
			}
		}
	}
}