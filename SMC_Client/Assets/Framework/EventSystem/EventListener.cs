using System;
using Framework.Misc.Pool;

namespace Framework.EventSystem
{
    public class EventParam : ISimpleMemoryClean, IDisposable
    {
        private static SimpleMemoryPool<EventParam> poolParm;
        public static EventParam Empty = new EventParam();
        
        public object objVal;
        public long longVal1;
        public long longVal2;
        public long longVal3;
        public double doubleVal;
        public bool boolVal;

        public static EventParam Get()
        {
            poolParm ??= new SimpleMemoryPool<EventParam>(15);
            var e = poolParm.Get();
            return e;
        }
        
        public static EventParam Get(bool b)
        {
            poolParm ??= new SimpleMemoryPool<EventParam>(15);
            var e = poolParm.Get();
            e.boolVal = b;
            return e;
        }
        
        public static EventParam Get(double d)
        {
            poolParm ??= new SimpleMemoryPool<EventParam>(15);
            var e = poolParm.Get();
            e.doubleVal = d;
            return e;
        }
        
        public static EventParam Get(long l)
        {
            poolParm ??= new SimpleMemoryPool<EventParam>(15);
            var e = poolParm.Get();
            e.longVal1 = l;
            return e;
        }
        
        public static EventParam Get(object o)
        {
            poolParm ??= new SimpleMemoryPool<EventParam>(15);
            var e = poolParm.Get();
            e.objVal = o;
            return e;
        }
        
        public static EventParam Get(object o, bool b)
        {
            poolParm ??= new SimpleMemoryPool<EventParam>(15);
            var e = poolParm.Get();
            e.objVal = o;
            e.boolVal = b;
            return e;
        }
        
        public static EventParam Get(int o, bool b)
        {
            poolParm ??= new SimpleMemoryPool<EventParam>(15);
            var e = poolParm.Get();
            e.longVal1 = o;
            e.boolVal = b;
            return e;
        }
        
        public static EventParam Get(long o, bool b)
        {
            poolParm ??= new SimpleMemoryPool<EventParam>(15);
            var e = poolParm.Get();
            e.longVal1 = o;
            e.boolVal = b;
            return e;
        }
        
        public static EventParam Get(long v, object o, bool b)
        {
            poolParm ??= new SimpleMemoryPool<EventParam>(15);
            var e = poolParm.Get();
            e.longVal1 = v;
            e.objVal = o;
            e.boolVal = b;
            return e;
        }
        
        public static EventParam Get(long v1, long v2, long v3, object o = null)
        {
            poolParm ??= new SimpleMemoryPool<EventParam>(15);
            var e = poolParm.Get();
            e.longVal1 = v1;
            e.longVal2 = v2;
            e.longVal3 = v3;
            e.objVal = o;
            return e;
        }

        public static EventParam GetEmpty()
        {
            return Empty;
        }
        
        public void Reset()
        {
            objVal = null;
            longVal1 = 0L;
            longVal2 = 0L;
            longVal3 = 0L;
            doubleVal = 0D;
        }

        public void Dispose()
        {
            poolParm.Return(this);
            GC.SuppressFinalize(this);
        }
    }
    
    
    public class EventListener : ISimpleMemoryClean
    {
        public EventName eventName;
        public Action<EventParam> callback;
        private int idx;
        
        private static SimpleMemoryPool<EventListener> poolListener;

        public static EventListener GetEventListener(EventName eveName, Action<EventParam> cb)
        {
            poolListener ??= new SimpleMemoryPool<EventListener>(30);
            
            EventListener e = poolListener.Get();

            e.Set(eveName, cb);
            
            return e;
        }

        private void Set(EventName eveName, Action<EventParam> cb)
        {
            eventName = eveName;
            callback = cb;
 
        }

        public static void ReturnEventListener(EventListener e)
        {
            poolListener.Return(e);
        }

        public void Reset()
        {
            eventName = EventName.InvalidEvent;
            callback = null;
            idx = 0;
        }
    }
}