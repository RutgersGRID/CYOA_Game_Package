using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadCSV : MonoBehaviour
{


/// https://www.youtube.com/watch?v=mAeTRCT0qZg example didnt work breaks after TextAsset line.
    // List<Dialogue> dialogues = new List<Dialogue>();

    // void Start()
    // {
    //     TextAsset Scene_Templet = Resources.Load<TextAsset>("SceneTemplet");

    //     string[] data = Scene_Templet.text.Split(new char[] { '\n' });

    //     for (int i = 1; i <data.Length - 1; i++)
    //     {
    //         string[] row = data[i].Split(new char[] { ','});
    //         Dialogue d = new Dialogue();
    //         // int.TryParse(row[0], out d.Character_Name); for ints
    //         d.Character_Name = row[0];
    //         d.Character_Dialogue = row[1];
    //         // Sprite.TryParse(row[2], out d.Prop_Sprite);
    //         // Sprite.TryParse(row[2], out d.Character_Sprite);
    //         d.Prop_Sprite = row[2];
    //         d.Character_Sprite = row[3];

    //         dialogues.Add(d);
    //     }

    //     foreach (Dialogue d in dialogues)
    //     {
    //         Debug.Log(d.Character_Name + "," + d.Character_Dialogue);
    //     }
    // }

    void Update()
    {

    }
}
