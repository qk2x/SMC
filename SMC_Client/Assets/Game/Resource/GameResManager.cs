using System.Collections.Generic;
using Framework.Misc;
using Game.Resource.ResItem;

namespace Game.Resource
{
	public class GameResManager : Singleton<GameResManager>, IRestartComponent
	{
		private Dictionary<ResType, int> m_ResMap = new Dictionary<ResType, int>(12);

		public void AddResCount(ResType rt, int count)
		{
			var old = GetResCount(rt);
			count += old;
			m_ResMap[rt] = count;
		}
		
		public int GetResCount(ResType rt)
		{
			m_ResMap.TryGetValue(rt, out var count);
			return count;
		}
	}
}