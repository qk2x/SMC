using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMono : MonoBehaviour
{
    private static int layer = int.MinValue;
    
    protected virtual void Awake()
    {
        if (layer == Int32.MinValue)
        {
            layer = LayerMask.NameToLayer("RayClick");
        }
        gameObject.layer = layer;
    }

    public virtual bool OnPointerDown(TouchDetail touchDetail)
    {
        return true;
    }
    
    public virtual bool OnPointerUp(TouchDetail touchDetail)
    {
        return true;
    }
    
    public virtual bool OnPointerClick(TouchDetail touchDetail)
    {
        return true;
    }

    public virtual bool OnBeginDrag(TouchDetail touchDetail)
    {
        return true;
    }
    
    public virtual bool OnDrag(TouchDetail touchDetail)
    {
        return true;
    }
    
    public virtual bool OnEndDrag(TouchDetail touchDetail)
    {
        return true;
    }
}
