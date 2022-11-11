using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Input sprites, names, dialogue, etc.
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRendererProp;
    public float typeSpeed;
    public Animator DBox;
    public bool stopAudioSource;
    public int charInterval;
    public AudioClip dialogueTypingSoundClip;

    // Array lists
    private Queue<string> sentences;
    private Queue<string> names;
    private Queue<Sprite> characters;
    private Queue<Sprite> props;
    private AudioSource audioSource;

    

    void Start()
    {
        // Loads up arrays
        sentences = new Queue<string>();
        names = new Queue<string>();
        characters = new Queue<Sprite>();
        props = new Queue<Sprite>();
        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    public void StartDialogue (Dialogue dialogue)
    {   
        // Clears queue to remove any lingering sentences.
        DBox.SetBool("IsOpen",true);
        sentences.Clear();
        names.Clear();
        characters.Clear();
        props.Clear();

        // Forloops that queue up elements from the array
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

        dialogueText.text = sentence;
        nameText.text = name;
        spriteRenderer.sprite = character;
        spriteRendererProp.sprite = prop;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }


    // Displays the letters in a sentence one at a time.
    IEnumerator TypeSentence (string sentence) 
    {   

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
        DBox.SetBool("IsOpen",false);
    }
   
}
