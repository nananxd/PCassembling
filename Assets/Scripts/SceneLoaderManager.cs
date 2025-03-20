using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager Instance;
    public GameType currentGameType;
    public AssesmentType currentAssesmentType;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void LoadLevel(string sceneName,bool isAdditive = false)
    {
        if (isAdditive)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
       
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevelAsAsync(string sceneName,GameObject loadingScreen = null)
    {
        StartCoroutine(LoadAsynCoroutine(sceneName,loadingScreen));
    }

    private IEnumerator LoadAsynCoroutine(string sceneName,GameObject loadingScreen = null)
    {
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            //if (loadingScreen != null)
            //{
            //    loadingScreen.SetActive(true);
            //}
            LoadingManager.Instance.ActivateLoading(true);

            if (operation.progress >= 0.9f)
            {
                Debug.Log("LoadAsync");
                //LoadingManager.Instance.ActivateLoading(false);
                yield return new WaitForSeconds(1f);
                
                operation.allowSceneActivation = true;

                //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            }
        }

        yield return null;
    }
}
