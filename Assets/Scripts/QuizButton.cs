using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Disassemble;

public class QuizButton : MonoBehaviour
{

    [Header("Animation settings")]
    [SerializeField] private Ease easeType;
    [SerializeField] private float duration;

    public string selectedChoice;
    public string selectedChoiceAnswer;

  

    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private bool isDisassemble;
    [SerializeField] private Image btnImage;
    [SerializeField] private Color correctColor, wrongColor, normalColor;
    private Vector3 originalScale;

    public Button CurrentQuizButton { get => button; set => button = value; }

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectAnswer);
        nameText = button.GetComponentInChildren<TextMeshProUGUI>();
        originalScale =transform.localScale;
    }


    public void SelectAnswer()
    {
        QuizManager.Instance.CurrentChoice = selectedChoice;
        QuizManager.Instance.CurrentChoiceAnswer = selectedChoiceAnswer;
        QuizManager.Instance.DisableQuizChoices(this);

        if (isDisassemble)
        {
            //Disassemble.UIManager.Instance.HideQuiz();
            StartCoroutine(Disassemble.UIManager.Instance.HideQuizCoroutine());
        }
        else
        {
            //UIManager.Instance.HideQuiz();
            StartCoroutine(UIManager.Instance.HideQuizCoroutine());
        }
       
        QuizManager.Instance.CheckAnswer();

        if (QuizManager.Instance.IsAnswerCorrect)
        {
            CorrectAnimation();
        }
        else
        {
            WrongAnimation();
        }

        QuizManager.Instance.isOpen = false;
        QuizManager.Instance.CanStartQuiz = true;
    }

    public void SetUI()
    {
        nameText.text = $"{selectedChoice}.) {selectedChoiceAnswer}";
    }

    public void SetChoice(string choice)
    {
        selectedChoice = choice;
      
    }

    public void SetAnswer(string answer)
    {
        selectedChoiceAnswer = answer;
    }

    public void SetToNormal()
    {
        btnImage.color = normalColor;
    }

    public void WrongAnimation()
    {
      
        btnImage.color = wrongColor;
        ShakeAnimation(.6f);
    }

    public void CorrectAnimation()
    {
       
        btnImage.color = correctColor;
        ShakeAnimation(.1f);
    }

    private void ShakeAnimation(float givenDuration)
    {
        
        Sequence sequence = DOTween.Sequence();
        transform.DOPunchScale(originalScale * .2f, givenDuration).SetEase(easeType);
       

    }

    
}
