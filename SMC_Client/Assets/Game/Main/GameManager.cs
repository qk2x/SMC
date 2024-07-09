using Framework.Misc;
using Game.Map;

namespace Game.Manager
{
	public class GameManager : SingletonUnity<GameManager>, IRestartComponent
	{
		void Start()
		{
			InitGame();
		}

		void InitGame()
		{
			BigMap.Instance.InitBigMap();
		}
	}
}