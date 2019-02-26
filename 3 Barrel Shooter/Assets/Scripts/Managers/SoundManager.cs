using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public Dictionary<string,AudioClip> sounds;
    public AudioSource audioSource;


    public void InitSoundManager()
    {
        sounds = new Dictionary<string, AudioClip>();
        LoadSounds();
        //audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void StartBGMusic(AudioSource a)
    {
        audioSource = a;
        PlaySoundByName(a, "DrumsFury");
    }

    public void StopBGMusic()
    {
        StopSound(audioSource);
    }

    public void PauseBGMusic()
    {
        audioSource.Pause();
    }

    public void SetVolume(AudioSource asource, float n) // between 0.0-1.0 
    {
        asource.volume = n;
    }

    private void LoadSounds()
    {
        TextAsset txt = (TextAsset)Resources.Load("Sounds/loadSounds", typeof(TextAsset));
        string[] lines = Regex.Split(txt.text, "\n|\r|\r\n");

        foreach (string line in lines)
        {
            sounds[line] = (Resources.Load<AudioClip>("Sounds/" + line));
           
        }
    }

    public void PlaySoundByName(AudioSource s, string name, bool loop = false, float volume = 1.0f, float pitch = 1.0f)
    {
        if (s != null && !s.isPlaying)
        {
            s.clip = sounds[name];
            //s.loop = true;
            s.pitch = Random.Range(0.5f, 1.5f);
            s.volume = volume;
            s.Play();

            s.loop = loop;

        }
    }


    public void StopSound(AudioSource asource)
    {
        if (asource != null)
        asource.Stop();
    }

}
