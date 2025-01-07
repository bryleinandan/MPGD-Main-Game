using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsMenuManager : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;
    public Slider volume;
    public AudioMixer mainAudioMixer;

    public void ChangeGraphicsQuality() //Change graphics quality via a dropdown menu.
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }

    public void ChangeVolume() //Change game sound volume with a slider.
    {
        mainAudioMixer.SetFloat("MasterVolume", volume.value);
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

}
