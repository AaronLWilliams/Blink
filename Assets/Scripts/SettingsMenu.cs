using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public TMP_Dropdown resolutionDropdown;
    public Toggle windowedToggle;

    List<Resolution> uniqueResolutions = new List<Resolution>();

    private void Start()
    {
        LoadSettings();

        // Load saved resolution settings
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        bool isWindowed = PlayerPrefs.GetInt("IsWindowed", 1) == 1;

        Resolution[] resolutions = Screen.resolutions;
        HashSet<string> uniqueResolutionStrings = new HashSet<string>();

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        foreach (Resolution resolution in resolutions)
        {
            string option = resolution.width + " x " + resolution.height;

            if (uniqueResolutionStrings.Add(option))
            {
                uniqueResolutions.Add(resolution);
                options.Add(option);

                if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = uniqueResolutions.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        // Set listeners for the UI elements
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        windowedToggle.onValueChanged.AddListener(SetWindowed);
    }

    private void LoadSettings()
    {
        // Load audio settings
        masterVolumeSlider.value = PlayerPrefs.GetFloat("Master", 0.75f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("Music", 0.75f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffects", 0.75f);

        // Set initial values to the audio mixer
        SetMasterVolume(masterVolumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);

        // Load resolution settings
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        bool isWindowed = PlayerPrefs.GetInt("IsWindowed", 1) == 1;

        windowedToggle.isOn = isWindowed;
        Screen.fullScreen = !isWindowed;
        if (uniqueResolutions.Count > 0 && savedResolutionIndex < uniqueResolutions.Count)
        {
            Resolution resolution = uniqueResolutions[savedResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, !isWindowed);
        }

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = uniqueResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetWindowed (bool IsWindowed)
    {
        Screen.fullScreen = !IsWindowed;
        PlayerPrefs.SetInt("IsWindowed", IsWindowed ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void clearSaveData ()
    {
        SaveSystem.ClearSaveData();
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Master", volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Music", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SoundEffects", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SoundEffects", volume);
        PlayerPrefs.Save();
    }
}
