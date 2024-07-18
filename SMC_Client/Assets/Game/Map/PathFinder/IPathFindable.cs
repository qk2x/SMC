using System;
using System.Collections.Generic;
using Framework.Misc;
using UnityEngine;

namespace Game.Map.PathFinder
{
	public interface IPathFindableNode
	{
		public void GetLinkedPathFindable(IList<IPathFindableNode> list);
		public PathFindableData PathNodeData { get; }
	}

	//[Serializable]
	public class PathFindableData
	{
		[SerializeField]
		private int m_Step = int.MaxValue;
		
		public int Step
		{
			get => m_Step;
			set => m_Step = value;
		}
		
	
		public int Cost = 0;
		public int Total = 0;

		public void Reset()
		{
			Step = int.MaxValue;
			Cost = 0;
			Total = 0;
		}
	}
}