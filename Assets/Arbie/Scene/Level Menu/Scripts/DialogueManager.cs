using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;  // ✅ Required for UI buttons

using UnityEngine.UI;  // ✅ Required for UI buttons

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
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
        if (isDialogueActive)
        {
            Debug.Log("Dialogue already in progress!");
            return;
        }

        isDialogueActive = true;
        Debug.Log("Starting conversation with " + dialogue.name);

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

        if (sentences.Count == 0)
        {
            nextButton.interactable = false;  // ✅ Disable button when last sentence is shown
        }
    }




    void EndDialogue()
    {
        Debug.Log("End of conversation.");
        isDialogueActive = false;

        dialogueBox.SetActive(false);  // ❌ Hide the dialogue panel
        nextButton.gameObject.SetActive(false);  // ❌ Hide "Next" button
    }



    void Update()
    {
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            DisplayNextSentence();
        }
    }
}
