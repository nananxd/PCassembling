using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTriggerManager : MonoBehaviour
{
    public static LocationTriggerManager Instance;
    [SerializeField] private List<LocationTrigger> locationTriggers;
    [SerializeField] private LocationTrigger currentLocationTrigger;

    public LocationTrigger CurrentLocationTrigger { get => currentLocationTrigger;  set => currentLocationTrigger = value;  }

    private void Awake()
    {
        Instance = this;
    }
    //public LocationTrigger GetLocationTriggerById(string idName)
    //{
    //    var currentLocation = locationTriggers.Find(lt => lt.TriggerName == idName);
    //    if (!currentLocation)
    //    {
    //        Debug.Log("No location trigger found");
    //        return null;
    //    }
    //    return currentLocation;
    //}
}
