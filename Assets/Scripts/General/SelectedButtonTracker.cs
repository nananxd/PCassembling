using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedButtonTracker : MonoBehaviour
{
    [SerializeField] private List<ButtonAnimator> buttons;
    [SerializeField] private ButtonAnimator currentSelectedButton;
    

    public ButtonAnimator CurrentSelectedButton { get => currentSelectedButton; set => currentSelectedButton = value; }


    public void SelectCurrentButton()
    {
        currentSelectedButton.OnClickAnimate();
    }
    
    public void ResetOtherButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (currentSelectedButton == buttons[i])
            {
                buttons[i].OnClickAnimate();
            }
            else
            {
                buttons[i].ResetButton();
            }
        }
    }

    public void ResetAll()
    {
        foreach (var item in buttons)
        {
            item.ResetButton();
        }
    }
}
