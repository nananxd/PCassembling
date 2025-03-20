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
    public SaveDataList listSaveData;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "game_results.json");
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

    public string OverallValue(int mistakeCount)
    {
        string overall = "";
        if (mistakeCount <= 0)
        {
            overall = "Demonstrated";

        }
        else
        {
            overall = "Not Demonstrated";
        }

        return overall;
    }
}
