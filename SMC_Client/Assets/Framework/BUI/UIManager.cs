#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Framework.EventSystem;
using Framework.Misc;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Framework.BUI
{
    public class UIManager : SingletonUnity<UIManager>, IRestartComponent
    {
        private static long _index = 0;
        
        private readonly Dictionary<Type, UIContext> m_UIContextsDic = new Dictionary<Type, UIContext>();
        private readonly Dictionary<string, GameObject> m_UIGameObjectsDic = new Dictionary<string, GameObject>();
        private readonly Dictionary<int, UILayer> m_LayerRootDic = new Dictionary<int, UILayer>();
        private readonly Dictionary<UIContext, IUIUpdate> m_UIUpdatesDic = new Dictionary<UIContext, IUIUpdate>();
        
        public GameObject layerTemplate;
        
        private UIConfig GetUIConfig(Type type)
        {
            var cfg = CustomAttributeExtensions.GetCustomAttribute<UIConfig>(type);
            if (cfg == null)
            {
                throw new Exception($"[UIManager] {type.FullName} 需要配置UIConfig，具体详情查看UIConfig Attribute。");
            }

            return cfg;
        }
    
        private void DoUIClose(UIContext ctx, Type type)
        {
            ctx.UI.OnUIClose();

            m_UIUpdatesDic.Remove(ctx);
            m_UIContextsDic.Remove(type);
        }

        public UILayer GetLayer(int layer)
        {
            if (!m_LayerRootDic.TryGetValue(layer, out var l))
            {
                if (layerTemplate == null)
                {
                    DLog.Error("LayerTemplate is null");
                }


                var root = transform;
                var layerGO = Instantiate(layerTemplate, root, false);
                layerGO.name = $"layer_{layer}";

                var rect = layerGO.transform as RectTransform;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
                rect.SetParent(transform);

                l = layerGO.GetComponent<UILayer>();
                l.SetLayer(layer);
                m_LayerRootDic.Add(layer, l);
            }

            return l;
        }

        private GameObject LoadPrefab(Type type, Transform parent)
        {
#if UNITY_EDITOR
            if (!type.IsSubclassOf(typeof(UIBase)))
            {
                DLog.Error("[UIManager] 类型必须是UIBase派生类");
            }
#endif
            var cfg = GetUIConfig(type);
            var key = cfg.Address;
            if (!m_UIGameObjectsDic.TryGetValue(key, out var go) || go == null)
            {
                var prefab = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();
                go = GameObject.Instantiate(prefab, parent);
                m_UIGameObjectsDic[key] = go;
            }

            return go;
        }
        
        public UIBase Open(
            Type type,
            UILayer.ShowMode mode, 
            object data = null, 
            Action onCloseCall = null,
            bool isReopen = false)
        {
            DLog.Log($"[UIManager] 打开UI: 类型:{type},方式:{mode}");
#if UNITY_EDITOR
            // DLog.Log($"[UIManager] 打开UI: 类型:{type},方式:{mode}");
            if (!type.IsSubclassOf(typeof(UIBase)))
            {
                DLog.Error("[UIManager] 类型必须是UIBase派生类");
                return null;
            }
#endif
            GameEventDispatcher.Instance.DispatchEvent(EventName.UIOpen, type);

            var cfg = GetUIConfig(type);
            var layer = GetLayer(cfg.Layer);

            if (!m_UIContextsDic.TryGetValue(type, out var ctx))
            {
                var go = LoadPrefab(type, layer.transform);

                ctx = new UIContext
                {
                    Prefab = cfg.Address,
                    Layer = layer,
                    Data = data,
                    OnCloseCall = onCloseCall,
                    State = State.None,
                    showMode = mode,
                    UI = go.GetComponent<UIBase>(),
                    Config = cfg.Clone(),
                    type = type,
                    Index = _index++,
                    IsReopen = isReopen,
                };
                ctx.UI.Context = ctx;
            }
            else
            {
                if (ctx.State == State.Shown)
                {
                    DLog.Error($"[UIManager] 重复UIOpen:{type}");
                    return ctx.UI;
                }
            }

            layer.OperatorOpen(ctx);

            if (ctx.UI is IUIUpdate updater && !m_UIUpdatesDic.ContainsKey(ctx))
            {
                m_UIUpdatesDic.Add(ctx, updater);
            }

            foreach (var ui in m_UIContextsDic)
            {
                ui.Value.UI.OnOtherUIOpen(ctx.type, ctx.showMode);
            }


            m_UIContextsDic[type] = ctx;

            GameEventDispatcher.Instance.DispatchEvent(EventName.UIOpenOver, type);


            return ctx.UI;
        }
    
        public void Back(Type type, bool destroy = true)
        {
#if UNITY_EDITOR
            if (!type.IsSubclassOf(typeof(UIBase)))
            {
                DLog.Error("[UIManager] 类型必须是UIBase派生类");
            }
#endif
            if (!m_UIContextsDic.TryGetValue(type, out var ctx)) return;

            if (ctx.Layer.Back(ctx))
            {
                DoUIClose(ctx, type);

                if (destroy)
                {
                    Destroy(ctx.UI.gameObject);
                    m_UIGameObjectsDic.Remove(GetUIConfig(type).Address);
                }
            }

            foreach (var ui in m_UIContextsDic)
            {
                ui.Value.UI.OnOtherUIClose(ctx.type);
            }
        }

        /// <summary>
        /// 关闭指定UI，可以选择是否Destroy预制体
        /// </summary>
        /// <param name="type"></param>
        /// <param name="destroy"></param>
        /// <param name="isOpenOther"></param>
        public void Close(Type type, bool destroy = true, bool isOpenOther = true)
        {
            DLog.Log($"[UIManager] Close UI: 类型:{type}");
#if UNITY_EDITOR
            if (!type.IsSubclassOf(typeof(UIBase)))
            {
                DLog.Error("[UIManager] 类型必须是UIBase派生类");
            }
#endif
            GameObject go = null;
            if (m_UIContextsDic.TryGetValue(type, out var ctx))
            {
                if (ctx.Layer.OperatorClose(ctx))
                {
                    DoUIClose(ctx, type);
                }

                go = ctx.UI.gameObject;

                if (destroy)
                {
                    Destroy(go);
                    m_UIGameObjectsDic.Remove(ctx.Prefab);
                }
                else
                {
                    go.SetActive(false);
                }
            }
            else
            {
                if (m_UIGameObjectsDic.TryGetValue(GetUIConfig(type).Address, out go))
                {
                    if (destroy)
                    {
                        Destroy(go);
                        m_UIGameObjectsDic.Remove(GetUIConfig(type).Address);
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
            }

            if (ctx != null)
            {
                foreach (var ui in m_UIContextsDic)
                {
                    ui.Value.UI.OnOtherUIClose(ctx.type);
                }
                
                if (isOpenOther)
                {
                    ctx.OnCloseCall?.Invoke();
                }

            }
            
            GameEventDispatcher.Instance.DispatchEvent(EventName.UIClose, type);
        }
    }
}
