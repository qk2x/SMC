using System;
using System.Collections.Generic;
using Framework.Misc;
using Game.Map.Entity;
using UnityEngine;

namespace Game.Map
{
    public class BigMapMono : MonoBehaviour
    {
        [SerializeField] private BigMapHexagon hexagonPrefab;
        [SerializeField] private BigMapHq hqPrefab;
        
        private Dictionary<ValueTuple<int, int>, BigMapHexagon> hexMap;

        private Dictionary<int, BigMapHq> hqMap;

        void Start()
        {
            CreateBigMap(10, 5);
            CreateHq(0, 0);
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
            hex.SetResData(BigMap.RandomBigMapResData());
            hexMap.Add(new ValueTuple<int, int>(x, y), hex);
        }

        void Update()
        {
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

                    BigMap.Instance.OnHexClick();
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
    }
}