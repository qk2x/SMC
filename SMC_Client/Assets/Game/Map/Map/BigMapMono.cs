using System;
using System.Collections.Generic;
using Framework.Misc;
using Framework.Qath;
using Game.Map.Entity;
using Game.Map.PathFinder;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;

namespace Game.Map
{
    public class BigMapMono : MonoBehaviour
    {
        [SerializeField] private BigMapHexagon hexagonPrefab;
        [SerializeField] private BigMapHq hqPrefab;
        
        private Dictionary<ValueTuple<int, int>, BigMapHexagon> hexMap;

        private Dictionary<int, BigMapHq> hqMap;

        public static BigMapMono Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [Button()]
        public void Step(int step)
        {
            PathFinder.PathFinder.FindLinked(BigMapMono.Instance.GetHexagonByCoordinate(0, 0), step);
        }
        
        [Button()]
        public void TestFindPath()
        {
            List<IPathFindableNode> path = new List<IPathFindableNode>();
            
            PathFinder.PathFinder.FindPath(
                Instance.GetHexagonByCoordinate(0, 0), 
                Instance.GetHexagonByCoordinate(-3, 2), path);
        }

        public void CreateHq(int x, int y)
        {
            hqMap = new Dictionary<int, BigMapHq>(1);
            var hq = GameObject.Instantiate(hqPrefab, 
                BigMap.GetPos(x, y, GetEntityShowZ(BigMapEntity.BigMapEntityType.MapHq)),
                Quaternion.identity, transform);
            hqMap[10000] = hq;
            hexMap[new ValueTuple<int, int>(x, y)].AddEntity(hq);
        }
        
        public void CreateBigMap(int sizeX, int sizeY)
        {
            int count = (2 * sizeX - 1) * (2 * sizeY - 1);
            hexMap = new Dictionary<(int, int), BigMapHexagon>(count);
            
            for (int x = 0; x < sizeX; ++x)
            {
                for (int y = 0; y < sizeY; ++y)
                {
                    CreateHex(x, y);

                    if (x != 0)
                    {
                        CreateHex(-x, y);
                    }

                    if (y != 0)
                    {
                        CreateHex(x, -y);
                    }

                    if (x != 0 && y != 0)
                    {
                        CreateHex(-x, -y);
                    }
                }
            }

        }

        private void CreateHex(int x, int y)
        {
            var hex = GameObject.Instantiate(hexagonPrefab, 
                BigMap.GetPos(x, y, GetEntityShowZ(BigMapEntity.BigMapEntityType.MapHex)),
                Quaternion.identity, transform);
            
            hex.SetCoordinate(x, y);
            if (ProbabilityMath.RandomFloat() > 0.7f)
            {
                hex.SetSolarSystemType(BigMapHexagon.SolarSystem.Obstacle);
            }
            else
            {
                hex.SetResData(BigMap.RandomBigMapResData());
            }

            hex.SetInteractive(false);
            
            hexMap.Add(new ValueTuple<int, int>(x, y), hex);
        }

        private bool m_MouseDown;
        private Vector2 m_MouseDownPos;
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!m_MouseDown)
                {
                    m_MouseDown = true;
                    m_MouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                else
                {
                    var newMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                // 获取鼠标点击位置的世界坐标
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // 检测点击位置是否在Sprite的Collider上
                Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

                if (hitCollider != null)
                {
                    var hex  = hitCollider.GetComponent<BigMapHexagon>();
                    hex.OnClick();

                    BigMap.Instance.OnHexClick(hex);
                }
            }
        }

        /// <summary>
        /// 显示用排序
        /// </summary>
        /// <param name="et"></param>
        /// <returns></returns>
        public float GetEntityShowZ(BigMapEntity.BigMapEntityType et)
        {
            switch (et)
            {
                case BigMapEntity.BigMapEntityType.MapHex:
                    return 0;
                case BigMapEntity.BigMapEntityType.MapHq:
                    return -9;
            }

            DLog.Error($"无深度类型:{et}");
            
            return 999;
        }

        public BigMapHexagon GetHexagonByCoordinate(int x, int y)
        {
            if (hexMap.TryGetValue((x, y), out var hex))
            {
                return hex;
            }
            else
            {
                return null;
            }
        }
    }
}
