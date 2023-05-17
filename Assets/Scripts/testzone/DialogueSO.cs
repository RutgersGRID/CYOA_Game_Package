using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public int ID;
    public string Speaker;
    public string Line;
    public Sprite LeftSideSpeaker;
    public Sprite RightSideSpeaker;
    public Sprite Prop;
    public Sprite Background;
    // public string LeftSideSpeaker;
    // public string RightSideSpeaker;
    // public string Prop;
    // public string Background;
    public int Checkpoint;
    public string Type;
    public int GoToID;
    public int Effect;
    //public string A1;
    public string A1Answer;
    public int GoToIDA1;
    public int EffectA1;
    //public string A2;
    public string A2Answer;
    public int GoToIDA2;
    public int EffectA2;
    //public string A3;
    public string A3Answer;
    public int GoToIDA3;
    public int EffectA3;

}
