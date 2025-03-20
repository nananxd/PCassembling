using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;
    [SerializeField] private Ease easeType;
    [SerializeField] private float duration;
    [SerializeField] private CanvasGroup loadingCanvasGroup;



    private void Awake()
    {
        Instance = this;   
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject); // Prevents destruction on scene change
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }
    void Start()
    {
        
    }

    

    public void ActivateLoading()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        //sequence.AppendCallback(()=>EnableLoadingObject(true));
        sequence.Append(loadingCanvasGroup.DOFade(1f, duration).SetEase(easeType));
        loadingCanvasGroup.blocksRaycasts = true;
        loadingCanvasGroup.interactable = true;
        sequence.AppendInterval(1f);
        sequence.Append(loadingCanvasGroup.DOFade(0, duration).SetEase(easeType));
        loadingCanvasGroup.blocksRaycasts = false;
        loadingCanvasGroup.interactable = false;
        sequence.AppendInterval(.5f);
    }

    public void ActivateLoading(bool isShown)
    {
        Debug.Log("Loading manager is called");
        Sequence sequence = DOTween.Sequence();
        if (isShown)
        {
            //sequence.AppendInterval(1f);
            sequence.Append(loadingCanvasGroup.DOFade(1f, duration).SetEase(easeType));
            loadingCanvasGroup.blocksRaycasts = true;
            loadingCanvasGroup.interactable = true;
        }
        else
        {
            sequence.AppendInterval(1f);
            sequence.Append(loadingCanvasGroup.DOFade(0, duration).SetEase(easeType));
            loadingCanvasGroup.blocksRaycasts = false;
            loadingCanvasGroup.interactable = false;
            sequence.AppendInterval(.5f);
        }
    }
}
