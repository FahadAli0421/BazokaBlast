using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace QAudioManager
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        public Sound[] sounds;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                foreach (Sound s in sounds)
                {
                    s.source = gameObject.AddComponent<AudioSource>();
                    s.source.clip = s.clip;
                    s.source.volume = s.volume;
                    s.source.pitch = s.pitch;
                    s.source.loop = s.loop;
                    s.source.priority = 1;
                }
                if (PlayerPrefs.GetInt("first_Q") == 0)
                {
                    PlayerPrefs.SetInt("music", 1);
                    PlayerPrefs.SetInt("sound", 1);
                    PlayerPrefs.SetInt("first_Q", 1);
                }
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log("Sound : " + name + "not found");
                return;
            }
            if (name != "music" && PlayerPrefs.GetInt("sound") == 1)
                s.source.Play();
            if (name == "music" && PlayerPrefs.GetInt("music") == 1)
                s.source.Play();
        }
        public void Pause(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log("Sound : " + name + "not found");
                return;
            }
            s.source.Pause();
        }
        public void UnPause(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log("Sound : " + name + "not found");
                return;
            }
            s.source.UnPause();
        }
        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log("Sound : " + name + "not found");
                return;
            }
            s.source.Stop();
        }
        public void Volume(string name, float volume)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log("Sound : " + name + "not found");
                return;
            }

            s.source.volume = volume;


        }
        public void Pitch(string name, float pitch)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.Log("Sound : " + name + "not found");
                return;
            }

            s.source.pitch = pitch;


        }
    }
}

