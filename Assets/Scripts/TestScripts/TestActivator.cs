using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActivator : MonoBehaviour
{
    [SerializeField] private List<GameObject> parts;
    [SerializeField] private int index;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Activatepart();
        }
    }
    public void Activatepart()
    {
        if (parts.Count == 0 || parts == null)
        {
            Debug.Log($"List:{parts} is empty");
            return;
        }

        if (index < 0 || index >= parts.Count)
        {
            Debug.Log($"index is out of range");
            return;
        }
        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].SetActive(false);
            Debug.Log("deactivating the parts");
        }
        parts[index].SetActive(true);

        var interactableObj = parts[index].gameObject.GetComponent<InteractableDeactivator>();
        if (interactableObj != null)
        {
            interactableObj.DeactivateObject();
        }


        Debug.Log("activating the parts");
        index++;
    }
}
