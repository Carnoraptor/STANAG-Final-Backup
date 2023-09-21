using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    //Msc for music
    public EventReference currentlyPlayingMusic;
    public EventReference CurrentMusic
    {
        get
        {
            return currentlyPlayingMusic;
        }
        set
        {
            AudioManager.instance.StopMusic();
            currentlyPlayingMusic = CurrentMusic;
            AudioManager.instance.InitializeMusic(currentlyPlayingMusic);
        }
    }


    public EventReference forgottenStationMsc;

    public void ChangeMusic(EventReference newMusic)
    {
        CurrentMusic = newMusic;
    }


    //Singleton shit
    public static MusicManager instance {get; private set;}
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}