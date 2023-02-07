using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character")]
public class CharacterData : ScriptableObject
{
    public enum Gender
    {
        Male,
        Female,
        None
    }

    public Gender _gender;

    public string _name;

    [TextArea]
    public string _description;

    [Header("Emotions")]
    public Sprite _happySprite;
    public Sprite _sadSprite;
    public Sprite _madSprite;

}
