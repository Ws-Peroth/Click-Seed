using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlobalEventReceiver
{
    public void Regist(IGlobalEventReceiver observeInstance, string[] eventIds)
    {
        foreach (var eventId in eventIds)
        {
            Debug.Log($"Regist: {eventId}");
            GlobalEventController.Instance.Regist(observeInstance, eventId);
        }
    }
    public void Unregist(IGlobalEventReceiver observeInstance, string[] eventIds)
    {
        foreach (var eventId in eventIds)
        {
            Debug.Log($"Unregist: {eventId}");
            GlobalEventController.Instance.Unregist(observeInstance, eventId);
        }
    }
    public abstract void ReceiveEvent(string EventId, object[] param);
    public abstract object GetOriginObject();
}