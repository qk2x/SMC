using UnityEngine;
using System.Collections;
using System;
using System.Runtime.CompilerServices;
using Framework.EventSystem;
using Framework.Misc;

public class Singleton<T> where T : IRestartComponent, new() 
{
	private static T instance = default(T);

	public static T GetIns()
	{
		return Instance;
	}
	
	public static T getInstance()
	{
		return Instance;
	}

	public static T Instance
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get 
		{
			if (instance == null)
			{
				instance = new T();
				
				GameEventDispatcher.Instance.Register(EventName.GameRestart, OnRestart);
			}

			return instance;
		}
	}

	private static void OnRestart(EventParam obj)
	{
		instance.OnGameRestart();
	}
	
	public virtual void OnGameRestart()
	{
		instance = default;
	}
}