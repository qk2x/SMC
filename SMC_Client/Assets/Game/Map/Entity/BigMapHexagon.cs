using System.Collections.Generic;
using Framework.Misc;
using TMPro;
using UnityEngine;

namespace Game.Map.Entity
{
    public class BigMapHexagon : BigMapEntity
    {
        [SerializeField] private TextMeshPro testTxt;
        [SerializeField] private SpriteRenderer mainSprite;
        [SerializeField] private SpriteRenderer subSprite;
        
        private List<BigMapEntity> m_EntityHas = new List<BigMapEntity>(0);
        public int Cx { get; private set; }
        public int Cy { get; private set; }
        
        public BigMapResData MapResData { get; private set; }
        public bool IsInteractive { get; private set; }
        
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
    }
}
