using UnityEngine;

namespace Game.Map.Entity
{
	public abstract class BigMapEntity : MonoBehaviour
	{
		public enum BigMapEntityType
		{
			MapHex,
			MapHq,
			MapTraffic,
		}

		public abstract BigMapEntityType GetEntityType();
	}
}
