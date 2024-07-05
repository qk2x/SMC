using System;
using System.Collections.Generic;
using Framework.EventSystem;
using UnityEngine;

namespace Framework.BUI
{
	public class UIBase  : MonoBehaviour
	{
		#region Event

		private struct EventRegister
		{
			public EventName Name;
			public Action<EventParam> Callback;
		}

		private List<EventRegister> _registers = new List<EventRegister>(8);

		protected void RegisterEvent(EventName eventName, Action<EventParam> cb)
		{
			_registers.Add(new EventRegister() {Name = eventName, Callback = cb});
		}

		private void RegisterAll()
		{
			foreach (var register in _registers)
			{
				GameEventDispatcher.Instance.Register(register.Name, register.Callback);
			}
		}

		#endregion
		
		[NonSerialized] public UIContext Context;
		public State State => Context.State;
		
		/// <summary>
		/// 是否可点击
		/// </summary>
		/// <returns></returns>
		public virtual bool CanClick()
		{
			return Context.State == State.Shown;
		}

		private void Awake()
		{
			UIBindButton();
		}

		/// <summary>
		/// 绑定按钮
		/// </summary>
		protected virtual void UIBindButton()
		{
		}

		/// <summary>
		/// 注册事件监听
		/// </summary>
		protected virtual void UIRegisterGameEvent()
		{
		}

		/// <summary>
		/// 反注册事件监听
		/// </summary>
		protected virtual void UIUnregisterGameEvent()
		{
		}

		private void UnRegisterAll()
		{
			foreach (var register in _registers)
			{
				GameEventDispatcher.Instance.Unregister(register.Name, register.Callback);
			}

			_registers.Clear();
		}
		
		/// <summary>
		/// 打开前自动处理流程
		/// </summary>
		public void AutoOnCreate()
		{
			OnCreate();
		}
		
		/// <summary>
		/// UIManager Open时，第一次创建会调用该接口, 用来创建UI中的东西
		/// </summary>
		protected virtual void OnCreate()
		{
		}
		
		/// <summary>
		/// 显示窗口
		/// </summary>
		public void Show()
		{
			OnPreTop();
			PreShowAuto();
			gameObject.SetActive(true);
			_registers.Clear();
			UIRegisterGameEvent();
			RegisterAll();
			Context.State = State.Shown;
			OnShow();
		}
		
		/// <summary>
		/// UI处于顶层时调用
		/// </summary>
		public void OnPreTop()
		{
			OnTop();
		}
		
		protected virtual void OnTop()
		{
		}

		/// <summary>
		/// 窗口显示前调用
		/// </summary>
		protected virtual void PreShow()
		{
		}
		
		private void PreShowAuto()
		{
			PreShow();
		}
		
		/// <summary>
		/// 窗口显示时调用，可以在这里刷新UI
		/// </summary>
		protected virtual void OnShow()
		{
		}
		
		/// <summary>
		/// 隐藏窗口，看不见时会调用
		/// </summary>
		public void Hide()
		{
			PreHide();
			gameObject.SetActive(false);
			OnHide();
			UIUnregisterGameEvent();
			UnRegisterAll();
			Context.State = State.Hiden;
		}

		protected virtual void PreHide()
		{
		}

		protected virtual void OnHide()
		{
			Context.State = State.Hiden;
		}

		public void Close(bool destroy = true, bool isShowOther = true)
		{
			UIManager.Instance.Close(Context.type, destroy, isShowOther);
		}

		protected void Back(bool destroy = true)
		{
			UIManager.Instance.Back(Context.type, destroy);
		}
		
		public void OnUIClose()
		{
			AutoPreClose();
			PreClose();
			OnClose();
		}
		
		/// <summary>
		/// 关闭前自动解绑
		/// </summary>
		private void AutoPreClose()
		{
		}

		/// <summary>
		/// 窗口关闭前调用
		/// </summary>
		protected virtual void PreClose()
		{
		}

		/// <summary>
		/// 窗口关闭时调用
		/// </summary>
		protected virtual void OnClose()
		{
		}
		
		/// <summary>
		/// 其他UI窗口关闭
		/// </summary>
		/// <param name="uiType"></param>
		public virtual void OnOtherUIClose(Type uiType)
		{
		}
		
		/// <summary>
		/// 其他UI窗口打开
		/// </summary>
		/// <param name="uiType"></param>
		public virtual void OnOtherUIOpen(Type uiType, UILayer.ShowMode showMode)
		{
		}
	}
}