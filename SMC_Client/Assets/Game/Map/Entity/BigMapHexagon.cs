using System.Collections.Generic;
using Framework.Misc;
using Game.Map.PathFinder;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Map.Entity
{
    public class BigMapHexagon : BigMapEntity, IPathFindableNode
    {
        public enum SolarSystem
        {
            Normal,
            Obstacle,
            Gate,
        }
        
        [SerializeField] private TextMeshPro testTxt;
        [SerializeField] private SpriteRenderer mainSprite;
        [SerializeField] private SpriteRenderer subSprite;
        
        private List<BigMapEntity> m_EntityHas = new List<BigMapEntity>(0);
        public int Cx { get; private set; }
        public int Cy { get; private set; }
        
        public SolarSystem SystemType { get; private set; }

        public BigMapResData MapResData;
        public HexHexShaft HexShaft { get; private set; }
        
        public bool IsInteractive { get; private set; }

        public void SetSolarSystemType(SolarSystem type)
        {
            SystemType = type;
            if (type == SolarSystem.Obstacle)
            {
                mainSprite.color = Color.black;
            }
        }
        
        public void SetCoordinate(int x, int y)
        {
            Cx = x;
            Cy = y;

            testTxt.text = $"({Cx}, {Cy})";
            //testTxt.gameObject.SetActive(false);
            name = $"HEX:({Cx}, {Cy})";
        }

        public void SetResData(BigMapResData resData)
        {
            MapResData = resData;

            var color = BigMapColor.GetColor(resData);
            if (color != Color.clear)
            {
                mainSprite.color = color;
            }
        }

        /// <summary>
        /// 在地图块上加矿井
        /// </summary>
        /// <param name="mapStruct"></param>
        public HexHexShaft FormShaft()
        {
            HexShaft = new HexHexShaft(this) { Type = HexStructType.Shaft };
            
            return HexShaft;
        }

        public void OnClick()
        {
            DLog.Log($"坐标:<{Cx},{Cy}>, {MapResData}");
        }

        public void AddEntity(BigMapEntity e)
        {
            m_EntityHas.Add(e);
        }

        public override BigMapEntityType GetEntityType()
        {
            return BigMapEntityType.MapHex;
        }

        public void SetInteractive(bool on)
        {
            subSprite.color = on ? System.Drawing.Color.Khaki.ToUnityColor() : Color.black;
        }

        public void GetLinkedPathFindable(IList<IPathFindableNode> list)
        {
            list.AddRange(BigMap.Instance.GetLinked(this));
        }

        public PathFindableData m_PathFindableData;


        public PathFindableData PathNodeData
        {
            get
            {
                if (m_PathFindableData == null)
                {
                    m_PathFindableData = new PathFindableData();
                }

                return m_PathFindableData;
            }
        }

        void Update()
        {
            testTxt.gameObject.SetActive(true);
            if (PathNodeData.Step != int.MaxValue)
            {
                testTxt.text = PathNodeData.Step.ToString() + $" ({Cx}, {Cy})";
                testTxt.color = Color.green;
            }
            else
            {
                testTxt.text = $"({Cx}, {Cy})";;
                testTxt.color = Color.red;
            }
        }

        public override string ToString()
        {
            return $"({Cx}, {Cy})-{PathNodeData.Step}";;
        }
    }
}
