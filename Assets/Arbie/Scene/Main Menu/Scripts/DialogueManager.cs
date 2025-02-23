using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;  // ✅ Required for UI buttons

using UnityEngine.UI;  // ✅ Required for UI buttons

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;
    public GameObject dialogueBox;
    public Button nextButton;  // ✅ Reference to the "Next" button

    private Queue<string> sentences;
    private bool isDialogueActive = false;

    void Start()
    {
        Debug.Log("DialogueManager Initialized");
        sentences = new Queue<string>();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        if (isDialogueActive)
        {
            Debug.Log("Dialogue already in progress!");
            return;
        }

        isDialogueActive = true;
        nameText.text = dialogue.name;

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        dialogueBox.SetActive(true);  // ✅ Show the dialogue panel
        nextButton.gameObject.SetActive(true);  // ✅ Show "Next" button
        nextButton.interactable = true;

        DisplayNextSentence();
    }






    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;

        AudioManager.instance.Play("Dialogue");  // ✅ Call your AudioManager to play SFX

        if (sentences.Count == 0)
        {
            nextButton.interactable = false;
        }
    }



    void EndDialogue()
    {
        Debug.Log("End of conversation.");
        isDialogueActive = false;
        animator.SetBool("IsOpen", false);
    }



    void Update()
    {
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            DisplayNextSentence();
        }
    }
}
