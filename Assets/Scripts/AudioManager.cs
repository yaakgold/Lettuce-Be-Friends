using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;

    public List<float> volumes;

    public Slider musicVol, soundVol;

    public TextMeshProUGUI music, sound;

    // Start is called before the first frame update
    private void Start()
    {
        Play("Music");
    }

    void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            
            sound.source.clip = sound.clip;
            sound.source.volume = sound.vol;
            sound.source.loop = sound.loop;
            volumes.Add(sound.vol);
        }
    }

    public void Play(string name)
    {
        Sound s = null;
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
                s = sound;
        }

        if(s != null)
        {
            if(!s.source.isPlaying)
                s.source.Play();
        }
    }

    public void Play(string name, float delay)
    {
        Sound s = null;
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
                s = sound;
        }

        if (s != null)
        {
            if (!s.source.isPlaying)
                s.source.PlayDelayed(delay);
        }
    }

    public void Stop(string name)
    {
        Sound s = null;
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
                s = sound;
        }

        if (s != null)
        {
            if (s.source.isPlaying)
                s.source.Stop();
        }
    }

    public void changeSoundEffects()
    {
        for(int i = 0; i < sounds.Count; i++)
        {
            if(!sounds[i].name.Contains("Music"))
            {
                sounds[i].source.volume = volumes[i] * soundVol.value;
                sound.text = "" + (int)(soundVol.value * 10);
            }
        }
    }

    public void changeMusic()
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (sounds[i].name.Contains("Music"))
            {
                sounds[i].source.volume = volumes[i] * musicVol.value;
                music.text = "" + (int)(musicVol.value * 10);
            }
        }
    }
}
[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0, 1)]
    public float vol;

    public bool loop;

    public AudioSource source;
}