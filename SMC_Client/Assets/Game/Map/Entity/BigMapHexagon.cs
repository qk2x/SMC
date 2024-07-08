using System.Collections.Generic;
using Framework.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Map.Entity
{
    public class BigMapHexagon : BigMapEntity
    {
        [SerializeField] private TextMeshPro testTxt;
        [SerializeField] private SpriteRenderer mainSprite;
        
        private List<BigMapEntity> m_EntityHas = new List<BigMapEntity>(0);
        private int m_Cx = 0;
        private int m_Cy = 0;

        private BigMapResData m_MapResData;
        
        public void SetCoordinate(int x, int y)
        {
            m_Cx = x;
            m_Cy = y;

            testTxt.text = $"({m_Cx}, {m_Cy})";
            testTxt.gameObject.SetActive(false);
            name = $"HEX:({m_Cx}, {m_Cy})";
        }

        public void SetResData(BigMapResData resData)
        {
            m_MapResData = resData;

            var color = resData.GetColor();
            if (color != Color.clear)
            {
                mainSprite.color = color;
            }
        }

        public void OnClick()
        {
            DLog.Log($"坐标:<{m_Cx},{m_Cy}>, {m_MapResData}");
        }

        public void AddEntity(BigMapEntity e)
        {
            m_EntityHas.Add(e);
        }

        public override BigMapEntityType GetEntityType()
        {
            return BigMapEntityType.MapHex;
        }
    }
}
