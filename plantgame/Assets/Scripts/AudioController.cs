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

    public bool isItPlaying;

    [SerializeField]
    public List<AudioClip> laugh;
    [SerializeField]
    public List<AudioClip> water;
    [SerializeField]
    public List<AudioClip> chomp;
    [SerializeField]
    public List<AudioClip> yippie;
    [SerializeField]
    public List<AudioClip> yippieS3;
    [SerializeField]
    public List<AudioClip> yuck;
    [SerializeField]
    public List<AudioClip> yuckS3;
    [SerializeField]
    public List<AudioClip> brush;
    [SerializeField]
    public List<AudioClip> purr;
    [SerializeField]
    public List<AudioClip> click;

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

        isItPlaying = false;

        //clipToPlay = null;
        //soundFX.clip = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        soundFX = GetComponent<AudioSource>();
    }

    public void PlayEatFX()
    {
        clipToPlay = RandomizeTrack(yippie);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayEatFX2()
    {
        clipToPlay = RandomizeTrack(yippieS3);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayYuck()
    {
        clipToPlay = RandomizeTrack(yuck);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayYuck2()
    {
        clipToPlay = RandomizeTrack(yuckS3);
        soundFX.clip = clipToPlay;
        soundFX.Play();
    }

    public void PlayPurr()
    {
        clipToPlay = RandomizeTrack(purr);
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

    public void PlayLaugh()
    {
        clipToPlay = laugh[0];
        soundFX.clip = clipToPlay;
        soundFX.Play();
        //soundFX.PlayOneShot(clipToPlay);
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
        Debug.Log("I'm in Audio");
        PlayLaugh();
        //yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(6);

        //yield return new WaitWhile(() => soundFX.isPlaying);

        //soundFX.clip = chomp;
        PlayChomp();
        //yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(1);
        //soundFX.PlayOneShot(clipToPlay);
        //yield return new WaitWhile(() => soundFX.isPlaying);
    }

    public void ClearAudio()
    {
        clipToPlay = null;
        soundFX.clip = null;
    }

    public bool IsItPlaying()
    {
        if(soundFX.isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private AudioClip RandomizeTrack(List<AudioClip> clips)
    {
        AudioClip chosenClip;

        int random = UnityEngine.Random.Range(0, clips.Count);

        return chosenClip = clips[random];
    }
}
