using Framework.Misc;
using UnityEngine;

namespace Game.Map
{
	public class BigMap : Singleton<BigMap>, IRestartComponent
	{
		private const float A = 0.5f;
		private const float B = 0.25f;
		private const float C = 0.433f;
		private const float D = 0.433f * 2;
		private const float E = A + B;
		public void InitBigMap()
		{
			
		}

		void CreateBigMapMono()
		{
			
		}
		
		public static Vector3 GetPos(int m, int n)
		{
			var px = m * E;
			var py = (Mathf.Abs(m) % 2) * C;

			py += n * D;
            
			return new Vector3(px, py, 0);
		}
	}
}