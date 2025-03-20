using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentPartActivator : MonoBehaviour
{
    [SerializeField] private List<GameObject> parts;
    [SerializeField] private int index;
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

    private IEnumerator DelayActivate(int index)
    {
        yield return new WaitForSeconds(.5f);
        parts[index].gameObject.SetActive(true);
    }
}
