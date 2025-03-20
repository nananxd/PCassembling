using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;
    public QuizSO currentQuiz;
    [SerializeField] private Quiz selectedQuiz;
    [SerializeField] private string currentChoice;
    [SerializeField] private string currentChoiceAnswer;
    [SerializeField] private float currentWaitTime , waitTime;
    [SerializeField] private float closeTime = 2f, closeTimer;
    [SerializeField] private bool canStartQuiz,isAnswerCorrect;
    [SerializeField] private List<QuizButton> quizButtons;
    public bool isOpen;
    [SerializeField] private int maxQuiz = 10;
    [SerializeField] private int currentQuizCount;

    public string CurrentChoice { get => currentChoice; set => currentChoice = value; }
    public string CurrentChoiceAnswer { get => currentChoiceAnswer; set => currentChoiceAnswer = value; }
    public float CurrentWaiTime { get => currentWaitTime; set => currentWaitTime = value; }
    public float CloseTime { get => closeTime; set => closeTime = value; }
    public bool CanStartQuiz { get => canStartQuiz; set => canStartQuiz = value; }
    public float WaitTime { get => waitTime; set => waitTime = value; }
    public bool IsAnswerCorrect { get => isAnswerCorrect; set => isAnswerCorrect = value; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        //waitTime += Time.deltaTime;
        //if (waitTime >= currentWaitTime && !isInventoryOpen)
        //{
        //    SetQuiz();
        //    isInventoryOpen = true;
        //    waitTime = 0;

        //}


        if (Input.GetKeyDown(KeyCode.B))
        {
            SetQuiz();
        }
    }
    public void DisableQuizChoices(QuizButton selectedQuizButton)
    {
        foreach (var item in quizButtons)
        {
            if (item == selectedQuizButton)
            {
                item.enabled = true;
                item.CurrentQuizButton.enabled = true;
            }
            else
            {
                item.enabled = false;
                item.CurrentQuizButton.enabled = false;
            }
        }
    }



    public void EnableAllQuizChoices()
    {
        foreach (var item in quizButtons)
        {
            item.enabled = true;
            item.CurrentQuizButton.enabled = true;
        }
    }

    public void SetQuiz()
    {
        if (currentQuizCount < maxQuiz)
        {
            var selectedIndex = Random.Range(0, currentQuiz.quizes.Count);
            selectedQuiz = currentQuiz.quizes[selectedIndex];
            UIManager.Instance.SetQuizChoiceButton
                (
                nameof(selectedQuiz.a),
                nameof(selectedQuiz.b),
                nameof(selectedQuiz.c),
                nameof(selectedQuiz.d)
                );

            UIManager.Instance.SetQuizAnswer
                (
                    selectedQuiz.a,
                    selectedQuiz.b,
                    selectedQuiz.c,
                    selectedQuiz.d
                );

            UIManager.Instance.SetQuizUI(selectedQuiz.quizName);
            UIManager.Instance.ShowQuiz();
            EnableAllQuizChoices();
            canStartQuiz = false;
            currentQuizCount++;
        }
        
    }

    public void SetQuiz(Disassemble.UIManager uiManager)// for disassemble scene
    {

        if (currentQuizCount < maxQuiz)
        {
            var selectedIndex = Random.Range(0, currentQuiz.quizes.Count);
            selectedQuiz = currentQuiz.quizes[selectedIndex];
            uiManager.SetQuizChoiceButton
                (
                nameof(selectedQuiz.a),
                nameof(selectedQuiz.b),
                nameof(selectedQuiz.c),
                nameof(selectedQuiz.d)
                );

            uiManager.SetQuizAnswer
                (
                    selectedQuiz.a,
                    selectedQuiz.b,
                    selectedQuiz.c,
                    selectedQuiz.d
                );

            uiManager.SetQuizUI(selectedQuiz.quizName);
            uiManager.ShowQuiz();
            EnableAllQuizChoices();
            canStartQuiz = false;
            currentQuizCount++;
        }

      
    }

    public void CheckAnswer()
    {
        if (selectedQuiz.correctAnswer.ToLower() == currentChoiceAnswer.ToLower())
        {
            Debug.Log("Correct Answer");
            isAnswerCorrect = true;
            StatisticsManager.Instance.CountCorrectAnswer();
        }else
        {
            isAnswerCorrect = false;
            Debug.Log("Wrong Answer");
        }
    }
}
