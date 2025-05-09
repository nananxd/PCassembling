using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
   
    [Header("Animation settings")]
    [SerializeField] private float duration,radius;
    [SerializeField] private Ease easeType;
    [SerializeField] private Transform cameraToRotate, objectTorotateFrom;

    public const string ASSEMBLY_ATTEMPT = "assemble";
    public const string DISASSEMBLY_ATTEMPT = "disassemble";

    private void Awake()
    {

//#if UNITY_EDITOR
//        Debug.unityLogger.logEnabled = true;
//#endif

//#if UNITY_ANDROID
//        Debug.unityLogger.logEnabled = false;
//#endif

        DOTween.SetTweensCapacity(3000, 200);
    }
    private void Start()
    {
        //RotateAround();
        SceneLoaderManager.Instance.LoadLevel("LoadingScene", true);
        SoundManager.Instance.PlayBGM("BG");
       
    }

    private void LateUpdate()
    {
       
    }
    public void OnAssemble()
    {
        SoundManager.Instance.LowerVolune("BG");
        var sceneLoader =  SceneLoaderManager.Instance;
        sceneLoader.currentAssesmentType = AssesmentType.assemble;
        //PlayerPrefs.SetString(ConstantData.ASESSMENT_KEY,asessmentType.ToString());
        //SceneLoaderManager.Instance.LoadLevel("AsessmentScene");
        sceneLoader.SaveAttempt(ASSEMBLY_ATTEMPT, 1);
        sceneLoader.LoadLevelAsAsync("AsessmentScene",TestUIManager.Instance.LoadingScreen);
    }

    public void OnDisassemble()
    {
        SoundManager.Instance.LowerVolune("BG");
        var sceneLoader = SceneLoaderManager.Instance;
        sceneLoader.currentAssesmentType = AssesmentType.disassemble;
        sceneLoader.SaveAttempt(DISASSEMBLY_ATTEMPT,1);
        //PlayerPrefs.SetString(ConstantData.ASESSMENT_KEY, asessmentType.ToString());
        sceneLoader.LoadLevelAsAsync("Disassemble", TestUIManager.Instance.LoadingScreen);
        
       // SceneLoaderManager.Instance.LoadLevel("AsessmentScene");
    }

    

    public void RotateAround()
    {
        DOTween.To(() => 0f, angle =>
        {
            float x = objectTorotateFrom.position.x + Mathf.Cos(angle) * radius;
            float z = objectTorotateFrom.position.z + Mathf.Sin(angle) * radius;
            cameraToRotate.position = new Vector3( x, cameraToRotate.position.y, z);
            cameraToRotate.LookAt(objectTorotateFrom); // Keep looking at the target
        }, Mathf.PI * .5f, duration)
        .SetEase(easeType)
        .SetLoops(-1, LoopType.Incremental);
    }
}

public static class ConstantData
{
    public const string ASESSMENT_KEY = "asessmentKey";
}
