using UnityEngine;

namespace Game.Map
{
    [CreateAssetMenu]
    public class BigMapConfig : ScriptableObject
    {
        [Range(5000, 20000)]
        public float mineBaseMax = 10000f;
        [Range(500, 3000)]
        public float mineAdvMax = 2000f;
        [Range(50, 400)]
        public float minSupperMax = 200f;

        public float mineEmptyWeight = 0.8f;
        public float mineBaseWeight = 0.9f;
        public float mineAdvWeight = 0.99f;

        public float mineBaseStd = 38000f;
        public float mineAdvStd = 7000f;
        public float mineSupperStd = 300f;
    }
}
