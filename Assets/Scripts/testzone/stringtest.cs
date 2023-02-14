using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class stringtest : MonoBehaviour
{
    private VisualElement characterImage;
    private string ResourcesLoadC = "Characters/";
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        characterImage = root.Q<VisualElement>("CharacterRight");
        if (characterImage == null)
        {
            Debug.LogError("Could not find characterImage element");
            return;
        }
        characterImage.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>(ResourcesLoadC+"Facu_Happy")); 
        Debug.Log(Resources.Load<Sprite>(ResourcesLoadC+"Facu_Happy"));
    }

    private void PopulateUI()
    {
        //characterImage.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>(ResourcesLoadC+"Facu_Happy.png"));       
    }
}
