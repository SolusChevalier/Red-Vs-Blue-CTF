using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeSettingsMenu : MonoBehaviour
{
    #region FIELDS

    public AudioMixer audioMixer;
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;
    //public Toggle MasterToggle;
    //public Toggle MusicToggle;
    //public Toggle SFXToggle;

    public int PreviousSceneIndex = 1;

    /*[SerializeField]
    [Range(-80f, 20f)]*/
    private float masterVolume = 0f;

    //[SerializeField]
    private float lastMasterVolume = 0f;

    //[SerializeField]
    private bool masterMute = false;

    /*[SerializeField]
    [Range(-80f, 20f)]*/
    private float musicVolume = 0f;

    //[SerializeField]
    private float lastMusicVolume = 0f;

    //[SerializeField]
    private bool musicMute = false;

    /*[SerializeField]
    [Range(-80f, 20f)]*/
    private float sfxVolume = 0f;

    //[SerializeField]
    private float lastSfxVolume = 0f;

    //[SerializeField]
    private bool sfxMute = false;

    #endregion FIELDS

    #region METHODS

    public void SetMusicVolume()
    {
        musicVolume = MusicSlider.value - 30;
        audioMixer.SetFloat("MusicVolume", musicVolume);
        if (musicMute)
            musicMute = false;
    }

    public void SetMasterVolume()
    {
        masterVolume = MasterSlider.value - 30;
        audioMixer.SetFloat("MasterVolume", masterVolume);
        if (masterMute)
            masterMute = false;
    }

    public void SetSFXVolume()
    {
        sfxVolume = SFXSlider.value - 30;
        audioMixer.SetFloat("SFXVolume", sfxVolume);
        if (sfxMute)
            sfxMute = false;
    }

    public void MuteMaster()
    {
        if (!masterMute)
        {
            lastMasterVolume = masterVolume + 30;
            MasterSlider.value = 0;
            masterVolume = -80;
            audioMixer.SetFloat("MasterVolume", masterVolume);
            masterMute = true;
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", -30 + lastMasterVolume);
            MasterSlider.value = lastMasterVolume;
            masterMute = false;
        }
    }

    public void MuteMusic()
    {
        if (!musicMute)
        {
            lastMusicVolume = musicVolume + 30;
            MusicSlider.value = 0;
            musicVolume = -80;
            audioMixer.SetFloat("MusicVolume", musicVolume);
            musicMute = true;
        }
        else
        {
            audioMixer.SetFloat("MusicVolume", -30 + lastMusicVolume);
            MusicSlider.value = lastMusicVolume;
            musicMute = false;
        }
    }

    public void MuteSFX()
    {
        if (!sfxMute)
        {
            lastSfxVolume = sfxVolume + 30;
            SFXSlider.value = 0;
            sfxVolume = -80;
            audioMixer.SetFloat("SFXVolume", sfxVolume);
            sfxMute = true;
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", -30 + lastSfxVolume);
            SFXSlider.value = lastSfxVolume;
            sfxMute = false;
        }
    }

    public void BackBtnClicked()
    {
        SceneManager.LoadScene(PreviousSceneIndex);
    }

    #endregion METHODS
}