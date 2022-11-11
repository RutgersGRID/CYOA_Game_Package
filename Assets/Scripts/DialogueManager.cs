using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRendererProp;
    public float typeSpeed;
    //public Sprite characterSprite;
    //public Sprite propSprite;

    //public Animator DialogueBox;
    public Animator DBox;

    private Queue<string> sentences;
    private Queue<string> names;
    private Queue<Sprite> characters;
    private Queue<Sprite> props;

    public AudioClip dialogueTypingSoundClip;
    private AudioSource audioSource;

    public bool stopAudioSource;

    public int charInterval;
 /*    private Queue<Sprite> characters;
    private Queue<Sprite> props; */

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        //Sprite sprite = Resources.Load<Sprite>("Resources/Characters/Facu");

        //Sprite sprite = Resources.Load<Sprite>(path);
        //Sprite[] characterSprites = Resources.LoadAll <Sprite> ("Resources/Characters/Facu");
        
        characters = new Queue<Sprite>();
        props = new Queue<Sprite>();
        //props = new Queue<Sprite>();

        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    public void StartDialogue (Dialogue dialogue)
    {   
        //DialogueBox.SetBool("IsOpen",true);
        DBox.SetBool("IsOpen",true);
        //Debug.Log("Starting conversation with " + dialogue.name);

        //nameText.text = dialogue.name;


        sentences.Clear();
        names.Clear();
        characters.Clear();
        props.Clear();

    /*     characters.Clear();
        props.Clear(); */

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
            
        }
        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }

        foreach (Sprite character in dialogue.characters)
        {
            characters.Enqueue(character);
        }
        foreach (Sprite prop in dialogue.props)
        {
            props.Enqueue(prop);
        }

//        foreach (Sprite prop in dialogue.props)
//        {
//            props.Enqueue(prop);
//        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string name = names.Dequeue();
        string sentence = sentences.Dequeue();
        Sprite character = characters.Dequeue();
        Sprite prop = props.Dequeue();
/*         Sprite character = characters.Dequeue();
        Sprite prop = props.Dequeue(); */

        dialogueText.text = sentence;
        nameText.text = name;
        spriteRenderer.sprite = character;
        spriteRendererProp.sprite = prop;
        //Sprite sprite = characterSprites[Array.IndexOf(names, "textureName")];
        //dialogSpriteRenderer.sprite = sprite;


     /*    characterSprite.Sprite = character;
        propSprite.Sprite = prop; */
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }

    IEnumerator TypeSentence (string sentence) 
    {   //char space = " ";
        dialogueText.text = "";
        int counter = 0;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
            if ((letter.Equals(" ") == false) && (counter % charInterval == 0))
            {
                audioSource.PlayOneShot(dialogueTypingSoundClip);
            }

            yield return new WaitForSeconds(.02f);
            counter++;
        }
    }

    void EndDialogue ()
    {
        //Debug.Log("End of conversation.");
        //DialogueBox.SetBool("IsOpen",false);
        DBox.SetBool("IsOpen",false);
    }
   
}
