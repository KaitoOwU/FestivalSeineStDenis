using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public enum Emotion
    {
        Happy,
        Sad,
        Angry,
        Surprised,
        Neutral,
        Flushed
        
    }

    public CharacterData _character;

    public Emotion _emotion;

    [TextArea]
    public string _line;

}
