using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioAsset : ScriptableObject
{
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
    public List<AudioClip> reactionsSoundsYuck;

    [SerializeField]
    public List<AudioClip> reactionsSoundsS3Yuck;

    [SerializeField]
    public List<AudioClip> reactionSoundsYippie;

    [SerializeField]
    public List<AudioClip> reactionSoundsS3Yippie;

    [SerializeField]
    public List<AudioClip> evilLaugh;


}
