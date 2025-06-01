using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource soundFX;

    [SerializeField]
    public List<AudioClip> click;
    [SerializeField]
    public List<AudioClip> chomp;
    [SerializeField]
    public List<AudioClip> brush;
    [SerializeField]
    public List<AudioClip> water;
    [SerializeField]
    public List<AudioClip> reactionSoundsPurr;
    [SerializeField]
    public List<AudioClip> reactionSoundsYuck;
    [SerializeField]
    public List<AudioClip> reactionSoundsS3Yuck;
    [SerializeField]
    public List<AudioClip> reactionSoundsYippie;
    [SerializeField]
    public List<AudioClip> reactionSoundsS3Yippie;

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

       // StartCoroutine(waitForSound(clipToPlay));
    }

    public void PlayEatFX()
    {
        clipToPlay = RandomizeTrack(reactionSoundsYippie);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayEatFX2()
    {
        clipToPlay = RandomizeTrack(reactionSoundsS3Yippie);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayYuck()
    {
        clipToPlay = RandomizeTrack(reactionSoundsYuck);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayYuck2()
    {
        clipToPlay = RandomizeTrack(reactionSoundsS3Yuck);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayPurr()
    {
        clipToPlay = RandomizeTrack(reactionSoundsPurr);
        //soundFX.clip = clipToPlay;
        soundFX.PlayOneShot(clipToPlay, 0.10f);
    }

    public void PlayBrush()
    {
        clipToPlay = RandomizeTrack(brush);
        //soundFX.clip = clipToPlay;
        soundFX.PlayOneShot(clipToPlay, .075f);
    }

    public void PlayClick()
    {
        clipToPlay = RandomizeTrack(click);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayChomp()
    {
        clipToPlay = RandomizeTrack(chomp);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayWater()
    {
        clipToPlay = RandomizeTrack(water);
        soundFX.clip = clipToPlay;
        //soundFX.PlayOneShot(clipToPlay, 0.75f);
        soundFX.loop = true;
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


    private AudioClip RandomizeTrack(List<AudioClip> clips)
    {
        AudioClip chosenClip;

        int random = UnityEngine.Random.Range(0, clips.Count);

        return chosenClip = clips[random];
    }
}
