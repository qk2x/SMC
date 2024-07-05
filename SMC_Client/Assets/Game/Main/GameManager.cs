using Framework.Misc;
using Game.Map;

namespace Game.Manager
{
	public class GameManager : SingletonUnity<GameManager>, IRestartComponent
	{
		void Awake()
		{
			
		}

		void InitGame()
		{
			BigMap.Instance.InitBigMap();
		}
	}
}