using System;
using Game.Map.Entity;
using Game.Resource;
using Game.Resource.ResItem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Game.Map.UI
{
	public class UIHexagonMineInfo : MonoBehaviour
	{
		[SerializeField] private UIResItem mineSupper;
		[SerializeField] private UIResItem mineAdv;
		[SerializeField] private UIResItem mineBase;
		
		[SerializeField] private Slider mineSupperSlider;
		[SerializeField] private Slider mineAdvSlider;
		[SerializeField] private Slider mineBaseSlider;
		
		[SerializeField] private Image mineSupperImage;
		[SerializeField] private Image mineAdvImage;
		[SerializeField] private Image mineBaseImage;
		
		private BigMapHexagon m_CurHexagon;
		private int m_LastUpdateFrame;

		private void OnEnable()
		{
			mineSupperImage.color = GameResUtil.GetColorByResType(ResType.S);
			mineAdvImage.color = GameResUtil.GetColorByResType(ResType.A);
			mineBaseImage.color = GameResUtil.GetColorByResType(ResType.B);
		}

		public void SetMapHexagon(BigMapHexagon hex)
		{
			m_CurHexagon = hex;
		}

		void Update()
		{
			if (Time.frameCount - m_LastUpdateFrame > 3)
			{
				if (!Equals(m_CurHexagon, null))
				{
					RefreshInfo();
				}
			}
		}

		private void RefreshInfo()
		{
			m_LastUpdateFrame = Time.frameCount;
			
			mineSupper.Set(ResType.S, m_CurHexagon.MapResData.GetResCount(ResType.S));
			mineAdv.Set(ResType.A, m_CurHexagon.MapResData.GetResCount(ResType.A));
			mineBase.Set(ResType.B, m_CurHexagon.MapResData.GetResCount(ResType.B));

			mineBaseSlider.value = m_CurHexagon.MapResData.GetResCount(ResType.B) / BigMap.MapConfig.mineBaseStd;
			mineAdvSlider.value = m_CurHexagon.MapResData.GetResCount(ResType.A) / BigMap.MapConfig.mineAdvStd;
			mineSupperSlider.value = m_CurHexagon.MapResData.GetResCount(ResType.S) / BigMap.MapConfig.mineSupperStd;
		}
	}
} 