using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenuCamera : MonoBehaviour
{
    public static MainMenuCamera Instance;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCams;
    [SerializeField] private CinemachineVirtualCamera mainCam,foundCam;

    private void Awake()
    {
        Instance = this;
    }

    public void SwitchCam(string cameraName)
    {
        foundCam = virtualCams.Find(x=>x.name == cameraName);
        if (foundCam != null)
        {
            mainCam.Priority = 10;
            foundCam.Priority = 11;
        }
        //switch (cameraName)
        //{
        //    case "assesment":
        //        break;
        //    case "tutorial":
        //        break;
        //    case "practice":
        //        break;
        //    case "explore":
        //        break;
        //    case "history":
        //        break;

           
        //}
    }

    public void SwitchToMainCam()
    {
        mainCam.Priority = 11;
        foundCam.Priority = 10;
    }
}
