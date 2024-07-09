using System;
using DG.Tweening;
using Framework.BUI;
using Game.Map.Entity;
using Game.Map.UI;
using UnityEngine;

namespace Game.Map
{
	[UIConfig(Address = "Assets/Game/Map/Res/UIBigMap.prefab", Layer = UIConfig.eLayer.Normal)]
	public class UIBigMap : UIBase
	{
		[SerializeField] private UIHexagonMineInfo hexagonMineInfo;
		
		protected override void OnShow()
		{
			base.OnShow();
		}

		private Tween m_MineInfoTween;
		public void ShowHexagonInfo(BigMapHexagon hexagon)
		{
			m_MineInfoTween?.Kill();
			
			RectTransform rt = (RectTransform)hexagonMineInfo.transform;
			rt.anchoredPosition = new Vector2(350, rt.anchoredPosition.y);
			m_MineInfoTween = rt.DOAnchorPos(new Vector2(0, rt.anchoredPosition.y), 0.5f, true);
			
			hexagonMineInfo.SetMapHexagon(hexagon);
		}

		public void HideHexagonInfo()
		{
			m_MineInfoTween?.Kill();
			RectTransform rt = (RectTransform)hexagonMineInfo.transform;
			//rt.anchoredPosition = new Vector2(330, rt.anchoredPosition.y);
			m_MineInfoTween = rt.DOAnchorPos(new Vector2(330, rt.anchoredPosition.y), (350 - rt.anchoredPosition.x) / 330f * 0.5f, true);
		}
	}
}
