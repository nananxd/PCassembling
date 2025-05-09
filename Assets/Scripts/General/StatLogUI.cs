using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatLogUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI modeTxt, timeTakeTxt, correctTxt, mistakeTxt,dateTxt,overAllTxt;
    [SerializeField] private Color competentColor,nonCompetentColor;
    [SerializeField] private Image backgroundImage;
    
    public void SetUI(string mode,float timeTake,int correct,int mistake,string date,string overall)
    {
        modeTxt.text = mode;
        timeTakeTxt.text = FormatSecondsToMinutesSeconds(timeTake);
        correctTxt.text = $"{correct}/10";
        mistakeTxt.text = mistake.ToString();
        dateTxt.text = date;
        overAllTxt.text = SetText(overall);

        SetUIColor(overall);
    }

    string FormatSecondsToMinutesSeconds(float value)
    {
        int totalSeconds = Mathf.FloorToInt(value);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    private void SetUIColor(string overall)
    {
        if (overall == "Demonstrated")
        {
            backgroundImage.color = competentColor;
            //modeTxt.color = Color.green;
            //timeTakeTxt.color = Color.green;
            //correctTxt.color = Color.green;
            //mistakeTxt.color = Color.green;
            //dateTxt.color = Color.green;
            //overAllTxt.color = Color.green;

        }
        else
        {
            backgroundImage.color = nonCompetentColor;
            //modeTxt.color = Color.red;
            //timeTakeTxt.color = Color.red;
            //correctTxt.color = Color.red;
            //mistakeTxt.color = Color.red;
            //dateTxt.color = Color.red;
            //overAllTxt.color = Color.red;
        }
    }

    private string SetText(string overall)
    {
        if (overall == "Demonstrated")
        {
            return "Competent";
        }

        return "Not Competent";
    }
}
