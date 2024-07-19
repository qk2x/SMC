using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Common.UIComponent
{
	public class UIComProgressBar : UICom
	{
		[SerializeField] private TextMeshProUGUI progressName;
		[SerializeField] private Slider progressSlider;

		public void SetName(string n)
		{
			progressName.text = n;
		}

		public void SetProgress(float p)
		{
			progressSlider.value = p;
		}
	}
}