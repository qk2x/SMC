using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Misc
{
	public class UnityUtils
	{
		/// <summary>
		/// 点中ui
		/// </summary>
		///
		static readonly List<RaycastResult> Uilist = new List<RaycastResult>();

		public static GameObject GetClickUITarget()
		{
			PointerEventData eventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);

			eventData.position = Input.mousePosition;

			UnityEngine.EventSystems.EventSystem.current?.RaycastAll(eventData, Uilist);

			//这个函数抄的unity源码的，就是取第一个值
			RaycastResult raycast = new RaycastResult(); // FindFirstRaycast(uilist);
			for (var i = 0; i < Uilist.Count; ++i)
			{
				if (Uilist[i].gameObject == null)
					continue;

				else
				{
					raycast = Uilist[i];
					break;
				}
			}

			var go = ExecuteEvents.GetEventHandler<IEventSystemHandler>(raycast.gameObject);

			//既然没拿到button之类的，说明只有Image挡住了，取点中结果即可
			if (go == null)
			{
				go = raycast.gameObject;
			}
			
			Uilist.Clear();
			
			return go;
		}
	}
}