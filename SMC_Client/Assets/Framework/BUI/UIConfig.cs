using System;

namespace Framework.BUI
{
    public class UIConfig : Attribute
    {
        public class eLayer
        {
            public const int Down = -100;
            public const int Normal = 0;
            public const int Up = 100;
            public const int Popup = 1000;
            public const int Top = 10000;
            public const int Overlay = 100000;
        }

        public string Address = null;
        public int Layer = 0;

        public UIConfig Clone()
        {
            UIConfig newCfg = new UIConfig
            {
                Address = Address,
                Layer = Layer,
            };
            
            return newCfg;
        }
    }
}
