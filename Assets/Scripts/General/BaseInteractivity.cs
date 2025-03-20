using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractivity : MonoBehaviour
{
    [Header("reference to tutorial id should be the same name")]
    public string id;
    public virtual void DisableObject() { }
    public virtual void EnableObject() { }
}
