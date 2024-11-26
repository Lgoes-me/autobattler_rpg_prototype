using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [field: SerializeField] private AudioSource SfxAudioSource { get; set; }
    [field: SerializeField] private AudioSource MusicAudioSource { get; set; }
    [field: SerializeField] private AudioSource LoopAudioSource { get; set; }
    [field: SerializeField] private List<Sfx> SoundEffects { get; set; }
    [field: SerializeField] private List<Music> Musics { get; set; }
    
    private MusicType CurrentMusic { get; set; }
    private Coroutine MusicCoroutine { get; set; }
    
    public void PlaySound(SfxType type)
    {
        var sfx = SoundEffects.FirstOrDefault(sfx => sfx.Type == type);

        if (sfx == null)
            throw new Exception($"som do tipo: {type} não cadastrado");
        
        PlaySfx(sfx);
    }

    public void PlaySfx(Sfx sfx)
    {
        if(sfx.Sounds.Count == 0)
            return;
        
        var sound = sfx.Sounds[Random.Range(0, sfx.Sounds.Count)];
        SfxAudioSource.pitch = Random.Range(0.9f, 1.15f);
        SfxAudioSource.PlayOneShot(sound);
    }
    
    public void PlayMusic(MusicType type)
    {
        if(type == CurrentMusic)
            return;
        
        var music = Musics.FirstOrDefault(sfx => sfx.Type == type);
        
        if (music == null)
            throw new Exception($"musica do tipo: {type} não cadastrado");
        
        CurrentMusic = type;
        
        MusicAudioSource.clip = music.MainMusic;
        
        //TODO poder mudar o volume maximo da musica (aqui e no lerp do fade)
        MusicAudioSource.volume = 1;
        LoopAudioSource.volume = 0;
        
        MusicAudioSource.pitch = Random.Range(0.9f, 1.15f);
        MusicAudioSource.loop = false;
        MusicAudioSource.Play(0);

        if (MusicCoroutine != null)
            StopCoroutine(MusicCoroutine);

        MusicCoroutine = StartCoroutine(MusicLoopCoroutine(music));
    }

    private IEnumerator MusicLoopCoroutine(Music music)
    {
        yield return new WaitForSeconds(music.MainMusic.length - 5f);

        LoopAudioSource.clip = music.SecondaryLoop;
        LoopAudioSource.pitch = Random.Range(0.9f, 1.15f);
        LoopAudioSource.loop = true;
        LoopAudioSource.Play(0);
        
        yield return Fade(5);
    }
    
    private IEnumerator Fade(int delay) 
    {
        float timeElapsed = 0;

        while (timeElapsed < delay) 
        {
            MusicAudioSource.volume = Mathf.Lerp(1, 0, timeElapsed / delay);
            LoopAudioSource.volume = Mathf.Lerp(0, 1, timeElapsed / delay);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

}

[Serializable]
public class Sfx
{
    [field: SerializeField] public SfxType Type { get; set; }
    [field: SerializeField] public List<AudioClip> Sounds { get; set; }
}

public enum SfxType
{
    Unknown = 0,
    Slash = 1,
    Voice = 2,
}

[Serializable]
public class Music
{
    [field: SerializeField] public MusicType Type { get; set; }
    [field: SerializeField] public AudioClip MainMusic { get; set; }
    [field: SerializeField] public AudioClip SecondaryLoop { get; set; }
}

public enum MusicType
{
    Unknown = 0,
    Dungeon = 1,
    Battle = 2,
    Victory = 3
}