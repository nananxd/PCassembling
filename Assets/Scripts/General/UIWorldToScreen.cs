using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIWorldToScreen : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject target3d;
    public bool isDragging;

    
    public void SetDragging(bool isEnable)
    {
        isDragging = isEnable;
    }
    // Update is called once per frame
    void Update()
    {
        if (target3d == null)
        {
            return;
        }

        if (!isDragging)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target3d.transform.position);

            screenPos.z = 0;
            float scaleX = Screen.width / 1920f;
            float scaleY = Screen.height / 1080f;
            Vector2 scaledOffset = new Vector2(offset.x * scaleX, offset.y * scaleY);
            transform.position = screenPos + (Vector3)scaledOffset;
        }

        
    }

    private void OnEnable()
    {
        //if (target3d == null)
        //{
        //    return;
        //}

        //Vector3 screenPos = Camera.main.WorldToScreenPoint(target3d.transform.position);

        //float scaleX = Screen.width / 1920f;
        //float scaleY = Screen.height / 1080f;
        //Vector2 scaledOffset = new Vector2(offset.x * scaleX, offset.y * scaleY);
        //transform.position = screenPos + (Vector3)scaledOffset;
        //if (target3d == null)
        //{
        //    return;
        //}

        //Vector3 screenPos = Camera.main.WorldToScreenPoint(target3d.transform.position);
        //transform.position = screenPos + offset;
    }
}
