using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DisassembleFrontPanelAnimator : MonoBehaviour
{
    [SerializeField] private List<Transform> frontPanelPlugs;
    [SerializeField] private Transform location;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            AnimateFrontPanel();
        }
    }
    public void AnimateFrontPanel()
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < frontPanelPlugs.Count; i++)
        {
            var currentPlug = frontPanelPlugs[i];
            sequence.Append(currentPlug.DOMove(location.position, .5f).SetEase(Ease.OutSine));
            sequence.Join(currentPlug.DORotate(new Vector3(0,-45f,-45f),.5f).SetEase(Ease.OutSine));
        }
    }

    
}
