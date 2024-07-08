using System.Collections.Generic;
using Game.Resource;
using Game.Resource.ResItem;
using UnityEngine;

namespace Game.Map.Entity
{
	public struct BigMapResData
	{
		private int m_Ms;
		private int m_Ma;
		private int m_Mb;
		//private int Mc;

		public int Total => m_Ms + m_Ma + m_Mb;
		
		public int GetResCount(ResType rt)
		{
			switch (rt)
			{
				case ResType.S:
					return m_Ms;
				case ResType.A:
					return m_Ma;
				case ResType.B:
					return m_Mb;
				// case ResType.C:
				// 	return Mc;
			}

			return 0;
		}

		public void AddRandom(float val)
		{
			const float a = .0f;
			const float b = .9f;
			const float c = .99f;
			const int qb = 10000;
			const int qa = 10000;
			const int qs = 200;
			
			switch (val)
			{
				case < a:
					break;
				case < b:
					m_Mb += (int)((val - a)/ (b - a) * qb);
					break;
				case < c:
					m_Ma += (int)((val - b) / (c - b) * qa);
					break;
				default:
					m_Ms += (int)((val - c) / (1 - c) * qs);
					break;
			}
		}



		public override string ToString()
		{
			return $"S:{m_Ms}, A:{m_Ma}, B:{m_Mb}";
		}
	}
}