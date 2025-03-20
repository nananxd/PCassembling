using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PcPart : NameController
{
    //public string partName;
    public bool isDontHaveControlUI;
    public string referencePartName;
    [SerializeField] private Ease easeType;
    [SerializeField] private float duration;
    private Vector3 currentRot;
    private Vector3 startPos;
    [Header("Rotation axis")]
    public bool isX;
    public bool isY;
    public bool isZ;

    private void Start()
    {
        currentRot = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(transform.eulerAngles);
        startPos = transform.position;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    RotateLeft();
        //}

        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    RotateRight();
        //}
    }

    private void OnEnable()
    {
        if (partsName == "opticSata2")
        {
            PCComponentManager.Instance.OpticChangeParent();
        }
        else if (partsName == "hddSata2")
        {
            PCComponentManager.Instance.HddChangeParent();
        }
    }


    public void RotateRight()
    {
        if (isX)
        {
            //currentRot.x += 45f;
            Rotate(45f,Vector3.right);
        }
        else if (isY)
        {
            // currentRot.y += 45f;
            Rotate(45f, Vector3.up);
        }
        else
        {
            Rotate(45f,Vector3.forward);
            //currentRot.z += 45f;
        }

        //currentRot.y += 45f;
        
        //transform.DORotate(new Vector3(currentRot.x, currentRot.y, currentRot.z) ,duration).SetEase(easeType);
        //transform.DORotateQuaternion();
       
    }

    public void RotateLeft()
    {
        if (isX)
        {
            //currentRot.x -= 45f;
            Rotate(45f, -Vector3.right);
        }
        else if (isY)
        {
            //currentRot.y -= 45f;
            Rotate(45f, -Vector3.up);
        }
        else
        {
            //currentRot.z -= 45f;
            Rotate(45f, -Vector3.forward);
        }
        //currentRot.y -= 45f;
        //transform.DORotate(new Vector3(currentRot.x, currentRot.y, currentRot.z), duration).SetEase(easeType);
    }

    public void Rotate(float angle, Vector3 axis)
    {
        // Calculate the new target rotation using Quaternion multiplication
        Quaternion targetRotation = transform.rotation * Quaternion.AngleAxis(angle, axis);

        // Animate to the new rotation
        transform.DORotateQuaternion(targetRotation, duration).SetEase(easeType);
    }

    public void MoveAnimation()
    {
        Sequence sequence = DOTween.Sequence();
       var currentTransform =  PCComponentManager.Instance.GetPosition(partsName);
        sequence.Append(transform.DOMove(new Vector3(currentTransform.position.x, currentTransform.position.y, currentTransform.position.z), 1f).SetEase(easeType));
        //sequence.OnComplete(() => { UIManager.Instance.CinematicUI.SetActive(false); });
        Debug.Log("Sequence complete move animation");
    }

    
}
