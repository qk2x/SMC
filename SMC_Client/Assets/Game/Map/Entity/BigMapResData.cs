using System.Collections.Generic;
using Game.Resource.ResItem;

namespace Game.Map.Entity
{
	public struct BigMapResData
	{
		private int Ms;
		private int Ma;
		private int Mb;
		private int Mc;

		public int GetTotal()
		{
			return Ms + Ma + Mb + Mc;
		}

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
				case ResType.C:
					return Mc;
			}

			return 0;
		}
	}
}