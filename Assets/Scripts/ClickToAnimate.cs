using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ClickToAnimate : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] Ease easeType;
    [SerializeField] private float duration;
    [SerializeField] private Transform ramClipA1,ramClipA2,ramClipB1,ramClipB2;
       
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnClickRamClip();
        }
    }

    public void OnClickRamClip()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(ramClipA1.DOLocalRotate(new Vector3(0,0,0), duration,RotateMode.FastBeyond360).SetEase(easeType));
        sequence.Join(ramClipA2.DOLocalRotate(new Vector3(0, 0, 0), duration, RotateMode.FastBeyond360).SetEase(easeType));
        sequence.Join(ramClipB1.DOLocalRotate(new Vector3(0, 0, 0), duration, RotateMode.FastBeyond360).SetEase(easeType));
        sequence.Join(ramClipB2.DOLocalRotate(new Vector3(0, 0, 0), duration, RotateMode.FastBeyond360).SetEase(easeType));

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
