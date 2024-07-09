using System.Collections.Generic;
using Game.Resource;
using Game.Resource.ResItem;
using UnityEngine;

namespace Game.Map.Entity
{
	public struct BigMapResData
	{
		public int Ms;
		public int Ma;
		public int Mb;
		//private int Mc;

		public int Total => Ms + Ma + Mb;
		
		public int GetResCount(ResType rt)
		{
			switch (rt)
			{
				case ResType.S:
					return Ms;
				case ResType.A:
					return Ma;
				case ResType.B:
					return Mb;
				// case ResType.C:
				// 	return Mc;
			}

			return 0;
		}





		public override string ToString()
		{
			return $"S:{Ms}, A:{Ma}, B:{Mb}";
		}
	}
}