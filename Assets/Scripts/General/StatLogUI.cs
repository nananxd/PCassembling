using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatLogUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI modeTxt, timeTakeTxt, correctTxt, mistakeTxt,dateTxt,overAllTxt;
    
    public void SetUI(string mode,float timeTake,int correct,int mistake,string date,string overall)
    {
        modeTxt.text = mode;
        timeTakeTxt.text = FormatSecondsToMinutesSeconds(timeTake);
        correctTxt.text = $"{correct}/10";
        mistakeTxt.text = mistake.ToString();
        dateTxt.text = date;
        overAllTxt.text = overall;
    }

    string FormatSecondsToMinutesSeconds(float value)
    {
        int totalSeconds = Mathf.FloorToInt(value);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }
}
