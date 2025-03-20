using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ScrewsAnimator : MonoBehaviour
{
    [SerializeField] private Ease easeType;
    [SerializeField] private float duration;

    [SerializeField] private List<Transform> transformToAnimate;
    [SerializeField] private List<Transform> transformLocation;

    private List<Vector3> originalPositions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StorePosition()
    {
        for (int i = 0; i < transformToAnimate.Count; i++)
        {
            var currentPos = transformToAnimate[i];
            originalPositions.Add(currentPos.position);
        }
    }

    public void ResetPosition()
    {
        for (int i = 0; i < transformToAnimate.Count; i++)
        {
            var currentTransform = transformToAnimate[i];
            currentTransform.position = originalPositions[i];
        }
    }

    private void AnimateToLocation()
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < transformToAnimate.Count; i++)
        {
            var transformToMove = transformToAnimate[i];
            var transformLoc = transformLocation[i];

            sequence.Append(transformToMove.DOMove(transformLoc.position, duration).SetEase(easeType));
            
        }
    }

    private void OnEnable()
    {
        StorePosition();
        AnimateToLocation();
    }

    private void OnDisable()
    {
        ResetPosition();
    }
}
