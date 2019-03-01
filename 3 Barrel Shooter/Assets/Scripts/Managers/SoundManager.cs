using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SoundManager
{

    public Dictionary<string,AudioClip> sounds;
    public AudioSource audioSource;
    float pitchIncrement;

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
    public void PitchIncrement()
    {
        if (pitchIncrement >= 1.6f)
        {
            pitchIncrement = .5f;
        }
        pitchIncrement += 0.2f;
    }
    public void PlaySoundByName(AudioSource s, string name, bool loop = false, float volume = 0.5f, float pitch = 0.0f)
    {
        AudioClip c;
        if (s != null && !s.isPlaying)
        {

            //s.loop = true;
            //if (pitch == 1.0f) // changes pitch by ascending upward
            //{
            //    s.pitch = 1.0f;
            //}
            //else
            //{
            //    s.pitch = pitchIncrement;
            //    PitchIncrement();
            //}
            //c = sounds[name];
            s.clip = sounds[name];
            
            if (pitch != 1.0f) // randomizes pitch
                s.pitch = Random.Range(0.8f, 1.4f);
            else
                s.pitch = pitch;
            s.volume = volume;

            //s.PlayOneShot(c);
            s.Play();

            s.loop = loop;

        }
        else if (s.isPlaying && name != "Grasswalk" && name != "LeafPetals")
        {
            c = sounds[name];
            if (pitch != 1.0f) // randomizes pitch
                s.pitch = Random.Range(0.7f, 1.4f);
            else
                s.pitch = pitch;
            s.volume = volume;
            s.PlayOneShot(c);
        }

    }


    public void StopSound(AudioSource asource)
    {
        if (asource != null)
        asource.Stop();
    }

}
