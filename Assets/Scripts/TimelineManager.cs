using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager  Instance;
    [SerializeField] private PlayableDirector director;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayCinematic();
        }
    }
    public void PlayCinematic()
    {
        UIManager.Instance.CinematicUI.SetActive(true);
        director.Play();
        
    }
}
