using Framework.Misc;
using Framework.Misc.Touch;
using UnityEngine;

public class TouchDetail
{
    public const int InValidNumber = -10000;

    public static TouchDetail InvalidDetail = new TouchDetail()
    {
        fingerID = InValidNumber,
    };

    public int fingerID;
    public int touchIndex;
    public int startX;
    public int startY;
    public int previousX;
    public int previousY;
    public Vector2 delta => new Vector2(curX - previousX, curY - previousY);
    public int curX;
    public int curY;
    public float startTime;
    /// <summary>
    /// 按下时是否在UI上
    /// </summary>
    public bool startOnUI;
    /// <summary>
    /// 结束时是否在UI上
    /// </summary>
    public bool endOnUI;
    public bool hasMove;
    public bool end;

    public int f;
    
    /// <summary>
    /// 按下或者抬起鼠标左键或者touch(0)的时候，是否raycast到了对象
    /// </summary>
    public bool clickHit;
    /// <summary>
    /// 是否拖拽了物体
    /// </summary>
    public bool draging;

    public Vector3 startPosition;
    public Vector3 position;
    public TouchManager.TouchStage touchStage;
    public TouchMono startTouchMono;
    public TouchMono endTouchMono;
    public Vector3 lastTouchPoint;
    public Vector3 touchPoint;
    public GameObject touchUIGo; 
    public int maxTouchCountSinceStart;

    public int hitCount;
    public RaycastHit[] hitPoints;


    private int lastTmCheck = 0;
    private TouchMono frameTm = null;
    public TouchMono GetRaycastTouchMono()
    {
        if (lastTmCheck != Time.frameCount)
        {
            lastTmCheck = Time.frameCount;
            frameTm = TouchManager.Instance.GetCurTouchMono(this);
        }
        
        return frameTm;
    }
    
    public void Reset()
    {
        fingerID = InValidNumber;
        end = false;
        clickHit = false;
        //TouchMono = null;
        draging = false;
        hasMove = false;
        touchPoint = default;
        maxTouchCountSinceStart = 0;
    }
    
    public void InitData(int id, int idx, int x, int y, TouchManager.TouchStage stage)
    {
        f = 0;
        fingerID = id;
        touchIndex = idx;
        startTime = Time.time;
        startX = curX = x;
        startY = curY = y;
        previousX = previousY = InValidNumber;
        var touch = TouchManager.Instance;
        startOnUI = touch.IsOnUI(fingerID, x, y);
        position = new Vector3(x, y, 0);
        hasMove = end = endOnUI = false;
        touchStage = stage;
        
        startTouchMono = null;
        endTouchMono = null;
        
        if (startOnUI)
        {
            if (TouchManager.Instance.openLogs)
            {
                DLog.Error("[TouchDetail] StartOnUI");
                touch.Output();
            }
            touchUIGo = UnityUtils.GetClickUITarget();
        }
        else
        {
            //0是touch(0) -1是鼠标左键
            if (idx == 0 || idx == -1)
            {
                startPosition = TouchManager.Instance.DoRayCast(this);
                lastTouchPoint = startPosition;
            }
        }
    }
    
    public void UpdateData(int x, int y, bool isEnd)
    {
        previousX = curX;
        previousY = curY;
        curX = x;
        curY = y;
        end = isEnd;
        position = new Vector3(x, y, 0);
        
        var touch = TouchManager.Instance;

        bool move = Mathf.Abs(curX - startX) > TouchManager.MoveThreshold
                    || Mathf.Abs(curY - startY) > TouchManager.MoveThreshold;

        if (end)
        {
            endOnUI = touch.IsOnUI(fingerID, x, y);
            if (touch.openLogs)
            {
                DLog.Log("[TouchDetail] EndOnUI");
                touch.Output();
            }

            touchStage = TouchManager.TouchStage.Up;
            if (Time.time - startTime < TouchManager.ClickThreshold)
            {
                touchStage = TouchManager.TouchStage.Click;
            }

            if (clickHit)
            {
                if (startTouchMono != null)
                {
                    if (move)
                    {
                        startTouchMono.OnEndDrag(this);
                    }
                    else
                    {
                        startTouchMono.OnPointerUp(this);
                        if (Time.time - startTime < TouchManager.ClickThreshold)
                        {
                            startTouchMono.OnPointerClick(this);
                        }
                    }
                }

                //有需要需要知道抬起的时候的那个目标
                if (move)
                {
                    if (touchIndex == 0 || touchIndex == -1)
                    {
                        TouchManager.Instance.DoRayCastEnd(this);
                    }
                }
                else
                {
                    endTouchMono = startTouchMono;
                }
            }
            else
            {
                endTouchMono = null;
            }
        }
        else
        {
            if (clickHit)
            {
                if (startTouchMono != null)
                {
                    if (!hasMove && move)
                    {
                        startTouchMono.OnBeginDrag(this);
                    }
                    else if (hasMove && move)
                    {
                        startTouchMono.OnDrag(this);
                    }
                }
            }
        }

        hasMove = move;
    }
    
    public bool IsAlive()
    {
        return fingerID != InValidNumber;
    }

    /// <summary>
    /// 处理可滑动情况下的Click，这个时候click事件触发于鼠标抬起的那一下
    /// </summary>
    /// <returns></returns>
    public bool SafeClick()
    {
        return end && fingerID != InValidNumber && !startOnUI && !hasMove && Input.touchCount < 2;
    }
    
    public bool IsStartTouch()
    {
        return fingerID != InValidNumber && previousX == InValidNumber;
    }
    
    public void SetTouchPoint(int num)
    {
        if (num > maxTouchCountSinceStart)
            maxTouchCountSinceStart = num;
    }

    public bool IsClick()
    {
        return
            end && !startOnUI && !endOnUI
            && !hasMove && Input.touchCount <= 1;
    }
}