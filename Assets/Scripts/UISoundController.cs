using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundController : MonoBehaviour
{

    public static UISoundController instance = null;
    public AudioClip Swipe;
    public AudioClip Destroy;
    public AudioClip Win;
    public static AudioSource audioSource;
    public static AudioSource audioSourceBg;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSource = audioSources[0];
       // audioSourceBg = audioSources[0];
        //audioSourceBg.Play();
        //audioSourceBg.pitch = 0.65f;
        //buttonClip = Resources.Load<AudioClip>("click_v1");
        /* buttonClip = Resources.Load<AudioClip> ("pallete");
        swapClip = Resources.Load<AudioClip> ("swap button2");
        settingsClip = Resources.Load<AudioClip> ("setting button sludo new"); */
        // StartCoroutine (AudioFadeController.FadeIn (audioSources[0], 0.5f));
    }


    public void SwipeSound()
    {
        if (PlayerPrefs.HasKey("SfxPref"))
        {
            if (PlayerPrefs.GetString("SfxPref").Equals("Off"))
            {
                return;
            }
        }
        audioSource.PlayOneShot(Swipe);
    }

    public void DestroySound()
    {
        if (PlayerPrefs.HasKey("SfxPref"))
        {
            if (PlayerPrefs.GetString("SfxPref").Equals("Off"))
            {
                return;
            }
        }
        audioSource.PlayOneShot(Destroy);
    }

    public void WinSound()
    {
        if (PlayerPrefs.HasKey("SfxPref"))
        {
            if (PlayerPrefs.GetString("SfxPref").Equals("Off"))
            {
                return;
            }
        }
        audioSource.PlayOneShot(Win);
    }
}