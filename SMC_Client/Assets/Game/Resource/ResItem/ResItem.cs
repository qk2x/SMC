using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Resource.ResItem
{
    public class UIResItem : MonoBehaviour
    {
        [SerializeField] private Image img;
        [SerializeField] private TextMeshProUGUI text;

        [SerializeField] private bool autoUpdateCountFromResManager;
        private void OnEnable()
        {
            if (resType != ResType._)
            {
                SetColor(resType);
                SetCount(resType);
            }
        }

        public ResType resType;
        public int count;
        
        public void SetColor(ResType rt)
        {
            resType = rt;
            img.color = GameResUtil.GetColorByResType(rt);
        }
        
        public void SetCount(ResType rt)
        {
            int c = GameResManager.Instance.GetResCount(rt);
            if (c != count)
            {
                text.text = count.ToString();
                count = c;
            }
        }

        public void Set(ResType rt, int c)
        {
            autoUpdateCountFromResManager = false;
            
            SetColor(rt);
            text.text = c.ToString();
        }

        void Update()
        {
            if (Time.frameCount % 3 == 0)
            {
                if (resType != ResType._)
                {
                    SetCount(resType);
                }
            }
        }
    }
}
