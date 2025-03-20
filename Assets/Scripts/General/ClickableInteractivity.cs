using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableInteractivity : BaseInteractivity
{
    public UnityEvent enableEvent;
    public UnityEvent disableEvent;
    public override void EnableObject()
    {
        base.EnableObject();
        enableEvent?.Invoke();
    }

    public override void DisableObject()
    {
        base.DisableObject();
        disableEvent?.Invoke();
    }
}
