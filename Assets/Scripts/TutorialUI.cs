using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private string tutorialUid;

    public string TutorialUid { get => tutorialUid; set => tutorialUid = value; }
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private Button nextOrStartBtn;

    private void Start()
    {
        //Setup();
    }

    public void Setup()
    {
        tutorialText = GetComponentInChildren<TextMeshProUGUI>(true);
        if (nextOrStartBtn != null)
        {
            //nextOrStartBtn = GetComponentInChildren<Button>(true);
            nextOrStartBtn.onClick.AddListener(StartOrNextTutorialTrigger);
        }
    }

    public void SetTutorialText(string display)
    {
        tutorialText.text = display;
    }

    public void StartOrNextTutorialTrigger()
    {
        TutorialManager.Instance.CompleteTutorialSteps();
    }
}
