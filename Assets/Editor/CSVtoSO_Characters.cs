using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVtoSO_Characters
{
    private static string  CharacterCSVPath = "/Editor/Imports/SceneTemplet.csv";
    [MenuItem("Utilities/Generate Characters")]
    public static void GenerateCharacters()
    {
        string[] AllLines = File.ReadAllLines(Application.dataPath + CharacterCSVPath);
        string[] lines = AllLines.Split(new string[] { "\n" }, StringSplitOptions.None);

        int tableSize = lines.Length - 1;
        myCharacterList.character = new Character[tableSize - 1];

        int currentIndex = 0;
        for (int i = 1; i < tableSize; i++)
        {
            string line = lines[i];

            List<string> values = new List<string>();
            int currentLineIndex = 0;

            for (int j = 0; j < line.Length; j++)
            {
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

            Debug.Log($"fields: {splitData.Length}");

            ShelterCardSO shelterCard = ScriptableObject.CreateInstance<ShelterCardSO>();
            shelterCard.Name = splitData[0];
            shelterCard.IcoMicro = splitData[1];

            CharacterSO characterObj = ScriptableObject.CreateInstance<CharacterSO>();
            characterObj.Name = values[0];
            characterObj.Character_Dialogue = values[1];
            characterObj.Prop_Sprite = values[2];
            characterObj.Character_Sprite = values[3];

            AssetDatabase.CreateAsset(characterObj, $"Assets/CharacterSO/{characterObj.Name}.asset");
            currentIndex++;
        }

    }
}
