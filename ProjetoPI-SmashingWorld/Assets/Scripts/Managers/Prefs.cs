using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs : MonoBehaviour
{
    public const string SAVE_FULLSCREEN = "Save_FullScreen";
    public const string SAVE_RESOLUTION = "Save_Resolution";
    public const string SAVE_MUSICVOLUME = "Save_MusicVolume";
    public const string SAVE_SFXVOLUME = "Save_SFXVolume";


    public static bool LoadFullScreen()
    {
        if(PlayerPrefs.HasKey(SAVE_FULLSCREEN))
        {
            return PlayerPrefs.GetInt(SAVE_FULLSCREEN) == 1;
        }
        else
        {
            SaveFullScreen();
            return Screen.fullScreen;
        }
    }

    public static void SaveFullScreen()
    {

        PlayerPrefs.SetInt(SAVE_FULLSCREEN, Screen.fullScreen ? 1 : 0);
    }

    public static int LoadScreenResolution()
    {
        if (PlayerPrefs.HasKey(SAVE_RESOLUTION))
        {
            return PlayerPrefs.GetInt(SAVE_RESOLUTION);
        }
        else
        {
            SaveScreenResolution(1);
            return 1;
        }
    }

    public static void SaveScreenResolution(int type)
    {

        PlayerPrefs.SetInt(SAVE_RESOLUTION, type);
    }

    public static float LoadMusicVolume()
    {
        if (PlayerPrefs.HasKey(SAVE_MUSICVOLUME))
        {
            return PlayerPrefs.GetFloat(SAVE_MUSICVOLUME);
        }
        else
        {
            SaveMusicVolume(0.5f);
            return 0.5f;
        }
    }

    public static void SaveMusicVolume(float volume)
    {

        PlayerPrefs.SetFloat(SAVE_MUSICVOLUME, volume);
    }

    public static float LoadSFXVolume()
    {
        if (PlayerPrefs.HasKey(SAVE_SFXVOLUME))
        {
            return PlayerPrefs.GetFloat(SAVE_SFXVOLUME);
        }
        else
        {
            SaveSFXVolume(0.5f);
            return 0.5f;
        }
    }

    public static void SaveSFXVolume(float volume)
    {

        PlayerPrefs.SetFloat(SAVE_SFXVOLUME, volume);
    }




}
