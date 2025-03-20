using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour
{
    [SerializeField] private Ease easeType;
    [SerializeField] private float duration;
    [SerializeField] private Transform buttonTransform;
    [SerializeField] private Image buttonBackground;
    [SerializeField] private Vector3 buttonBGoriginalScale;
    [SerializeField] private Button currentButton;
    [SerializeField] private SelectedButtonTracker buttonTracker;

    private Color buttonOrigColor;
    // Start is called before the first frame update
    void Start()
    {
        buttonTracker = GetComponentInParent<SelectedButtonTracker>();
        currentButton = GetComponent<Button>();
        buttonBGoriginalScale = buttonBackground.transform.localScale;
        buttonBackground.transform.localScale = new Vector3(0,buttonBackground.transform.localScale.y);
        currentButton.onClick.AddListener(OnClickButtonAnimate);
        buttonOrigColor = currentButton.image.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClickButtonAnimate()
    {
        buttonTracker.CurrentSelectedButton = this;
        buttonTracker.ResetOtherButtons();
    }

    public void OnClickAnimate()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DOScale(new Vector3(1.1f,1.1f,1.1f),duration).SetEase(easeType));
        sequence.Join(buttonBackground.transform.DOScaleX(buttonBGoriginalScale.x,duration).SetEase(easeType));
        sequence.Join(currentButton.image.DOColor(buttonBackground.color,duration).SetEase(easeType));
        //sequence.Join(transform.DOScale(Vector3.one, duration).SetEase(easeType));
    }

    public void ResetButton()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DOScale(Vector3.one, duration).SetEase(easeType));
        sequence.Join(buttonBackground.transform.DOScaleX(0, duration).SetEase(easeType));
        sequence.Join(currentButton.image.DOColor(buttonOrigColor,duration).SetEase(easeType));
    }
}
