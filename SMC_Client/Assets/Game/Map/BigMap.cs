using Framework.BUI;
using Framework.Misc;
using Game.Map.Entity;
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

		private const int MAX = 1000000;
		private const int MIN = 1000;
		
		public void InitBigMap()
		{
			UIManager.Instance.Open(typeof(UIBigMap), UILayer.ShowMode.Simple);

			CreateBigMapMono();
		}

		void CreateBigMapMono()
		{
			
		}

		public static Vector3 GetPos(int m, int n, float z = 0)
		{
			var px = m * E;
			var py = (Mathf.Abs(m) % 2) * C;

			py += n * D;
            
			return new Vector3(px, py, z);
		}

		public void OnHexClick()
		{
		}

		public BigMapResData GetBigMapResData()
		{
			float r = Random.Range(1f, 100f);

			//先算有S,A,B得概率
			if (r > 90)
			{
								
			}
		}
	}


}