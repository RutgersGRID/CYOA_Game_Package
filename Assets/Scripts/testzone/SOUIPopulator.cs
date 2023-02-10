// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UIElements;

// public class SOUIPopulator : MonoBehaviour
// {
//     public List<CharacterSO> characters;
//     private int currentIndex = 0;

//     private TextElement nameText;
//     private TextElement dialogueText;
//     private VisualElement propImage;
//     private VisualElement characterImage;
//     private Button nextButton;

//     private void Start()
//     {
//         var root = GetComponent<UIDocument>().rootVisualElement;

//         nameText = root.Q<TextElement>("dlog-name");
//         if (nameText == null)
//         {
//             Debug.LogError("Could not find nameText element");
//             return;
//         }

//         dialogueText = root.Q<TextElement>("dlog-text");
//         if (dialogueText == null)
//         {
//             Debug.LogError("Could not find dialogueText element");
//             return;
//         }

//         propImage = root.Q<VisualElement>("Props");
//         if (propImage == null)
//         {
//             Debug.LogError("Could not find propImage element");
//             return;
//         }

//         characterImage = root.Q<VisualElement>("CharacterRight");
//         if (characterImage == null)
//         {
//             Debug.LogError("Could not find characterImage element");
//             return;
//         }

//         nextButton = root.Q<Button>("dlog-button");
//         if (nextButton == null)
//         {
//             Debug.LogError("Could not find nextButton element");
//             return;
//         }

//         nextButton.RegisterCallback<ClickEvent>(NextCharacter);

//         PopulateUI();
//     }

//     private void PopulateUI()
//     {
//         var character = characters[currentIndex];
//         nameText.text = character.Character_Name;
//         dialogueText.text = character.Character_Dialogue;
//         //propImage.style.backgroundImage = LoadSprite(character.Prop_Sprite);
//         //characterImage.style.backgroundImage = LoadSprite(character.Character_Sprite);
//     }

//     private void NextCharacter(ClickEvent evt)
//     {
//         currentIndex = (currentIndex + 1) % characters.Count;
//         PopulateUI();
//     }

//     private Sprite LoadSprite(string spriteName)
//     {
//         return Resources.Load<Sprite>(spriteName);
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SOUIPopulator : MonoBehaviour
{
    public CSVtoSO csvToSO;

    private int currentIndex = 0;

    private TextElement nameText;
    private TextElement dialogueText;
    private VisualElement propImage;
    private VisualElement characterImage;
    private Button nextButton;

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        nameText = root.Q<TextElement>("dlog-name");
        if (nameText == null)
        {
            Debug.LogError("Could not find nameText element");
            return;
        }

        dialogueText = root.Q<TextElement>("dlog-text");
        if (dialogueText == null)
        {
            Debug.LogError("Could not find dialogueText element");
            return;
        }

        propImage = root.Q<VisualElement>("Props");
        if (propImage == null)
        {
            Debug.LogError("Could not find propImage element");
            return;
        }

        characterImage = root.Q<VisualElement>("CharacterRight");
        if (characterImage == null)
        {
            Debug.LogError("Could not find characterImage element");
            return;
        }

        nextButton = root.Q<Button>("dlog-button");
        if (nextButton == null)
        {
            Debug.LogError("Could not find nextButton element");
            return;
        }

        nextButton.RegisterCallback<ClickEvent>(NextCharacter);

        PopulateUI();
    }

    private void PopulateUI()
    {
        var character = csvToSO.characters[currentIndex];
        nameText.text = character.Character_Name;
        dialogueText.text = character.Character_Dialogue;
        //propImage.style.backgroundImage = LoadSprite(character.Prop_Sprite);
        //characterImage.style.backgroundImage = LoadSprite(character.Character_Sprite);
    }

    private void NextCharacter(ClickEvent evt)
    {
        currentIndex = (currentIndex + 1) % csvToSO.characters.Count;
        PopulateUI();
    }

    private Sprite LoadSprite(string spriteName)
    {
        return Resources.Load<Sprite>(spriteName);
    }
}
