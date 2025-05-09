using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestTutorial : MonoBehaviour
{
    public List<TestTutorialUI> testTut;
    public TestTutorialUI selectedTutUI;
    public RectTransform inventoryViewport;
    public string currentSelectedItemInventory;
    public string givenName;
    public ScrollRect scrollRect;

    public Transform uiToTransform;
    public Vector3 offset;
    public string saveErrorListFilePath;
    private void Awake()
    {
        saveErrorListFilePath = Path.Combine(Application.persistentDataPath, "component.json");
    }

    // Start is called before the first frame update
    void Start()
    {
        //scrollRect.onValueChanged.AddListener(IsVisible);
        foreach (var item in testTut)
        {
            item.testButton.onClick.AddListener(()=> OnClickUI(item.transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TestSave()
    {
        //string json = JsonUtility.ToJson(listSaveData, true);
        //File.WriteAllText(saveFilePath, json);
    }
    public void OnClickUI(Transform buttonTransform)
    {
        uiToTransform.position = buttonTransform.position + offset;
    }

    public void IsVisible(Vector2 pos)
    {
        //var targetItem = InventoryManager.Instance.GetInventoryItemByName(currentSelectedItemInventory);
        selectedTutUI = testTut.Find(s => s.inventName == givenName);
        //var inventoryViewport = UIManager.Instance.inventoryViewport;

        if (RectTransformUtility.RectangleContainsScreenPoint(inventoryViewport, selectedTutUI.transform.position, null))
        {
            //TutorialManager.Instance.CompleteTutorialSteps();
            Debug.Log("item in viewport");
        }
        else
        {

        }
    }
}
