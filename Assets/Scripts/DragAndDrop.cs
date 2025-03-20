using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour,IPointerUpHandler,IPointerDownHandler,IDragHandler,IBeginDragHandler,IEndDragHandler
{

    private GameObject selectedObject;
    BoxCollider col;
    public bool isDragging;
    [SerializeField]private Vector3 startPos;
    [SerializeField] private BaseInteractivity interactivity;

    private void Start()
    {
        startPos = transform.position;
        col = GetComponent<BoxCollider>();
        //interactivity = GetComponent<BaseInteractivity>();
    }
    void Update()
    {
#if UNITY_ANDROID
     if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {

            }

            if (touch.phase == TouchPhase.Moved)
            {

            }

            if (touch.phase == TouchPhase.Ended)
            {

            }
        }
#endif


    }

    #region PC dragging codes
    //private void OnMouseUp()
    //{
    //    if (selectedObject != null)
    //    {
    //        isDragging = false;
    //        var mousePos = Input.mousePosition;
    //        Vector3 position = new Vector3(mousePos.x, mousePos.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
    //        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
    //        selectedObject.transform.position = new Vector3(worldPos.x, 0.2f, worldPos.z);
    //        // selectedObject.transform.DOLocalMove(new Vector3(worldPos.x,0.2f,worldPos.z),.5f).SetEase(Ease.OutBack);
    //        var nameCon = GetComponent<NameController>();
    //        PCComponentManager.Instance.HighlightObject(nameCon, false);
    //    }


    //}
    //private void OnMouseDown()
    //{
    //    if (selectedObject == null)
    //    {

    //        //
    //        //col.enabled = false;
    //        RaycastHit hit = GameManager.Instance.CastRay();
    //        if (hit.collider != null)
    //        {
    //            if (!hit.collider.CompareTag("draggable"))
    //            {
    //                return;
    //            }
    //            selectedObject = hit.collider.gameObject;

    //           // UIManager.Instance.mousePositionText.text = "mouse hit";
    //        }
    //    }
    //    else
    //    {

    //    }
    //}

    //private void OnMouseDrag()
    //{
    //    if (selectedObject != null)
    //    {
    //        isDragging = true;
    //        var mousePos = Input.mousePosition;
    //        Vector3 position = new Vector3(mousePos.x,mousePos.y,Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
    //        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
    //        selectedObject.transform.position = new Vector3(worldPos.x,10f,worldPos.z);
    //        var nameCon = GetComponent<NameController>();
    //        PCComponentManager.Instance.HighlightObject(nameCon);

    //    }
    //}
    #endregion




    private RaycastHit CastRay()
    {
        
        float mousePosX = Input.mousePosition.x;
        float mousePosY = Input.mousePosition.y;
        Camera cam = Camera.main;
        Vector3 screenMousePosFar = new Vector3(mousePosX, mousePosY, cam.farClipPlane);
        Vector3 screeMousePosNear = new Vector3(mousePosX,mousePosY,cam.nearClipPlane);

        Vector3 screenWorldMouseFar = cam.ScreenToWorldPoint(screenMousePosFar);
        Vector3 screenWorldMouseNear = cam.ScreenToWorldPoint(screeMousePosNear);
        
        RaycastHit hit;
       
        Physics.Raycast(screenWorldMouseNear,(screenWorldMouseFar - screenWorldMouseNear).normalized,out hit);
       // Debug.LogError($"CastRay{hit.transform.name}");
        Debug.Log($"Mouse Position in Build: {Input.mousePosition}");
       // UIManager.Instance.mousePositionText.text = Input.mousePosition.ToString();
        return hit;
    }

    public void ResetPosition(bool isObjectActive = false)
    {
        transform.position = startPos;
        ActivateObject(isObjectActive);
    }

    public void ActivateObject(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    #region event system
    public void OnPointerUp(PointerEventData eventData)
    {
        if (selectedObject != null)
        {
            isDragging = false;
            var mousePos = eventData.position;
            Vector3 position = new Vector3(mousePos.x, mousePos.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPos.x, 0.2f, worldPos.z);
            // selectedObject.transform.DOLocalMove(new Vector3(worldPos.x,0.2f,worldPos.z),.5f).SetEase(Ease.OutBack);
            var nameCon = GetComponent<NameController>();
            PCComponentManager.Instance.HighlightObject(nameCon, false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (selectedObject == null)
        {

            //
            //col.enabled = false;
            RaycastHit hit = GameManager.Instance.CastRay(eventData);
            if (hit.collider != null)
            {
                if (!hit.collider.CompareTag("draggable"))
                {
                    return;
                }
                selectedObject = hit.collider.gameObject;

                // UIManager.Instance.mousePositionText.text = "mouse hit";
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (selectedObject != null)
        {
            isDragging = true;
            var mousePos = eventData.position;
            Vector3 position = new Vector3(mousePos.x, mousePos.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPos.x, 10f, worldPos.z);
            var nameCon = GetComponent<NameController>();
            PCComponentManager.Instance.HighlightObject(nameCon);

            TutorialManager.Instance.CurrentSelectedInteractivity = interactivity;// add check if tutorial mode
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    #endregion
}
