using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReadCSV : MonoBehaviour
{
    public TextAsset textAssetData;

    [System.Serializable]

    public class Character
    {
        public string Character_Name;
        public string Character_Dialogue;
        public string Prop_Sprite;
        public string Character_Sprite;
    }

    [System.Serializable]

    public class CharacterList
    {
        public Character[] character;
    }

    public CharacterList myCharacterList = new CharacterList();

    // Start is called before the first frame update
    void Start()
    {
        ReadCSVFile();
    }

    void ReadCSVFile()
    {
        string[] data = textAssetData.text.Split(new string[] { ",", "\n"}, StringSplitOptions.None);

        int tableSize = data.Length / 4 - 1;
        myCharacterList.character = new Character[tableSize];

        for( int i = 0; i < tableSize; i++)
        {
            myCharacterList.character[i] = new Character();
            myCharacterList.character[i].Character_Name = data[4 * (i + 1)];
            myCharacterList.character[i].Character_Dialogue = data[4 * (i + 1) + 1];
            myCharacterList.character[i].Prop_Sprite = data[4 * (i + 1) + 2];
            myCharacterList.character[i].Character_Sprite= data[4 * (i + 1) + 3];
        }
    }
}
