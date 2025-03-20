using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Quiz", menuName = "ScriptableObjects/QuizData", order = 1)]

public class QuizSO : ScriptableObject
{
    public List<Quiz> quizes;
}

[System.Serializable]
public class Quiz
{
    [TextArea(4,2)]
    public string quizName;
    [Space(3)]
    [TextArea(2, 2)]
    public string correctAnswer;

    [Header("Choices")]
    public string a,b,c,d;
}
