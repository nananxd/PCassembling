using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIInteractivity : BaseInteractivity
{
    [SerializeField] private Button uiButton;
    public UnityEvent onEnableEvent;
    public UnityEvent onDisableEvent;
    public override void EnableObject()
    {
        base.EnableObject();
        uiButton.enabled = true;
        onEnableEvent?.Invoke();
    }

    public override void DisableObject()
    {
        base.DisableObject();
        uiButton.enabled = false;
        onDisableEvent?.Invoke();
    }
}
