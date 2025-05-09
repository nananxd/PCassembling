using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;


[System.Serializable]
public class Sound
{
    public string soundName;
    [Range(0,1f)]
    public float volume;
    [Range(0, 1f)]
    public float pitch;
    public bool playOnAwake;
    public bool isLoop;
    public AudioSource source;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private List<Sound> sounds;
    private void Awake()
    {
        Instance = this;
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        foreach (var item in sounds)
        {
            if (item.soundName == "BG")
            {
                item.volume = .3f;
            }
            else
            {
                item.volume = 1f;
            }
          
            item.pitch = 1f;
            item.source = gameObject.AddComponent<AudioSource>();
            item.source.clip = item.clip;
            item.source.volume = item.volume;
            item.source.pitch = item.pitch;
            item.source.playOnAwake = item.playOnAwake;
            item.source.loop = item.isLoop;

        }
    }

    private void Start()
    {
       
    }

    
    public void PlaySfx(string sfxName)
    {
        //var foundSound = sounds.Find(s => s.soundName == sfxName);
        for (int i = 0; i < sounds.Count; i++)
        {
            if (sounds[i].soundName == sfxName)
            {
                sounds[i].source.DOFade(1f,.2f).SetEase(Ease.Linear);
                sounds[i].source.Play();
            }
            else
            {
               
                if (sounds[i].soundName != "BG")
                {
                    sounds[i].source.DOFade(0, .5f).SetEase(Ease.Linear);
                }
               
            }
        }
        //if (foundSound != null)
        //{
        //    foundSound.source.DOFade(1f,.3f).SetEase(Ease.Linear);
        //    foundSound.source.Play();
        //}
        
    }

    public void PlayBGM(string sfxName)
    {
        var foundSound = sounds.Find(s => s.soundName == sfxName);

        if (foundSound != null)
        {
            foundSound.source.DOFade(1f, .3f).SetEase(Ease.Linear);
            foundSound.source.Play();
        }
    }

    public void LowerVolune(string sfxName)
    {
        var foundSound = sounds.Find(s => s.soundName == sfxName);
        if (foundSound != null)
        {
            foundSound.source.DOFade(0, 1f).SetEase(Ease.Linear);
            //foundSound.source.Play();
        }
    }
}
