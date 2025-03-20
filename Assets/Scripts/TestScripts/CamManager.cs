using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CamManager : MonoBehaviour
{
    public static CamManager Instance;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCams;
    [SerializeField] private CinemachineVirtualCamera normalCam, wideAngleCam,zoomInCam;
    [SerializeField] private CinemachineBrain brainCamera;

    private void Awake()
    {
        Instance = this;
    }


    public void AnimateVirtualCam()
    {
        var uiMan = Disassemble.UIManager.Instance;
        var manager = DisassembleGameManager.Instance;
        Sequence sequence = DOTween.Sequence();
        normalCam.Priority = 10;
        wideAngleCam.Priority = 11;
        uiMan.ChoicesCanvasGroup.interactable = false;
        manager.AllDisassembleInteractionEnableOrDisable(false);

        sequence.AppendInterval(5f).AppendCallback(()=> {

            normalCam.Priority = 11;
            wideAngleCam.Priority = 10;
            uiMan.ChoicesCanvasGroup.interactable = true;
            manager.AllDisassembleInteractionEnableOrDisable(true);
            Debug.Log("Cam Manager called");
        });
     
    }

    public void AnimateZoomInCam()
    {
        var uiMan = Disassemble.UIManager.Instance;
        var manager = DisassembleGameManager.Instance;
        Sequence sequence = DOTween.Sequence();
        normalCam.Priority = 10;
        zoomInCam.Priority = 11;
        uiMan.ChoicesCanvasGroup.interactable = false;
        manager.AllDisassembleInteractionEnableOrDisable(false);

        sequence.AppendInterval(5f).AppendCallback(() => {

            normalCam.Priority = 11;
            zoomInCam.Priority = 10;
            uiMan.ChoicesCanvasGroup.interactable = true;
            manager.AllDisassembleInteractionEnableOrDisable(true);
            Debug.Log("Cam Manager called");
        });
    }

}
