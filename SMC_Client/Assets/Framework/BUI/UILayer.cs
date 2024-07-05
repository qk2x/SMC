using System;
using System.Collections.Generic;
using Framework.Misc;
using UnityEngine;

namespace Framework.BUI
{
    /// <summary>
    /// UI根节点
    /// </summary>
    public class UILayer : MonoBehaviour
    {
        public enum ShowMode
        {
            /// <summary>
            ///直接弹出,会处于当前layer最前,无法back
            /// </summary>
            Popup,

            ///弹出之后会被加入stack,可以back,会隐藏之前最上层UI
            Stack,

            //队列管理，保证只打开队列中第一个
            Queue,

            /// <summary>
            /// 只是打开管理,不触发其他UI的OnTop,也不加入队列，直接当在最前面。
            /// </summary>
            Simple,
        }

        /// <summary>
        /// 同camera的order渲染顺序
        /// </summary>
        public int layerId;

        private static List<UIContext> _operators = new List<UIContext>(8);
        private static Queue<UIContext> _queueList = new Queue<UIContext>(8);
        [NonSerialized] public UIContext Context;
        


        public static void Clean()
        {
            _operators.Clear();
            _queueList.Clear();
        }

        
        public static UIContext LastOpenedUIType { get; private set; }

        public static bool IsTop(UIContext context)
        {
            if (context.showMode == ShowMode.Queue)
            {
                if (_queueList.Count > 0)
                {
                    return _queueList.Peek() == context;
                }

                return false;
            }
            else
            {
                if (_operators.Count > 0)
                {
                    return _operators[_operators.Count - 1] == context;
                }

                return false;
            }
        }

        private void ApplySafeArea()
        {
            var r = Screen.safeArea;
            var rt = transform as RectTransform;
            var anchorMin = r.position;
            var anchorMax = r.position + r.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
        }

        private void Init(UIContext ctx)
        {
            ctx.UI.Context = ctx;
            ctx.State = State.Initiliazed;
            ctx.UI.AutoOnCreate();
        }

        /// <summary>
        /// 设置layerId并处理层级
        /// </summary>
        public void SetLayer(int newLayerId)
        {
            layerId = newLayerId;
            var childCount = transform.parent.childCount;
            for (int index = 0; index < childCount; index++)
            {
                var childTran = transform.parent.GetChild(index);
                var childLayer = childTran.GetComponent<UILayer>();
                if (childLayer)
                {
                    if (newLayerId < childLayer.layerId)
                    {
                        transform.SetSiblingIndex(index);
                        return;
                    }
                }
            }
        }

        private void DoTop(UIContext ctx)
        {

        }

        private void DoHide(UIContext ctx, bool isClear = false)
        {
            ctx.State = State.Hiden;
        }

        private void DoShow(UIContext ctx)
        {
            DoTop(ctx);
            if (ctx.UI.State != State.Shown)
            {
                ctx.UI.Show();
            }
        }

        public void OperatorOpen(UIContext ctx)
        {
            switch (ctx.showMode)
            {
                case ShowMode.Simple:
                    if (ctx.State == State.None)
                    {
                        Init(ctx);
                    }

                    DoShow(ctx);
                    ctx.UI.transform.SetAsLastSibling();
                    break;
                case ShowMode.Popup:
                    if (ctx.State == State.None)
                    {
                        Init(ctx);
                    }

                    if (!_operators.Contains(ctx))
                    {
                        _operators.Add(ctx);
                    }
                    else
                    {
                        DLog.Error("[UIManager] 重复Popup打开了一个UI?!");
                    }

                    DoShow(ctx);
                    ctx.UI.transform.SetAsLastSibling();
                    break;
                case ShowMode.Stack:
                    if (_operators.Count > 0)
                    {
                        var old = _operators[_operators.Count - 1];

                        DoHide(old);
                    }

                    if (ctx.State == State.None)
                    {
                        Init(ctx);
                    }

                    _operators.Add(ctx);
                    DoShow(ctx);
                    break;
                case ShowMode.Queue:
                {
                    if (ctx.State == State.None)
                    {
                        Init(ctx);
                    }

                    if (_operators.Count == 0)
                    {
                        DoShow(ctx);
                    }
                    else
                    {
                        _queueList.Enqueue(ctx);
                    }
                }
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool OperatorClose(UIContext ctx, bool isClear = false)
        {
            switch (ctx.showMode)
            {
                case ShowMode.Simple:


                    DoHide(ctx, true);

                    return true;

                case ShowMode.Popup:

                    DoHide(ctx, isClear);

                    if (_operators.Remove(ctx))
                    {
                        if (!isClear && _operators.Count > 0)
                        {
                            var last = _operators[_operators.Count - 1];
                            DoTop(last);
                            last.UI.OnPreTop();
                            LastOpenedUIType = last;
                        }
                    }
                    else
                    {
                        DLog.Error("要关闭的UI不存在");
                    }

                    return true;

                case ShowMode.Stack:
                {
                    int index = -1;
                    for (int i = 0; i < _operators.Count; ++i)
                    {
                        if (_operators[i] == ctx)
                        {
                            index = i;
                            break;
                        }
                    }

                    if (index == -1)
                    {
                        DLog.Error($"要关闭的UI不存在:{ctx.type.Name}");
                        return false;
                    }

                    DoHide(ctx, isClear);
                    _operators.RemoveAt(index);

                    if (!isClear && _operators.Count > 0)
                    {
                        var last = _operators[_operators.Count - 1];
                        LastOpenedUIType = last;
                        DoShow(last);
                    }

                    return true;
                }
                case ShowMode.Queue:
                {
                    if (_queueList.Peek() != ctx)
                    {
                        DLog.Error("第一个打开的窗口不是该窗口");
                    }

                    DoHide(ctx, isClear);
                    _queueList.Dequeue();

                    if (!isClear && _queueList.Count > 0)
                    {
                        var head = _queueList.Peek();
                        DoShow(head);
                    }

                    return true;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public bool Back(UIContext ctx)
        {
            if (_operators.Count == 0)
            {
                throw new Exception("没有界面可以关闭");
            }

            switch (ctx.showMode)
            {
                case ShowMode.Popup:
                    DLog.Error("Popup模式不支持Back操作");
                    break;
                case ShowMode.Stack:
                {
                    if (_operators[_operators.Count - 1] != ctx)
                    {
                        DLog.Error("Stack中最后一个打开的窗口不是该窗口");
                    }

                    DoHide(ctx);
                    _operators.RemoveAt(_operators.Count - 1);
                    if (_operators.Count > 0)
                    {
                        var last = _operators[_operators.Count - 1];
                        DoShow(last);
                    }

                    break;
                }
                case ShowMode.Queue:
                {
                    DLog.Error("Queue模式不支持Back操作");
                    break;
                }
                case ShowMode.Simple:
                    DoHide(ctx);
                    break;
                default:
                    DLog.Error("未实现的UI模式.");
                    break;
            }

            return !_operators.Contains(ctx);
        }
    }
}