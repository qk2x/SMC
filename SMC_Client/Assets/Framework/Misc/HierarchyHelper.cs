using UnityEngine;

namespace Framework.Misc
{
    public static class HierarchyHelper
    {
        #region Extension
        /// <summary>
        /// 获取HierarchyPath
        /// </summary>
        public static string GetPath(this Transform transform)
        {
            return GetHierarchyPath(transform);
        }
        #endregion
        
        #region Path
        public static string GetHierarchyPath(GameObject go)
        {
            if (go == null)
            {
                return string.Empty;
            }

            string path = string.Empty;
            Transform cur = go.transform;
            while (cur != null)
            {
                path = "/" + cur.name + path;
                cur = cur.transform.parent;
            }

            return path;
        }

        public static string GetHierarchyPath(Transform transform)
        {
            if (transform == null)
            {
                return null;
            }
            return GetHierarchyPath(transform.gameObject);
        }
        #endregion
    }
}