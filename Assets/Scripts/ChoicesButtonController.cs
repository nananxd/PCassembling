using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicesButtonController : MonoBehaviour
{
    [SerializeField] private List<ChoicesBtnUI> choicesUI;
    public int currentSelecteedChoiceId;
    
    public void HideChoices()
    {
        for (int i = 0; i < choicesUI.Count; i++)
        {
            if (choicesUI[i].ChoiceID == currentSelecteedChoiceId)
            {
                choicesUI[i].ShowOrHideUI(true);
            }
            else
            {
                choicesUI[i].ShowOrHideUI(false);
            }
        }
    }

    public void ShowAllChoices()
    {
        for (int i = 0; i < choicesUI.Count; i++)
        {
            choicesUI[i].ShowOrHideUI(true);
        }
    }
}
