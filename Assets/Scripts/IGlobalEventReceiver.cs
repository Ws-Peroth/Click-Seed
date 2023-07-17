using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlobalEventReceiver
{
    public void Regist(IGlobalEventReceiver observeInstance, string[] eventIds)
    {
        foreach (var eventId in eventIds)
        {
            GlobalEventController.Instance.Regist(observeInstance, eventId);
        }
    }
    public void Unregist(IGlobalEventReceiver observeInstance, string[] eventIds)
    {
        foreach (var eventId in eventIds)
        {
            GlobalEventController.Instance.Unregist(observeInstance, eventId);
        }
    }
    public abstract void ReceiveEvent(string EventId);
    public abstract object GetOriginObject();
}

public class GlobalEventController : Singleton<GlobalEventController>
{
    private Dictionary<string, List<object>> eventReceivers = new();

    public void SendEvent(string key)
    {
        if (eventReceivers.ContainsKey(key))
        {
            var receivers = eventReceivers[key];
            foreach (var receiver in receivers)
            {
                if (receiver is IGlobalEventReceiver Interface)
                {
                    Interface.ReceiveEvent(key);
                }
                else
                {
                    Debug.LogError("[GlobalEventController] :: receiver is not IGlobalEventReceiver Interface");
                }
            }
        }
    }

    public void Regist(IGlobalEventReceiver eventReceiver, string key)
    {
        if (!eventReceivers.ContainsKey(key))
        {
            List<object> Receivers = new() { eventReceiver.GetOriginObject() };
            eventReceivers.Add(key, Receivers);
            return;
        }
        if (!eventReceivers[key].Contains(eventReceiver.GetOriginObject()))
        {
            eventReceivers[key].Add(eventReceiver.GetOriginObject());
            return;
        }
    }

    public void Unregist(IGlobalEventReceiver observeInstance, string key)
    {
        if (eventReceivers.ContainsKey(key))
        {
            var Receivers = eventReceivers[key];
            if (Receivers.Contains(observeInstance.GetOriginObject()))
            {
                Receivers.RemoveAt(Receivers.FindIndex((match) => match == observeInstance.GetOriginObject()));
            }
        }
    }
}