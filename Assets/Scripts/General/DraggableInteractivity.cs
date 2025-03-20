using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DraggableInteractivity : BaseInteractivity
{
    [SerializeField] private DragAndDrop dragAndDrop;
    public UnityEvent onEnableEvent,onDisableEvent;

    public override void EnableObject()
    {
        base.EnableObject();
        dragAndDrop.enabled = true;
        onEnableEvent?.Invoke();
    }

    public override void DisableObject()
    {
        base.DisableObject();
        dragAndDrop.enabled = false;
        onDisableEvent?.Invoke();
    }
}
