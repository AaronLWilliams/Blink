using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;

    List<Resolution> uniqueResolutions = new List<Resolution>();

    private void Start()
    {
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
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = uniqueResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void setWindowed (bool IsWindowed)
    {
        Screen.fullScreen = !IsWindowed;
    }

    public void clearSaveData ()
    {
        SaveSystem.ClearSaveData();
    }
}
