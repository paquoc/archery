using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class OptionMenu : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;

    public ResItem[] resolutions;

    private int selectedResolution;

    public Text resolutionLabel;

    public AudioMixer theMixer;

    public Slider mastSlider, musicSlider, sfxSlider;

    public Text mastLabel, musicLabel, sfxLabel;

    public AudioSource sfxLoop;

    // Start is called before the first frame update
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        bool foundRes = false;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (Screen.width == resolutions[i].horizontal)
            {
                foundRes = true;
                selectedResolution = i;
                break;
            }
        }

        if (foundRes)
        {
            Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
            UpdateResLabel();
        }
        else
        {
            resolutionLabel.text = Screen.width.ToString() + " x " + Screen.height.ToString();
        }

        SetMixerAndSliderWithKey(SaveLoadSystem.KeyMasterVol, mastSlider);
        SetMixerAndSliderWithKey(SaveLoadSystem.KeyMusicVol, musicSlider);
        SetMixerAndSliderWithKey(SaveLoadSystem.KeySFXVol, sfxSlider);

        mastLabel.text = (mastSlider.value + 100).ToString();
        musicLabel.text = (musicSlider.value + 100).ToString();
        sfxLabel.text = (sfxSlider.value + 100).ToString();

    }

    private void SetMixerAndSliderWithKey(string key, Slider slider)
    {
        float valueFromDisk = SaveLoadSystem.GetFloat(key, -1);
        if (valueFromDisk < 0)
        {
            valueFromDisk = 1f;
            SaveLoadSystem.SetFloat(key, valueFromDisk);
        }
        theMixer.SetFloat(key, valueFromDisk);
        slider.value = valueFromDisk;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution >= resolutions.Length)
        {
            selectedResolution = resolutions.Length - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullscreenTog.isOn;

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }

    public void SetMasterVol()
    {
        mastLabel.text = (mastSlider.value + 100).ToString();
        theMixer.SetFloat("MasterVol", mastSlider.value);
        SaveLoadSystem.SetFloat(SaveLoadSystem.KeyMasterVol, mastSlider.value);
    }

    public void SetMusicVol()
    {
        musicLabel.text = (musicSlider.value + 100).ToString();

        theMixer.SetFloat("MusicVol", musicSlider.value);

        SaveLoadSystem.SetFloat(SaveLoadSystem.KeyMusicVol, musicSlider.value);
    }

    public void SetSFXVol()
    {
        sfxLabel.text = (sfxSlider.value + 100).ToString();

        theMixer.SetFloat("SFXVol", sfxSlider.value);

        SaveLoadSystem.SetFloat(SaveLoadSystem.KeySFXVol, sfxSlider.value);
    }

    public void PlaySFXLoop()
    {
        sfxLoop.Play();
    }

    public void StopSFXLoop()
    {
        sfxLoop.Stop();
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
