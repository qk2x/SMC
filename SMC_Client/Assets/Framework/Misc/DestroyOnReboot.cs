using Framework.EventSystem;
using UnityEngine;

namespace Framework.Misc
{
    public class DestroyOnReboot : MonoBehaviour
    {
        private void Awake()
        {
            GameEventDispatcher.Instance.Register(
                EventName.GameReBoot, Reboot);
        }

        private void OnDestroy()
        {
            GameEventDispatcher.Instance.Unregister(
                EventName.GameReBoot, Reboot);
        }

        void Reboot(EventParam e)
        {
            if (gameObject != null)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}