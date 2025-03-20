using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControlManager : MonoBehaviour
{
    public static CameraControlManager Instance;
    [SerializeField] private CinemachineVirtualCamera mainCam,zoomInCam;

    private void Awake()
    {
        Instance = this;
    }

    public void ZoomIn()
    {
        mainCam.Priority = 10;
        zoomInCam.Priority = 11;
    }


    public void ZoomOut()
    {
        mainCam.Priority = 11;
        zoomInCam.Priority = 10;
    }
}
