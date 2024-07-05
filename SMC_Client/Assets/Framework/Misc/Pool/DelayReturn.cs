using Framework.Misc.Pool;
using UnityEngine;

namespace Framework.Misc.Pool

{
    public class DelayReturn : MonoBehaviour
    {
        public float delay;
        public UnityComponentPool pool;
        public Component target;
        void Update()
        {
            delay -= Time.deltaTime;

            if (delay <= 0)
            {
                enabled = false;
                pool.Return(target);
            }
        }

    }
}