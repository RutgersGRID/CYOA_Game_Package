using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using LINQtoCSV;
using System.Linq;
public class testscript : MonoBehaviour
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

    void Start()
    {
        ReadCSVFile();
    }

    void ReadCSVFile()
    {
        string[] lines = textAssetData.text.Split(new string[] { "\n" }, StringSplitOptions.None);

        int tableSize = lines.Length - 1;
        myCharacterList.character = new Character[tableSize - 1];

        int currentIndex = 0;

        // Ignores the first row which are the headers
        for (int i = 1; i < tableSize; i++)
        {
            string line = lines[i];

            List<string> values = new List<string>();
            int currentLineIndex = 0;

            for (int j = 0; j < line.Length; j++)
            {
                // This section here ignores commas in between quotation marks
                if (line[j] == '"')
                {
                    int nextQuoteIndex = line.IndexOf('"', j + 1);
                    values.Add(line.Substring(j + 1, nextQuoteIndex - j - 1));
                    j = nextQuoteIndex + 1;

                    currentLineIndex = j + 1;
                }
                else if (line[j] == ',')
                {
                    values.Add(line.Substring(currentLineIndex, j - currentLineIndex));
                    currentLineIndex = j + 1;
                }
            }

            values.Add(line.Substring(currentLineIndex));

            myCharacterList.character[currentIndex] = new Character();
            myCharacterList.character[currentIndex].Character_Name = values[0];
            myCharacterList.character[currentIndex].Character_Dialogue = values[1];
            myCharacterList.character[currentIndex].Prop_Sprite = values[2];
            myCharacterList.character[currentIndex].Character_Sprite = values[3];

            currentIndex++;
        }
    }
}
