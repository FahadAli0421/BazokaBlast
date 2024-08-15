using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QAudioManager
{
    public class Q_UIAudioManager : MonoBehaviour
    {
        public Sprite musicOffSprite, soundOffSprite, soundOnSprite, musicOnSprite;
        public Image soundImage, musicImage;
        public Slider musicSlider, VFXSlider;
        public List<string> vFXList;
        public float volumeMusic = 0.5f, volumeVFX = 0.5f;
        public TextMeshProUGUI musicText, vFXText;
 
        private void Start()
        {
            // Automatically plays the music at the start of the game
            if (PlayerPrefs.GetInt("music") == 1)
            {
                FindObjectOfType<AudioManager>().Play("music");
            }
            statusToggle();

            // Initialize volume from PlayerPrefs or set default
            if (PlayerPrefs.HasKey("VolumeMusic"))
            {
                volumeMusic = PlayerPrefs.GetFloat("VolumeMusic");
            }
            else
            {
                PlayerPrefs.SetFloat("VolumeMusic", volumeMusic);
            }

            if (PlayerPrefs.HasKey("VolumeVFX"))
            {
                volumeVFX = PlayerPrefs.GetFloat("VolumeVFX");
            }
            else
            {
                PlayerPrefs.SetFloat("VolumeVFX", volumeVFX);
            }

            // Update AudioManager with slider values
            musicSlider.value = volumeMusic; // Set the slider to the saved volume
            VFXSlider.value = volumeVFX; // Set the VFX slider to the saved volume

            musicText.text = Mathf.Round(musicSlider.value * 10).ToString();
            vFXText.text = Mathf.Round(VFXSlider.value * 10).ToString();

            AudioManager.instance.Volume("music", musicSlider.value);

            for (int i = 0; i < vFXList.Count; i++)
            {
                AudioManager.instance.Volume(vFXList[i], VFXSlider.value);
            }

            // Add listeners to sliders to update volume in real-time
            musicSlider.onValueChanged.AddListener(delegate { UpdateMusicVolume(); });
            VFXSlider.onValueChanged.AddListener(delegate { UpdateVFXVolume(); });
        }

        public void toggleMusic()
        {
            if (PlayerPrefs.GetInt("music") == 0)
            {
                PlayerPrefs.SetInt("music", 1);
                FindObjectOfType<AudioManager>().Play("music");
            }
            else if (PlayerPrefs.GetInt("music") == 1)
            {
                PlayerPrefs.SetInt("music", 0);
                FindObjectOfType<AudioManager>().Stop("music");
            }
            clickSound();
            statusToggle();
        }

        public void toggleSound()
        {
            if (PlayerPrefs.GetInt("sound") == 0)
            {
                PlayerPrefs.SetInt("sound", 1);
            }
            else if (PlayerPrefs.GetInt("sound") == 1)
            {
                PlayerPrefs.SetInt("sound", 0);
            }
            clickSound();
            statusToggle();
        }

        public void clickSound()
        {
            FindObjectOfType<AudioManager>().Play("click");
        }

        public void statusToggle()
        {
            if (PlayerPrefs.GetInt("sound") == 0)
            {
                soundImage.sprite = soundOffSprite;
            }
            else if (PlayerPrefs.GetInt("sound") == 1)
            {
                soundImage.sprite = soundOnSprite;
            }
            if (PlayerPrefs.GetInt("music") == 0)
            {
                musicImage.sprite = musicOffSprite;
            }
            else if (PlayerPrefs.GetInt("music") == 1)
            {
                musicImage.sprite = musicOnSprite;
            }
        }

        public void UpdateMusicVolume()
        {
            AudioManager.instance.Volume("music", musicSlider.value);
            PlayerPrefs.SetFloat("VolumeMusic", musicSlider.value); // Save the current music volume
            musicText.text = Mathf.Round(musicSlider.value * 10).ToString();
            
        }

        public void UpdateVFXVolume()
        {
            for (int i = 0; i < vFXList.Count; i++)
            {
                AudioManager.instance.Volume(vFXList[i], VFXSlider.value);
            }
            PlayerPrefs.SetFloat("VolumeVFX", VFXSlider.value); // Save the current VFX volume
            vFXText.text = Mathf.Round(VFXSlider.value * 10).ToString();
        }
    }
}
