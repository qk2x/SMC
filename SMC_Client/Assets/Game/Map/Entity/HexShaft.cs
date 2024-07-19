namespace Game.Map.Entity
{
	public class HexHexShaft : HexStruct
	{
		public HexMiningTeam MiningTeam { get; private set; }
		
		public HexHexShaft(BigMapHexagon hexagon)
		{
			OwnerHexagon = hexagon;
		}

		public void Gather()
		{
			var resData = OwnerHexagon.MapResData;
			
		}

		public void SetMiningTeam(HexMiningTeam team)
		{
			MiningTeam = team;
		}
	}
}