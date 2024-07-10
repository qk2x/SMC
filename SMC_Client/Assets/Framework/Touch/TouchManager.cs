using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TouchPhase = UnityEngine.TouchPhase;

namespace Framework.Misc.Touch
{
    [DefaultExecutionOrder(-100)]
    public class TouchManager : SingletonUnity<TouchManager>, IRestartComponent
    {
        public enum TouchStage
        {
            Down,
            Click,
            Drag,
            Up,
        }

        public static float MoveThreshold = 10;
        public const float ClickThreshold = .5f;
        private const int MAXCount = 5;
        [NonSerialized] public int touchWorking = 1;
        private TouchDetail[] touches;
        
        public LayerMask mask;
        
        private Camera _cam;
        public List<TouchDetail> alives = new List<TouchDetail>();
        
        public Camera cam
        {
            get
            {
                if (_cam == null || !_cam.gameObject.activeSelf)
                {
                    _cam = Camera.main;
                }

                // _cam ??= Camera.main;
                return _cam;
            }
        }

        public void SetCamera(Camera c)
        {
            _cam = c;
        }

        private bool EventSystemEnabledLastFrame { get; set; }
        private bool EventSystemEnabledThisFrame { get; set; }
        private bool m_HasInit;

        public void Init()
        {
            if (!m_HasInit)
            {
                noBlockLayer = LayerMask.NameToLayer("NoBlockUI");
                touches = new TouchDetail[MAXCount];
                for (int i = 0; i < MAXCount; i++)
                {
                    touches[i] = new TouchDetail();
                    touches[i].Reset();
                }

#if !UNITY_EDITOR
        MoveThreshold = Screen.width * MoveThreshold / 1080f;
#endif

                DontDestroyOnLoad(gameObject);
                touchWorking = 0;
                // transform.AddComponent<DestoryOnReboot>();
                m_HasInit = true;
            }
        }

        /// <summary>
        /// 每一帧计算之前重置数据
        /// </summary>
        private void UpdateAtFrameStart()
        {
            for (int i = 0; i < MAXCount; ++i)
            {
                ref var td = ref touches[i];
                
                if (td.f > 1)
                {
                    td.f -= 1;
                }
                
                if (td.f == 1)
                {
                    td.endTouchMono = null;
                
                    td.f = 0;
                }
                
                if (td.end)
                {
                    td.f = 3;
                    //td.startTouchMono = null;
                    td.Reset();
                }
            }
        }

        public void Update()
        {
            if (touchWorking != 0) return;

            // if (GameCameraSwitcher.CurType == eGameCameraType.CharacterRoom
            //     || GameCameraSwitcher.CurType == eGameCameraType.None)
            // {
            //     return;
            // }

            UpdateAtFrameStart();

            var current = UnityEngine.EventSystems.EventSystem.current;
            EventSystemEnabledThisFrame = current!= null
                                          && current.enabled
                                          && current.currentInputModule != null;

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                ref var td = ref GetDetailByFinger(-1, true);
                td.InitData(-1, -1, (int)Input.mousePosition.x, (int)Input.mousePosition.y, TouchStage.Down);
            }
            else if (Input.GetMouseButton(0))
            {
                ref TouchDetail td = ref GetDetailByFinger(-1);
                if (td.IsAlive())
                    td.UpdateData((int)Input.mousePosition.x, (int)Input.mousePosition.y, false);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ref TouchDetail td = ref GetDetailByFinger(-1);
                if (td.IsAlive())
                {
                    td.UpdateData((int)Input.mousePosition.x, (int)Input.mousePosition.y, true);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                ref var td = ref GetDetailByFinger(-2, true);
                td.InitData(-2, -2, (int)Input.mousePosition.x, (int)Input.mousePosition.y, TouchStage.Down);
            }
            else if (Input.GetMouseButton(1))
            {
                ref TouchDetail td = ref GetDetailByFinger(-2);
                if (td.IsAlive())
                    td.UpdateData((int)Input.mousePosition.x, (int)Input.mousePosition.y, false);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                ref TouchDetail td = ref GetDetailByFinger(-2);
                if (td.IsAlive())
                    td.UpdateData((int)Input.mousePosition.x, (int)Input.mousePosition.y, true);
            }
#endif

#if UseDebugFight
        if (Input.GetKeyUp(KeyCode.C))
        {
            FromLua.GameGlobalData.GlobalData.IsTeamRed = !FromLua.GameGlobalData.GlobalData.IsTeamRed;
        }
#endif
            for (int i = 0; i < Input.touchCount; i++)
            {
                UnityEngine.Touch tc = Input.GetTouch(i);
                if (tc.phase == TouchPhase.Began)
                {
                    ref var td = ref GetDetailByFinger(tc.fingerId, true);
                    td.InitData(tc.fingerId, i, (int)tc.position.x, (int)tc.position.y, TouchStage.Down);
                }
                else
                {
                    TouchDetail td = GetDetailByFinger(tc.fingerId);
                    if (td.IsAlive())
                    {
                        bool isEnd = tc.phase == TouchPhase.Ended || tc.phase == TouchPhase.Canceled;
                        td.UpdateData((int)tc.position.x, (int)tc.position.y, isEnd);
                    }
                    else
                        DLog.Warning("[TouchManager] 错误的FingerId: " + tc.fingerId);
                }
                
            }

            UpdateAtFrameEnd();
            EventSystemEnabledLastFrame = EventSystemEnabledThisFrame;
        }

        void UpdateAtFrameEnd()
        {
            UpdateAlive();
            foreach (var v in alives)
                v.SetTouchPoint(alives.Count);
        }
        
        void UpdateAlive()
        {
            alives.Clear();
            foreach (var v in touches)
                if (v.IsAlive())
                    alives.Add(v);
        }
        
        private ref TouchDetail GetEmpty()
        {
            float older = float.MaxValue;

            ref TouchDetail touchDetail = ref TouchDetail.InvalidDetail;

            for (int i = 0; i < touches.Length; ++i)
            {
                ref var td = ref touches[i];

                if (!td.IsAlive())
                    return ref td;

                if (older > td.startTime)
                {
                    older = td.startTime;
                    touchDetail = td;
                }
            }


            DLog.Error("Touch有问题?...10个缓存都不够？");
            touchDetail.end = false;

            return ref touchDetail;
        }


        #region Interface

        /// <summary>
        /// 单纯的做下检测
        /// </summary>
        /// <param name="detail"></param>
        public void DoRayCastEnd(TouchDetail detail)
        {
            var ray = cam.ScreenPointToRay(detail.position);
            var isClickHit = Physics.Raycast(ray, out var hitPoint, 
                100, mask);
            if (isClickHit)
            {
                TouchMono touchMono = null;

                Transform tr = hitPoint.transform;
                touchMono = tr.GetComponent<TouchMono>();

                if (touchMono == null)
                {
                    int recursive = 3;
                    while (recursive >= 0)
                    {
                        tr = tr.parent;

                        if (tr == null)
                        {
                            break;
                        }

                        touchMono = tr.GetComponent<TouchMono>();

                        if (touchMono == null)
                        {
                            --recursive;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                detail.endTouchMono = touchMono;
            }
        }

        public TouchMono GetCurTouchMono(TouchDetail detail)
        {
            if (cam == null)
            {
                return null;
            }
            
            var ray = cam.ScreenPointToRay(detail.position);
            var isClickHit = Physics.Raycast(ray, out var hitPoint, 
                100, mask);
                
            TouchMono touchMono = null;
            
            if (isClickHit)
            {
                Transform tr = hitPoint.transform;
                touchMono = tr.GetComponent<TouchMono>();

                if (touchMono == null)
                {
                    int recursive = 3;
                    while (recursive >= 0)
                    {
                        tr = tr.parent;

                        if (tr == null)
                        {
                            break;
                        }

                        touchMono = tr.GetComponent<TouchMono>();

                        if (touchMono == null)
                        {
                            --recursive;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return touchMono;
        }

        private RaycastHit[] hitPoints = new RaycastHit[32];
        private List<RaycastHit> hitList = new List<RaycastHit>(32);
        /// <summary>
        /// 做射线看看，在左键或者touch(0)按下或者抬起时会做一个
        /// </summary>
        /// <param name="detail"></param>
        public Vector3 DoRayCast(TouchDetail detail)
        {
            if (cam == null)
            {
                DLog.Warning("[TouchManager] 没有相机.");
                return default;
            }
            var ray = cam.ScreenPointToRay(detail.position);
            var hitCount = Physics.RaycastNonAlloc(ray, hitPoints, 
                10000,mask);
            
            detail.hitCount = hitCount;
            detail.hitPoints = hitPoints;
            detail.clickHit = hitCount > 0;

            hitList.Clear();
            for (int i = 0; i < hitCount; ++i)
            {
                hitList.Add(hitPoints[i]);
            }

            var p = cam.transform.position;
            
            hitList.Sort((hit, raycastHit) => 
                //hit.transform.position.z.CompareTo(raycastHit.transform.position.z)
                Vector3.SqrMagnitude(p - hit.point).CompareTo(Vector3.SqrMagnitude(p - raycastHit.point))
                );
            
            if (hitCount > 0)
            {
                TouchMono touchMono = null;
                
                for (int i = 0; i < hitList.Count; ++i)
                {
                    var hitPoint = hitList[i];
                    
                    Transform tr = hitPoint.transform;
                    
                    touchMono = tr.GetComponent<TouchMono>();

                    if (touchMono == null)
                    {
                        int recursive = 3;
                        while (recursive >= 0)
                        {
                            tr = tr.parent;

                            if (tr == null)
                            {
                                break;
                            }

                            touchMono = tr.GetComponent<TouchMono>();

                            if (touchMono == null)
                            {
                                --recursive;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    Debug.DrawLine(ray.origin, hitPoint.point, Color.red);
                    
                    detail.startTouchMono = touchMono;
                    detail.lastTouchPoint = detail.touchPoint;
                    detail.touchPoint = hitPoint.point;
                    
                    if (touchMono != null)
                    {
                        if (detail.touchStage == TouchStage.Down)
                        {
                            if (touchMono.OnPointerDown(detail))
                            {
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                detail.startTouchMono = null;
            }

            return detail.touchPoint;
        }

        public ref TouchDetail GetClickDetail()
        {
            if (touchWorking != 0)
            {
                return ref TouchDetail.InvalidDetail;
            }
#if UNITY_EDITOR
            return ref GetDetailByFinger(-1);
#else
            return ref GetDetailByTouchIndex(0);
#endif
        }
        


        /// <summary>
        /// 是否点击了，并且没有滑动
        /// </summary>
        /// <returns></returns>
        public bool IsSafeClick()
        {
            ref var td = ref GetClickDetail();
            return td.SafeClick();
        }

        /// <summary>
        /// 获得指定finger的Touch Detail 数据，-1和-2分别为鼠标左右键
        /// </summary>
        /// <param name="fingerID"></param>
        /// <param name="getEmpty"></param>
        /// <returns></returns>
        public ref TouchDetail GetDetailByFinger(int fingerID, bool getEmpty = false) //-1 mouse left , -2 mouse right 
        {
            for (int i = 0; i < MAXCount; ++i)
            {
                if (touches[i].fingerID == fingerID)
                {
                    return ref touches[i];
                }
            }

            if (getEmpty)
            {
                ref var td = ref GetEmpty();
                return ref td;
            }

            return ref TouchDetail.InvalidDetail;
        }

        /// <summary>
        /// 通过touch index获得touch detail数据。-1和-2分别为鼠标左右键
        /// </summary>
        /// <param name="index"></param>
        /// <param name="getEmpty"></param>
        /// <returns></returns>
        public ref TouchDetail GetDetailByTouchIndex(int index, bool getEmpty = false) //-1 mouse left , -2 mouse right 
        {
            for (int i = 0; i < MAXCount; ++i)
            {
                if (touches[i].touchIndex == index)
                {
                    return ref touches[i];
                }
            } 

            if (getEmpty)
            {
                ref var td = ref GetEmpty();
                return ref td;
            }

            return ref TouchDetail.InvalidDetail;
        }

        private int noBlockLayer;

        public bool IsOnUI(TouchDetail td)
        {
            return IsOnUI(td.fingerID, (int) td.position.x, (int) td.position.y);
        }
        
        public bool IsOnUI(int finger, int x, int y)
        {
            bool res = true;
            var current = UnityEngine.EventSystems.EventSystem.current;
            if (EventSystemEnabledThisFrame && EventSystemEnabledLastFrame)
            {
                if (finger == -1 || finger == -2)
                    res = current.IsPointerOverGameObject();
                else
                {
                    res = IsPointerOverUIObject(x, y);
                    // res = current.IsPointerOverGameObject(finger);
                }

                if (res)
                {

                    PointerEventData pointer = new PointerEventData(current)
                    {
                        position = new Vector2(x, y)
                    };

                    List<RaycastResult> raycastResults = new List<RaycastResult>();
                    current.RaycastAll(pointer, raycastResults);

                    bool ignore = true;
                    
                    foreach (var raycast in raycastResults)
                    {
                        if (raycast.gameObject.layer != noBlockLayer)
                        {
                            ignore = false;
                            break;
                        }
                    }

                    if (ignore)
                    {
                        res = false;
                    }
                    
                    if (openLogs)
                    {
#if UNITY_EDITOR
                        List<UnityEngine.Object> focus = new List<UnityEngine.Object>();
                        foreach (var v in raycastResults)
                            focus.Add(v.gameObject);
                        UnityEditor.Selection.objects = focus.ToArray();
#endif
                    }
                }

            }
            else
            {
                return false;
            }
            
            return res;
        }
        
        /// <summary>
        ///  works with New Input system:
        /// </summary>
        /// <returns></returns>
        private bool IsPointerOverUIObject(int x, int y)
        {
            var touchPosition = new Vector2(x, y);
            var eventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current)
            {
                position = touchPosition
            };
            var results = new List<RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        public void Output()
        {
            int i = 0;
            foreach (var v in touches)
                if (v.IsAlive())
                    DLog.Log($"touch {i}:{v}");
        }

        #endregion

        private bool lastFocus = true;

        private void OnApplicationFocus(bool focus)
        {
            if (focus == false && lastFocus)
            {
                if (touches != null)
                {
                    for (int i = 0; i < MAXCount; i++)
                    {
                        if (touches[i] != null)
                        {
                            touches[i].Reset();
                        }
                    }
                }
            }

            lastFocus = focus;
        }
    }
}

