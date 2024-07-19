namespace Game.Map.Entity
{
	public enum HexStructType
	{
		/// <summary>
		/// 存储物资，并可以分发
		/// </summary>
		Warehouse,
		/// <summary>
		/// 矿井
		/// </summary>
		Shaft,
	}
	
	public class HexStruct : HexEntity
	{
		public HexStructType Type;
	}
}