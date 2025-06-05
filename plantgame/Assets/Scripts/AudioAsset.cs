using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioAsset : ScriptableObject
{
    [SerializeField]
    public AudioClip[] click;

    [SerializeField]
    public AudioClip[] chomp;

    [SerializeField]
    public AudioClip[] brush;

    [SerializeField]
    public AudioClip[] water;

    [SerializeField]
    public AudioClip[] reactionSoundsPurr;

    [SerializeField]
    public AudioClip[] reactionsSoundsYuck;

    [SerializeField]
    public AudioClip[] reactionsSoundsS3Yuck;

    [SerializeField]
    public AudioClip[] reactionSoundsYippie;

    [SerializeField]
    public AudioClip[] reactionSoundsS3Yippie;

    [SerializeField]
    public AudioClip[] evilLaugh;


}
