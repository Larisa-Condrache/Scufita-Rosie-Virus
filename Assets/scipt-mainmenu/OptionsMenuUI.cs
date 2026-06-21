using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;

    [Header("Volume")]
    public Slider volumeSlider;

    private float lastVolume = 1f;

    void Start()
    {
        volumeSlider.value = AudioListener.volume;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;

        if (volume > 0f)
            lastVolume = volume;
    }

    public void ToggleMute()
    {
        if (AudioListener.volume > 0f)
        {
            lastVolume = AudioListener.volume;
            AudioListener.volume = 0f;
            volumeSlider.value = 0f;
        }
        else
        {
            AudioListener.volume = lastVolume;
            volumeSlider.value = lastVolume;
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        if (isFullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.fullScreen = false;
        }
    }

    public void ReturnToMainMenu()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}