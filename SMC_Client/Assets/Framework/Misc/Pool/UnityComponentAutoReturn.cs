using System;
using Framework.Misc.Pool;
using UnityEngine;

namespace Framework.Misc.Pool
{
    public class UnityComponentAutoReturn : MonoBehaviour
    {
        private UnityComponentPoolHandler poolHandler;
        public void Set(UnityComponentPoolHandler handler)
        {
            poolHandler = handler;
            enabled = true;
        }

        private void Update()
        {
            poolHandler.autoReturnTime -= Time.deltaTime;
            if (poolHandler.autoReturnTime < 0)
            {
                enabled = false;
                poolHandler.Return();
            }
        }
    }
}