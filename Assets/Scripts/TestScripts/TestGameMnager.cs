using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GroupComponent
{

}
public class TestGameMnager : MonoBehaviour
{
    [SerializeField] private RectTransform uiParent;
    [SerializeField] private GameObject uiPrefab;
    [SerializeField] private List <GameObject> partsToDisable;
    [SerializeField] private List<TestParentInteractable> parentsInteractable;
    //[SerializeField] private SkinnedMeshRenderer skinMesh;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void TestDisableObject()
    {

    }

    public void TestSpawnUI(List<string> content)
    {
        for (int i = 0; i < content.Count; i++)
        {
            GameObject go = Instantiate(uiPrefab,uiPrefab.transform);
        }
    }
}
