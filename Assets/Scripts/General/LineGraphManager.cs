using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
//using XCharts.Example;
//using XCharts;
using System;

public class LineGraphManager : MonoBehaviour
{
    public LineChart quizLineChart, mistakeLineChart, timeLineChart;
    [Header("Quiz")]
    public int minQuiz,maxQuiz,intervalQuiz;
    [Header("Mistake")][Tooltip("mistake is the count the user place pc part incorrectly ")]
    public int minMistake, maxMistake, intervalMistake;
    [Header("Time")]
    public int minTime, maxTime, intervalTime;
    [SerializeField] private SaveDataList loadedData;
    // Start is called before the first frame update
    void Start()
    {
        Setup();
        SetupQuizData();
        SetupMistakeData();
        SetupTimeData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Setup()
    {
        var saveMan = TestUIManager.Instance.SaveMan;
        loadedData = saveMan.LoadDataResult();
        // quiz
        var xAxisQuiz = quizLineChart.EnsureChartComponent<XAxis>();
        //quizLineChart.series[0].showDataName = true;
        //quizLineChart.series[0].showDataName = true;
        //quizLineChart.series[0].serieName = "Assemble";
        xAxisQuiz.type = Axis.AxisType.Category;
        xAxisQuiz.splitNumber = 10;
        xAxisQuiz.boundaryGap = true;

        var yAxisQuiuz = quizLineChart.EnsureChartComponent<YAxis>();
        yAxisQuiuz.minMaxType = Axis.AxisMinMaxType.Custom;
        yAxisQuiuz.min = minQuiz;
        yAxisQuiuz.max = maxQuiz;
        yAxisQuiuz.interval = intervalQuiz;
        yAxisQuiuz.type = Axis.AxisType.Value;

        // mistake
        var xAxisMistake = mistakeLineChart.EnsureChartComponent<XAxis>();
        xAxisMistake.type = Axis.AxisType.Category;
        xAxisMistake.splitNumber = 10;
        xAxisMistake.boundaryGap = true;

        var yAxisMistake = mistakeLineChart.EnsureChartComponent<YAxis>();
        yAxisMistake.minMaxType = Axis.AxisMinMaxType.Custom;
        yAxisMistake.type = Axis.AxisType.Value;
        yAxisMistake.min = minMistake;
        yAxisMistake.max = maxMistake;
        yAxisMistake.interval = intervalMistake;

        //time

        var xAxisTimer = timeLineChart.EnsureChartComponent<XAxis>();
        //xAxisTimer.axisLabel.numericFormatter = "date:MMM dd HH:mm";
        xAxisTimer.type = Axis.AxisType.Category;
        xAxisTimer.splitNumber = 10;
        xAxisTimer.boundaryGap = true;



        var yAxisTimer = timeLineChart.EnsureChartComponent<YAxis>();
        yAxisTimer.axisLabel.numericFormatter = "time:mm\\:ss";
        //yAxisTimer.axisLabel.numericFormatter = "{0}:{1:00}";

        yAxisTimer.type = Axis.AxisType.Value;
        yAxisTimer.minMaxType = Axis.AxisMinMaxType.Custom;
        yAxisTimer.min = minTime;
        yAxisTimer.max = maxTime;
        yAxisTimer.interval = intervalTime;
    }

    

    public void SetupQuizData()
    {
        //var saveMan = TestUIManager.Instance.SaveMan;
        //var loadedData = saveMan.LoadDataResult();

        for (int i = 0; i < loadedData.saveDataList.Count; i++)
        {
            var quizData = loadedData.saveDataList[i];
            DateTime time = DateTime.Parse(quizData.timeStamp);
            // string formattedTime = time.ToString("MM/dd HH:mm");
            string formattedTime = time.ToString("MMM dd HH:mm");
            
            quizLineChart.AddXAxisData(formattedTime +$"<color=#7CB1FF>\n\n{quizData.gameMode.ToUpper()}</color>");
            quizLineChart.AddData(0, quizData.correctCounts);
        }
    }

    public void SetupMistakeData()
    {
        //var saveMan = TestUIManager.Instance.SaveMan;
        //var loadedData = saveMan.LoadDataResult();

        for (int i = 0; i < loadedData.saveDataList.Count; i++)
        {
            var mistakeData = loadedData.saveDataList[i];
            DateTime time = DateTime.Parse(mistakeData.timeStamp);
            // string formattedTime = time.ToString("MM/dd HH:mm");
            string formattedTime = time.ToString("MMM dd HH:mm");
            mistakeLineChart.AddXAxisData(formattedTime +$"<color=#7CB1FF>\n\n{mistakeData.gameMode.ToUpper()}</color>");
            mistakeLineChart.AddData(0, mistakeData.mistakesCount);
        }
    }

    public void SetupTimeData()
    {
        //var saveMan = TestUIManager.Instance.SaveMan;
        //var loadedData = saveMan.LoadDataResult();

        for (int i = 0; i < loadedData.saveDataList.Count; i++)
        {
            var timeData = loadedData.saveDataList[i];
            DateTime time = DateTime.Parse(timeData.timeStamp);
           // string formattedTime = time.ToString("MM/dd HH:mm");
            string formattedTime = time.ToString("MMM dd HH:mm");
            timeLineChart.AddXAxisData(formattedTime + $"<color=#7CB1FF>\n\n{timeData.gameMode.ToUpper()}</color>");
            timeLineChart.AddData(0, timeData.timeTake);
        }
    }
}
