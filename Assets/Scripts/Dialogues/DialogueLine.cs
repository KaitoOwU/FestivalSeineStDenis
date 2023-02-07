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
        Crying,
        Rage,
        Talking,
        
    }

    public CharacterData _character;

    public Emotion _emotion;

    [TextArea]
    public string _line;

}