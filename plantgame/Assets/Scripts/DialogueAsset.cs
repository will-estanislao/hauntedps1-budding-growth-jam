using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    [TextArea]
    public string[] dialogue;

    [TextArea]
    public string[] secretDialogue;

    [TextArea]
    public string[] badEndDialogue;

    [TextArea]
    public string[] goodEndDialogue;
}
