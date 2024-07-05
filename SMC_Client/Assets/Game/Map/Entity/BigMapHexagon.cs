using System.Collections.Generic;
using Framework.Misc;
using TMPro;
using UnityEngine;

namespace Game.Map.Entity
{
    public class BigMapHexagon : BigMapEntity
    {
        [SerializeField] private TextMeshPro testTxt;

        private List<BigMapEntity> m_EntityHas = new List<BigMapEntity>(0);
        private int m_Cx = 0;
        private int m_Cy = 0;
    
        public void SetCoordinate(int x, int y)
        {
            m_Cx = x;
            m_Cy = y;

            testTxt.text = $"({m_Cx}, {m_Cy})";
            testTxt.gameObject.SetActive(false);
            name = $"HEX:({m_Cx}, {m_Cy})";
        }

        public void OnClick()
        {
            DLog.Log($"OnClick:{m_Cx}， {m_Cy}");
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
