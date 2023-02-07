using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Assets/Characters/New Character Objects")]

public class CharacterSO : ScriptableObject
{
    public Sprite Character_Sprite;
    public Sprite Prop_Sprite;
    public string Character_Name;
    public string Character_Dialogue;
}    

