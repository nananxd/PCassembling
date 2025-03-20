using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventList : MonoBehaviour
{
    public List<UnityEvent> eventList;
    void Start()
    {
        
    }

    public void InvokeEventAtIndex(int index)
    {
        eventList[index]?.Invoke();
    }
}
