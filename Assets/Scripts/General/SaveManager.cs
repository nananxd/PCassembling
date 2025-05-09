using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Overall
{
    none,
    demonstrated,
    not_demonstrated
}

[System.Serializable]
public class TotatlAttempts
{
    public string mode;
    public int totalAttempts;
}

[System.Serializable]
public class ComponentError
{
    public string gameMode;
    public string componentName;
    public int errorCount;

    //public ComponentError(string compName,int count)
    //{
    //    componentName = compName;
    //    errorCount = count;
    //}
}

[System.Serializable]
public class ComponentErrorList
{
    public List<ComponentError> componentErrors;
}

[System.Serializable]
public class SaveData
{
    public string timeStamp;
    public string gameMode;
    public string overall;// demonstrated or not demonstrated value only
    public float timeTake;
    public int mistakesCount; // mistake when placing computer parts incorrectly
    public int correctCounts;// correct answer in quiz
}

[System.Serializable]
public class SaveDataList
{
    public List<SaveData> saveDataList /*= new List<SaveData>()*/;
}

public class SaveManager : MonoBehaviour
{
    public string saveFilePath;
    public string saveErrorListFilePath;

    public SaveDataList listSaveData;
    public ComponentErrorList errorList;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "game_results.json");
        saveErrorListFilePath = Path.Combine(Application.persistentDataPath,"component.json");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveDataResult("Assembling",63f,Random.Range(1,35),3,"not demonstrated");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadDataResult();
        }
    }

  

    public void SaveDataResult(string mode, float timeTaken,int mistake,int correctAns,string overall)
    {
        listSaveData =  LoadDataResult();
        //saveDataList.saveDataList = new List<SaveData>();
        if (listSaveData.saveDataList == null)
        {
            listSaveData.saveDataList = new List<SaveData>();
        }

        SaveData saveData = new SaveData
        {
            gameMode = mode,
            timeTake = timeTaken,
            mistakesCount = mistake,
            correctCounts = correctAns,
            timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            overall = overall
        };

        listSaveData.saveDataList.Add(saveData);

        if (listSaveData.saveDataList.Count > 10)
        {
            listSaveData.saveDataList.RemoveAt(0);
            Debug.Log($"save data exceed 10 removing old data at index:{0},value{listSaveData.saveDataList[0]}");
        }

        string json = JsonUtility.ToJson(listSaveData, true);
        File.WriteAllText(saveFilePath, json);
    }


    public void SaveErrorList(string compName, int count)
    {
        errorList = LoadDataErrorList();

        if (errorList.componentErrors == null)
        {
            errorList.componentErrors = new List<ComponentError>();
        }

        var foundData = errorList.componentErrors.Find(x => x.componentName == compName);

        if (foundData != null)
        {
            foundData.errorCount += count;
        }
        else
        {
            //errorList.componentErrors.Add(new ComponentError(compName, count));
            ComponentError errorSave = new ComponentError
            {
                gameMode = SceneLoaderManager.Instance.currentAssesmentType.ToString(),
                componentName = compName,
                errorCount = count
            };

            errorList.componentErrors.Add(errorSave);
        }

        

        //if (errorList.componentErrors.Count > 10)
        //{
        //    errorList.componentErrors.RemoveAt(0);
        //}

        string json = JsonUtility.ToJson(errorList,true);
        File.WriteAllText(saveErrorListFilePath,json);

        Debug.Log($"save component error {json}");
    }

    public ComponentErrorList LoadDataErrorList()
    {
        if (File.Exists(saveErrorListFilePath))
        {
            string json = File.ReadAllText(saveErrorListFilePath);
            var loadedData = JsonUtility.FromJson<ComponentErrorList>(json);

            if (loadedData == null || loadedData.componentErrors == null)
            {
                loadedData = new ComponentErrorList();
                loadedData.componentErrors = new List<ComponentError>();
            }

            return loadedData;
        }

        return new ComponentErrorList { componentErrors = new List<ComponentError>() };
    }

    public SaveDataList LoadDataResult()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            Debug.Log($"success load data:{json}");
            var loadedData = JsonUtility.FromJson<SaveDataList>(json);

            if (loadedData == null || loadedData.saveDataList == null)
            {
                loadedData = new SaveDataList();
                loadedData.saveDataList = new List<SaveData>();
            }
            Debug.Log("loadedData");
            return loadedData;
        }
        Debug.Log("new saveDataList");
        return new SaveDataList { saveDataList = new List<SaveData>() };


    }

    public void SaveErrorComponent()
    {

    }

    public string OverallValue(int mistakeCount , float timeTaken)
    {
        string overall = "";
        if (mistakeCount <= 0 && timeTaken < 600f)
        {
            overall = "Demonstrated";
            //overall = "Competent"
        }
        else
        {
            overall = "Not Demonstrated";
            //overall = "Not Competent"
        }

        return overall;
    }
}
