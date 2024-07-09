using System.Collections.Generic;
using Framework.BUI;
using Framework.Misc;
using Framework.Qath;
using Game.Map.Entity;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Map
{
	public class BigMap : Singleton<BigMap>, IRestartComponent
	{
		private const float A = 0.5f;
		private const float B = 0.25f;
		private const float C = 0.433f;
		private const float D = 0.433f * 2;
		private const float E = A + B;

		public static BigMapConfig MapConfig { private set; get; }
		public UIBigMap BigMapUI { get; private set; }
		public BigMapHexagon CurInteractive { get; private set; }

		public BigMapMono BigMapMono;

		public void InitBigMap()
		{
			BigMapUI = UIManager.Instance.Open(typeof(UIBigMap), UILayer.ShowMode.Simple) as UIBigMap;
			BigMapMono = BigMapMono.Instance;

			if (MapConfig == null)
			{
				MapConfig = Addressables.LoadAssetAsync<BigMapConfig>(
						"Assets/Game/Map/Res/BigMapConfig.asset")
					.WaitForCompletion();
			}

			CreateBigMapMono();
		}

		void CreateBigMapMono()
		{

		}

		public void OnHexClick(BigMapHexagon hex)
		{
			if (CurInteractive != hex)
			{
				if (CurInteractive != null)
				{
					CurInteractive.SetInteractive(false);
				}

				CurInteractive = hex;
				hex.SetInteractive(true);

				BigMapUI.ShowHexagonInfo(hex);
			}
			else
			{
				BigMapUI.HideHexagonInfo();
			}
		}

		public static Vector3 GetPos(int m, int n, float z = 0)
		{
			var px = m * E;
			var py = (Mathf.Abs(m) % 2) * C;

			py += n * D;

			return new Vector3(px, py, z);
		}

		public static BigMapResData RandomBigMapResData(int calTimes = 100)
		{
			BigMapResData data = new BigMapResData();

			if (ProbabilityMath.RandomFloat(0, 1) > 0.9)
			{
				for (int i = 0; i < calTimes; ++i)
				{
					var key = ProbabilityMath.RandomFloat(0, 1);
					var val = key * key * key;
					AddMine(ref data, val);
				}
			}

			return data;
		}

		static void AddMine(ref BigMapResData data, float val)
		{
			float a = MapConfig.mineEmptyWeight;
			float b = MapConfig.mineBaseWeight;
			float s = MapConfig.mineAdvWeight;

			var qb = MapConfig.mineBaseMax;
			var qa = MapConfig.mineAdvMax;
			var qs = MapConfig.minSupperMax;

			if (val > a)
			{
				if (val < b)
				{
					data.Mb += (int)((val - a) / (b - a) * qb);
				}
				else if (val < s)
				{
					data.Ma += (int)((val - b) / (s - b) * qa);
				}
				else
				{
					data.Ms += (int)((val - s) / (1 - s) * qs);
				}
			}
		}

		/// <summary>
		/// 当前逻辑下的6边型相邻格子
		/// </summary>
		/// <param name="target"></param>
		/// <param name="adjacent"></param>
		public void GetAdjacent(BigMapHexagon target, List<BigMapHexagon> adjacent)
		{
			var cx = target.Cx;
			var cy = target.Cy;

			var hex = BigMapMono.GetHexagonByCoordinate(cx + 1, cy);
			if (hex != null)
			{
				adjacent.Add(hex);
			}
			
			hex = BigMapMono.GetHexagonByCoordinate(cx -1, cy);
			if (hex != null)
			{
				adjacent.Add(hex);
			}
			
			hex = BigMapMono.GetHexagonByCoordinate(cx, cy + 1);
			if (hex != null)
			{
				adjacent.Add(hex);
			}
			
			hex = BigMapMono.GetHexagonByCoordinate(cx, cy - 1);
			if (hex != null)
			{
				adjacent.Add(hex);
			}
			
			hex = BigMapMono.GetHexagonByCoordinate(cx + 1, cy - 1);
			if (hex != null)
			{
				adjacent.Add(hex);
			}
			
			hex = BigMapMono.GetHexagonByCoordinate(cx - 1, cy - 1);
			if (hex != null)
			{
				adjacent.Add(hex);
			}
		}


	}
}