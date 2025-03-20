using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Tutorial", menuName = "ScriptableObjects/TutorialData", order = 1)]

public class TutorialSO : ScriptableObject
{
   

    public List<Tutorial> tutorials;
}
