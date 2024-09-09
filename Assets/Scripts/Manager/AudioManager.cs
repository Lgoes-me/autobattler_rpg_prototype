using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [field: SerializeField] private AudioSource SfxAudioSource { get; set; }
    [field: SerializeField] private AudioSource MusicAudioSource { get; set; }
    [field: SerializeField] private List<Sfx> SoundEffects { get; set; }
    [field: SerializeField] private AudioClip Music { get; set; }

    public void PlaySound(SfxType type)
    {
        var sfx = SoundEffects.FirstOrDefault(sfx => sfx.Type == type);

        if (sfx == null)
            throw new Exception($"som do tipo: {type} não cadastrado");
        
        var sound = sfx.Sounds[Random.Range(0, sfx.Sounds.Count)];
        SfxAudioSource.pitch = Random.Range(0.9f, 1.15f);
        SfxAudioSource.PlayOneShot(sound);
    }
    
    public void PlayMusic()
    {
        MusicAudioSource.clip = Music;
        MusicAudioSource.Play(0);
    }
}

[System.Serializable]
public class Sfx
{
    [field: SerializeField] public SfxType Type { get; set; }
    [field: SerializeField] public List<AudioClip> Sounds { get; set; }
}

public enum SfxType
{
    Slash,
}