using System.Collections.Generic;
using Game.Map.Entity;
using Unity.Mathematics;

namespace Game.Map
{
	public class BigMapPath
	{
		static List<BigMapHexagon> _adjacent = new List<BigMapHexagon>(6);
		static List<BigMapHexagon> _candidates = new List<BigMapHexagon>(6);
		static HashSet<BigMapHexagon> _checkedHexagons = new HashSet<BigMapHexagon>();
		
		public void PathFind(BigMapHexagon from, BigMapHexagon to, List<BigMapHexagon> path)
		{
			var cur = from;
			path.Add(cur);
			_candidates.Add(cur);
			
			int minGuessDistance = CalGuessDistance(from, to);
			
			while (cur.Cx != to.Cx || cur.Cy != to.Cy)
			{
				foreach (var candidate in _candidates)
				{
					cur = candidate;
					if (cur.Cx != to.Cx || cur.Cy != to.Cy)
					{
					}
				}
			}
		}
		
				

		
		public static int CalGuessDistance(BigMapHexagon from, BigMapHexagon to)
		{
			return math.abs(from.Cx - to.Cx) + math.abs(from.Cy - to.Cy);
		}
	}
}