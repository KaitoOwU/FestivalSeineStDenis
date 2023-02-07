using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] _lines;

    public DialogueLine[] Lines
    {
        get
        {
           return _lines;
        }
        set
        {
            return;
        }
    }
}
