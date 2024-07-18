using System.Collections.Generic;
using Framework.Misc;
using Game.Map.Entity;
using Unity.VisualScripting;

namespace Game.Map.PathFinder
{
	public static class PathFinder
	{
		private static readonly HashSet<IPathFindableNode> CandidateNodes= new HashSet<IPathFindableNode>();
		static readonly List<IPathFindableNode> Linked = new List<IPathFindableNode>();
		static readonly List<IPathFindableNode> NextNodes = new List<IPathFindableNode>();
		static readonly HashSet<IPathFindableNode> UsedNodes = new HashSet<IPathFindableNode>();
		
		/// <summary>
		/// 遍历寻路算法，没有启发函数，只根据路径长度进行搜索
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="path"></param>
		public static void  FindPath(IPathFindableNode start, IPathFindableNode end, IList<IPathFindableNode> path)
		{
			CandidateNodes.Clear();
			Linked.Clear();
			NextNodes.Clear();
			UsedNodes.Clear();
			
			start.PathNodeData.Step = 0;
			CandidateNodes.Add(start);
			
			while (CandidateNodes.Count > 0)
			{
				foreach (var current in CandidateNodes)
				{
					if (current == end)
					{
						DLog.Error("找到了");
						BackTrack(end, start, path);
						return;
					}
					var currentStep = current.PathNodeData.Step + 1;
					
					Linked.Clear();
					current.GetLinkedPathFindable(Linked);

					foreach (var linkedNode in Linked)
					{
						if (linkedNode.PathNodeData.Step > currentStep)
						{
							NextNodes.Add(linkedNode);
							linkedNode.PathNodeData.Step = currentStep;
						}
					}
				}
				
				CandidateNodes.Clear();
				CandidateNodes.AddRange(NextNodes);
				NextNodes.Clear();
			}
		}

		public static void BackTrack(IPathFindableNode start, IPathFindableNode end, IList<IPathFindableNode> path)
		{
			IPathFindableNode current = start;
			int currentStep = start.PathNodeData.Step;
			
			
			CandidateNodes.Clear();
			Linked.Clear();
			NextNodes.Clear();
			UsedNodes.Clear();
			
			while (true)
			{
				path.Add(current);

				if (current == end)
				{
					return;
				}
				
				currentStep -= 1;
				current.GetLinkedPathFindable(Linked);

				foreach (var node in Linked)
				{
					if (node.PathNodeData.Step == currentStep)
					{
						current = node;
						break;
					}
				}
			}
		}
		
		public static List<IPathFindableNode> FindLinked(IPathFindableNode start, int step)
		{
			CandidateNodes.Clear();
			Linked.Clear();
			NextNodes.Clear();
			UsedNodes.Clear();
			
			start.PathNodeData.Step = 0;
			CandidateNodes.Add(start);
			
			while (CandidateNodes.Count > 0)
			{
				foreach (var current in CandidateNodes)
				{
					var currentStep = current.PathNodeData.Step + 1;

					if (currentStep > step)
					{
						continue;
					}
					
					Linked.Clear();
					current.GetLinkedPathFindable(Linked);

					foreach (var linkedNode in Linked)
					{
						if (linkedNode.PathNodeData.Step > currentStep)
						{
							NextNodes.Add(linkedNode);
							linkedNode.PathNodeData.Step = currentStep;
						}
					}
				}
				
				UsedNodes.AddRange(CandidateNodes);
				CandidateNodes.Clear();
				CandidateNodes.AddRange(NextNodes);
				
				NextNodes.Clear();
			}

			// foreach (var node in UsedNodes)
			// {
			// 	node.PathNodeData.Reset();
			// }

			return null;
		}
	}
}