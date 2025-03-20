using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Disassemble
{
    public class UIManager : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private Ease easeType;
        [SerializeField] private float duration;

        [Header("Feedback UI")]
        [Space(1f)]
        [SerializeField] private Image wrongFeedbackUI;

        [Header("Others UI")]
        [Space(1f)]
        public static Disassemble.UIManager Instance;
        [SerializeField] private GameObject uiPrefab;
        [SerializeField] private RectTransform uiParent;

        [Header("Choices UI")]
        [Space(1f)]
        [SerializeField] private CanvasGroup choicesCanvasGroup;

        [Header("Statistic ui")]
        [Space(1f)]
        [SerializeField] private Vector3 statShowPos, statHidePos;
        [SerializeField] private TextMeshProUGUI timeText, correctAnswerText, mistakeText;
        [SerializeField] private RectTransform statRect, timeRect, correctAnsRect,mistakeRect;
        [SerializeField] private CanvasGroup statisticCanvasGroup;
        [SerializeField] private Button statCloseButton;

        [Header("Notification ui")]
        [Space(1f)]
        [SerializeField] private Vector3 notifShowPos, notifHidePos;
        [SerializeField] private CanvasGroup notifCanvasGroup;
        [SerializeField] private RectTransform notificationRect;
        [SerializeField] private TextMeshProUGUI notificationTxt;

        [Header("Quiz or Trivia UI")]
        [Space(1f)]
        [SerializeField] private Vector3 scaleFactor;
        [SerializeField] private Transform triviaTransform;
        [SerializeField] private CanvasGroup triviaCanvasGroup;
        [SerializeField] private QuizButton choiceA, choiceB, choiceC, choiceD;
        [SerializeField] private TextMeshProUGUI quizNameTxt;

        [Header("Timer")]
        [Space(1f)]
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private CanvasGroup timerCanvasGroup;
        [SerializeField] private RectTransform timerRect, timerIconRect;
        [SerializeField] private Vector3 timerShowPos, timerHidePos;
        [SerializeField] private bool isTimerShown;


        [Header("Menu UI")]
        [SerializeField] private CanvasGroup menuCanvasGroup;
        [SerializeField] private RectTransform menuUI;
        [SerializeField] private Vector3 hidePosition, showPosition;
        [SerializeField] private Button menuRestartBtn, menuMainMenuBtn;
        [SerializeField] private List<CanvasGroup> menuBtnCanvasGroup;


        [Header("End Screen for practice and tutorial")]
        [SerializeField] private TextMeshProUGUI endScreenTxt;
        [SerializeField] private CanvasGroup endScreenCanvas;
        [SerializeField] private RectTransform endScreenRect;
        [SerializeField] private Vector3 endScreenHidePos, endScreenShowPos;
        [SerializeField] private Button mainMenuBtn, restartBtn;

        [Header("Tutorial")]
        [SerializeField] TextMeshProUGUI tutorialTxt;

        public CanvasGroup ChoicesCanvasGroup { get => choicesCanvasGroup; set => choicesCanvasGroup = value; }

        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            SetupButtonOnclick();
        }

        private void LateUpdate()
        {
            timerText.text = StatisticsManager.Instance.FormattedTime();
            SetTutorialUI();
        }
        void Update()
        {
            if (StatisticsManager.Instance.CountDown <= 0 && StatisticsManager.Instance.CanTimerStart)
            {
                isTimerShown = true;
                ToggleTimer();
            }
        }

       



        #region Tutorial
        public void SetTutorialUI()
        {
            if (SceneLoaderManager.Instance.currentGameType == GameType.tutorial)
            {
                var tutorial = /*TutorialManager.Instance.CurrentTutorial*/ DisassembleTutorialManager.Instance.CurrentTutorial;
                if (tutorial.tutorialName != "")
                {
                    tutorialTxt.text = tutorial.tutorialName;
                }
            }
                

        }
        #endregion

        public void WrongFeedback()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(wrongFeedbackUI.DOFade(.3f, .1f));
            seq.Append(wrongFeedbackUI.DOFade(0, .1f).SetDelay(.1f));

        }

        public IEnumerator DisableInteractableCoroutine()
        {
            ChoicesCanvasGroup.interactable = false;
            yield return new WaitForSeconds(2f);
            ChoicesCanvasGroup.interactable = true;
        }

        public void SpawnUI(DisassembleInteractionParent selectedParent)
        {
            //if (DisassembleGameManager.Instance.CurrentSelectedInteraction != null)
            //{
            //    DisassembleGameManager.Instance.CurrentSelectedInteraction = null;
            //}
            HideOrShowCanvasGroup(ChoicesCanvasGroup, true);
            if (uiParent.childCount > 0)
            {
                foreach (Transform child in uiParent.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            DisassembleGameManager.Instance.CurrentSelectedGroupParent.GroupsUI.Clear();

            //if (DisassembleGameManager.Instance.CurrentSelectedGroupParent.GroupsUI.Count > 0)
            //{
            //    foreach (var item in DisassembleGameManager.Instance.CurrentSelectedGroupParent.GroupsUI)
            //    {
            //        Destroy(item.gameObject);
            //    }

            //    DisassembleGameManager.Instance.CurrentSelectedGroupParent.GroupsUI.Clear();
            //}


            for (int i = 0; i < selectedParent.InteractionGroups.Count; i++)
            {
                var uiGo = Instantiate(uiPrefab, uiParent.transform);
                var ui = uiGo.GetComponent<PartUI>();

                ui.SetOutlineObjectReference(selectedParent.InteractionGroups[i].DisAssembleOutlineObject);

                ui.SetUI(
                    selectedParent.InteractionGroups[i].partsName,
                    selectedParent.InteractionGroups[i].componentGroupType.ToString(),
                    selectedParent.InteractionGroups[i].IsPowerSupplyCable,
                    selectedParent.InteractionGroups[i].IsScrew,
                    selectedParent.InteractionGroups[i].isFrontPanel);
                uiGo.SetActive(true);
                selectedParent.GroupsUI.Add(ui);
            }

            //SetIndicatorPosition();
        }

        public void OnCloseComponentButton()
        {
            HideOrShowCanvasGroup(ChoicesCanvasGroup,false);
            //if (DisassembleGameManager.Instance.CurrentSelectedInteraction != null)
            //{
            //    DisassembleGameManager.Instance.CurrentSelectedInteraction = null;
            //}

            if (uiParent.childCount > 0)
            {
                foreach (Transform child in uiParent.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            //if (DisassembleGameManager.Instance.CurrentSelectedGroupParent.GroupsUI.Count > 0)
            //{
            //    foreach (var item in DisassembleGameManager.Instance.CurrentSelectedGroupParent.GroupsUI)
            //    {
            //        // Destroy(item.gameObject);
            //        item.gameObject.SetActive(false);
            //    }

            //   // DisassembleGameManager.Instance.CurrentSelectedGroupParent.GroupsUI.Clear();
            //}
            
           
        }
        #region Notification
        public void SetNotificationInfoUI(string info)
        {
            notificationTxt.text = info;
        }

        public void ShowNotification()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1f);
            sequence.Join(notificationRect.DOAnchorPos(notifShowPos, duration).SetEase(easeType));
            sequence.Join(notifCanvasGroup.DOFade(1f, duration).SetEase(easeType));
            sequence.AppendInterval(2f);
            sequence.Append(notificationRect.DOAnchorPos(notifHidePos, duration).SetEase(easeType));
            sequence.Join(notifCanvasGroup.DOFade(0, duration).SetEase(easeType));
        }
        #endregion

        #region Quiz
        public void SetQuizUI(string quizTitle)
        {
            quizNameTxt.text = quizTitle;
            choiceA.SetUI();
            choiceB.SetUI();
            choiceC.SetUI();
            choiceD.SetUI();
        }

        public void ShowQuiz()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Join(triviaTransform.DOScale(scaleFactor, duration).SetEase(easeType));
            sequence.Join(triviaCanvasGroup.DOFade(1f, duration).SetEase(easeType));
            SetChoicesButtonToNormal();
            triviaCanvasGroup.blocksRaycasts = true;
            triviaCanvasGroup.interactable = true;

        }

        public void HideQuiz()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Join(triviaTransform.DOScale(0, duration).SetEase(easeType));
            sequence.Join(triviaCanvasGroup.DOFade(0, duration).SetEase(easeType));

            triviaCanvasGroup.blocksRaycasts = false;
            triviaCanvasGroup.interactable = false;
        }

        public IEnumerator HideQuizCoroutine()
        {
            yield return new WaitForSeconds(2f);
            HideQuiz();
        }

        private void SetChoicesButtonToNormal()
        {
            Debug.Log("Setting button choice to normal");
            choiceA.SetToNormal();
            choiceB.SetToNormal();
            choiceC.SetToNormal();
            choiceD.SetToNormal();
        }

        public void SetQuizChoiceButton(string a, string b, string c, string d)
        {
            choiceA.SetChoice(a);
            choiceB.SetChoice(b);
            choiceC.SetChoice(c);
            choiceD.SetChoice(d);
        }

       

        public void SetQuizAnswer(string a, string b, string c, string d)
        {
            choiceA.SetAnswer(a);
            choiceB.SetAnswer(b);
            choiceC.SetAnswer(c);
            choiceD.SetAnswer(d);
        }
        #endregion

        #region Statistic ui
        public IEnumerator SetStatisticCoroutine()
        {
            yield return new WaitForSeconds(2.5f);
            Debug.Log("Statistic Coroutine Started");
            if (SceneLoaderManager.Instance.currentGameType == GameType.asessment)
            {
                SetStatisticInfoUI();
                ShowStatisticUI();
            }
            else
            {
                ShowEndScreen();
            }
            
        }
        public void SetStatisticInfoUI()
        {
            timeText.text = StatisticsManager.Instance.FormattedTime();
            correctAnswerText.text = StatisticsManager.Instance.correctAnswersCounter.ToString();
            mistakeText.text = StatisticsManager.Instance.mistakeCounter.ToString();
        }

        public void ShowStatisticUI()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(statRect.DOAnchorPos(statShowPos, duration).SetEase(easeType));
            sequence.Append(statisticCanvasGroup.DOFade(1f, duration).SetEase(easeType));

            sequence.Append(timeRect.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), duration).SetEase(easeType));
            sequence.Append(timeRect.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));

            sequence.Append(correctAnsRect.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), duration).SetEase(easeType));
            sequence.Append(correctAnsRect.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));

            sequence.Append(mistakeRect.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), duration).SetEase(easeType));
            sequence.Append(mistakeRect.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));

            statisticCanvasGroup.interactable = true;
            statisticCanvasGroup.blocksRaycasts = true;

        }

        public void HideStatisticUI()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(statRect.DOAnchorPos(statHidePos, duration).SetEase(easeType));
            sequence.Join(statisticCanvasGroup.DOFade(0, duration).SetEase(easeType));

            sequence.Join(timeRect.transform.DOScale(new Vector3(0, 0, 0), duration).SetEase(easeType));
            sequence.Join(correctAnsRect.transform.DOScale(new Vector3(0, 0, 0), duration).SetEase(easeType));
            sequence.Join(mistakeRect.transform.DOScale(new Vector3(0, 0, 0), duration).SetEase(easeType));

            statisticCanvasGroup.interactable = false;
            statisticCanvasGroup.blocksRaycasts = false;
        }
        #endregion

        #region Timer
        public void ToggleTimer()
        {
           // isTimerShown = !isTimerShown;
            if (isTimerShown)
            {
                ShowTimer();
            }
            else
            {
                HideTimer();
            }
        }

        public void ShowTimer()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(timerRect.transform.DOScale(new Vector3(.9f, .9f, .9f), duration).SetEase(easeType));
            sequence.Join(timerRect.DOAnchorPos(timerShowPos, duration).SetEase(easeType));
            sequence.Join(timerCanvasGroup.DOFade(1f, duration).SetEase(easeType));
            sequence.Append(timerRect.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));
            sequence.Append(timerIconRect.DORotate(new Vector3(0, 0, 180f), duration).SetEase(easeType));
        }

        public void HideTimer()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Join(timerCanvasGroup.DOFade(0, duration).SetEase(easeType));
            sequence.Join(timerRect.transform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(easeType));
            sequence.Join(timerRect.DOAnchorPos(timerHidePos, duration).SetEase(easeType));
            sequence.Append(timerIconRect.DORotate(new Vector3(0, 0, 0), duration).SetEase(easeType));

        }
        #endregion
        public void SetComponentGroupComplete()
        {

        }

        private void HideOrShowCanvasGroup(CanvasGroup canvas,bool isShown)
        {
            Sequence sequence = DOTween.Sequence();
            if (isShown)
            {
                canvas.DOFade(1f,duration).SetEase(easeType);
                canvas.interactable = true;
                canvas.blocksRaycasts = true;
            }
            else
            {
                canvas.DOFade(0, duration).SetEase(easeType);
                canvas.interactable = false;
                canvas.blocksRaycasts = false;
            }
        }

        public void SetIndicatorPosition()
        {
            var currentSelected = DisassembleGameManager.Instance.CurrentSelectedInteraction;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(currentSelected.transform.position);
            if (screenPos.z > 0)
            {
                screenPos.z = 0;
                //indicatorOBject.GetComponent<RectTransform>().anchoredPosition = screenPos;
                ChoicesCanvasGroup.transform.position = screenPos;
            }
        }

        public void OnClickMenu()
        {
            Sequence sequence = DOTween.Sequence();
            Ease temEase = Ease.OutExpo;
            float temDuration = .1f;
            sequence.Append(menuUI.DOScaleX(1f, temDuration).SetEase(temEase));
            sequence.Join(menuUI.DOScaleY(.003f, temDuration).SetEase(temEase));

            sequence.AppendInterval(.1f);
            //sequence.Append(menuUI.DOAnchorPos(showPosition, duration).SetEase(easeType));
            sequence.Join(menuCanvasGroup.DOFade(1f, .1f).SetEase(Ease.InSine));
            sequence.Append(menuUI.DOScaleY(1f, temDuration).SetEase(temEase));

            foreach (var item in menuBtnCanvasGroup)
            {
                sequence.Append(item.DOFade(1f, .1f).SetEase(Ease.InSine));
                sequence.Join(item.transform.DOScale(Vector3.one, .1f).SetEase(Ease.InSine));
            }


            menuCanvasGroup.blocksRaycasts = true;
            menuCanvasGroup.interactable = true;
        }

       


        public void OnClickResume()
        {
            Sequence sequence = DOTween.Sequence();
            Ease temEase = Ease.OutExpo;
            float temDuration = .1f;
            foreach (var item in menuBtnCanvasGroup)
            {
                sequence.Join(item.DOFade(0, .1f).SetEase(temEase));
                sequence.Join(item.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), .1f).SetEase(temEase));
            }
            sequence.Append(menuUI.DOScaleY(.003f, temDuration).SetEase(temEase));

            sequence.AppendInterval(.05f);

            sequence.Append(menuUI.DOScaleX(0, temDuration).SetEase(temEase));
            sequence.Join(menuUI.DOScaleY(0, temDuration).SetEase(temEase));
            sequence.Join(menuCanvasGroup.DOFade(0, .3f).SetEase(Ease.OutSine));


            menuCanvasGroup.blocksRaycasts = false;
            menuCanvasGroup.interactable = false;
        }

        private void SetupButtonOnclick()
        {
            var sceneLoader = SceneLoaderManager.Instance;

            restartBtn.onClick.AddListener(sceneLoader.RestartLevel);
            mainMenuBtn.onClick.AddListener(() => sceneLoader.LoadLevelAsAsync("MainMenu"));

            menuRestartBtn.onClick.AddListener(sceneLoader.RestartLevel);
            menuMainMenuBtn.onClick.AddListener(() => sceneLoader.LoadLevelAsAsync("MainMenu"));

            statCloseButton.onClick.AddListener(() => sceneLoader.LoadLevelAsAsync("MainMenu"));
            Debug.Log("Setup button complete");
        }

        public void SetEndScreenText()
        {
            if (SceneLoaderManager.Instance == null)
            {
                return;
            }
            switch (SceneLoaderManager.Instance.currentGameType)
            {
                case GameType.none:
                    break;
                case GameType.tutorial:
                    endScreenTxt.text = $"TUTORIAL FINISHED!";
                    break;
                case GameType.practice:
                    endScreenTxt.text = $"PRACTICE FINISHED!";
                    break;
                case GameType.asessment:
                    break;

            }
        }
        public void ShowEndScreen()//for practice and tutorial only
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(endScreenCanvas.DOFade(1f, .5f).SetEase(easeType));

            sequence.Join(endScreenRect.DOAnchorPos(endScreenShowPos, duration).SetEase(easeType));


            endScreenCanvas.blocksRaycasts = true;
            endScreenCanvas.interactable = true;
            SetEndScreenText();
        }

        public void HideEndScreen()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(endScreenCanvas.DOFade(0, 2f).SetEase(easeType));

            sequence.Join(endScreenRect.DOAnchorPos(endScreenHidePos, duration).SetEase(easeType));


            endScreenCanvas.blocksRaycasts = false;
            endScreenCanvas.interactable = false;
        }

    }
}

