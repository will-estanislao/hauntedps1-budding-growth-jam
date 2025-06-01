using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource soundFX;

    [SerializeField]
    private AudioAsset audioAsset;

    private AudioClip clipToPlay;

    public static AudioController Instance { get; private set; }

    // Array of Sound FX
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        soundFX = GetComponent<AudioSource>();
    }

    public void PlayEatFX()
    {
        clipToPlay = RandomizeTrack(audioAsset.reactionSoundsYippie);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayEatFX2()
    {
        clipToPlay = RandomizeTrack(audioAsset.reactionSoundsS3Yippie);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayYuck()
    {
        clipToPlay = RandomizeTrack(audioAsset.reactionsSoundsYuck);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayYuck2()
    {
        clipToPlay = RandomizeTrack(audioAsset.reactionsSoundsS3Yuck);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayPurr()
    {
        clipToPlay = RandomizeTrack(audioAsset.reactionSoundsPurr);
        //soundFX.clip = clipToPlay;
        soundFX.PlayOneShot(clipToPlay, 0.10f);
    }

    public void PlayBrush()
    {
        clipToPlay = RandomizeTrack(audioAsset.brush);
        //soundFX.clip = clipToPlay;
        soundFX.PlayOneShot(clipToPlay, .075f);
    }

    public void PlayClick()
    {
        clipToPlay = RandomizeTrack(audioAsset.click);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayChomp()
    {
        clipToPlay = RandomizeTrack(audioAsset.chomp);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayWater()
    {
        clipToPlay = RandomizeTrack(audioAsset.water);
        soundFX.clip = clipToPlay;
        //soundFX.PlayOneShot(clipToPlay, 0.75f);
        soundFX.loop = true;
        soundFX.Play();
    }

    public void PlayLaugh()
    {
        clipToPlay = audioAsset.evilLaugh[0];
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void StopPlay()
    {
        soundFX.loop = false;
        soundFX.Pause();
    }

    // Play Specific sound
    public IEnumerator PlayEatSound(bool happy, int stage)
    {
        PlayChomp();
        yield return new WaitWhile(() => soundFX.isPlaying);


        if (happy)
        {
            if (stage < 3)
            {
                PlayEatFX();
            }
            else
            {
                PlayEatFX2();
            }
        }
        else
        {
            if (stage < 3)
            {
                PlayYuck();
            }
            else
            {
                PlayYuck2();
            }
        }
        yield return new WaitForSeconds(1);

    }

    public IEnumerator PlayPetSound()
    {
        PlayBrush();
        yield return new WaitForSeconds(1);
        PlayPurr();
        yield return new WaitForSeconds(1);
    }

    public IEnumerator PlayEndAudio()
    {
        PlayLaugh();
        yield return new WaitWhile(() => soundFX.isPlaying);
        clipToPlay = audioAsset.chomp[0];
        soundFX.clip = clipToPlay;
        soundFX.Play();
        yield return new WaitWhile(() => soundFX.isPlaying);
    }


    private AudioClip RandomizeTrack(List<AudioClip> clips)
    {
        AudioClip chosenClip;

        int random = UnityEngine.Random.Range(0, clips.Count);

        return chosenClip = clips[random];
    }
}
