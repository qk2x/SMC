#region

using System;
using Framework.Misc;

#endregion

namespace Framework.BUI
{
	public class UIContext
	{
		public string Prefab;
		public object Data;
		public State State;
		public UILayer Layer;
		public UIBase UI;
		public Type type;
		public UILayer.ShowMode showMode;
		public UIConfig Config;
		public long Index;
		public Action OnCloseCall;
		public bool IsReopen;
		public long Order => GetShowModeOrder() + Layer.layerId * 100000000 + Index;

		public override string ToString()
		{
			return
				$"prefab={Prefab} type={type.ToString()} showMode={showMode} ui={UI.transform.GetPath()}";
		}

		private long GetShowModeOrder()
		{
			return showMode switch
			{
				UILayer.ShowMode.Popup => 1000000000,
				UILayer.ShowMode.Stack => 2000000000,
				UILayer.ShowMode.Queue => 3000000000,
				UILayer.ShowMode.Simple => 4000000000
			};
		}
	}

	public enum State
	{
		None,
		Initiliazed,
		Shown,
		Hiden,
	}
}