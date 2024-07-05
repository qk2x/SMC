using UnityEngine;

namespace Game.Map
{
    public class BigMapMono : MonoBehaviour
    {
        [SerializeField] private BigMapHexagon hexagon;


        void Start()
        {
            CreateBigMap(8);
        }
        
        public void CreateBigMap(int size)
        {
            for (int x = 0; x < size; ++x)
            {
                for (int y = 0; y < size; ++y)
                {
                    var hex = GameObject.Instantiate(hexagon, BigMap.GetPos(x, y), Quaternion.identity);
                    hex.SetCoordinate(x, y);

                    if (x != 0 || y != 0)
                    {
                        hex = GameObject.Instantiate(hexagon, BigMap.GetPos(-x, -y), Quaternion.identity);
                        hex.SetCoordinate(-x, -y);

                        hex = GameObject.Instantiate(hexagon, BigMap.GetPos(x, -y), Quaternion.identity);
                        hex.SetCoordinate(x, -y);

                        hex = GameObject.Instantiate(hexagon, BigMap.GetPos(-x, y), Quaternion.identity);
                        hex.SetCoordinate(-x, y);
                    }
                }
            }

        }


    }
}
