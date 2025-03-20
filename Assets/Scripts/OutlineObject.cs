using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObject : MonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] private GameObject objectOutline;

    public void EnableOutlineObject(bool isEnable)
    {
        //outline.enabled = isEnable;
        objectOutline.gameObject.SetActive(isEnable);
    }
}
