using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;
    public GameObject dialogueBox;
    public Button nextButton;

    private Queue<string> sentences;
    private bool isDialogueActive = false;

    public GameObject diagramPanel;
    public Image diagramImage;
    public Sprite[] diagramSprites;
    private int currentDiagramIndex = -1;

    public GameObject extraInfoPanel;
    public TextMeshProUGUI extraInfoText;
    public RectTransform extraInfoTransform;
    public List<ExtraInfoEntry> extraInfoEntries;
    private int currentExtraInfoIndex = -1;

    public GameObject mainTextPanel;
    public TextMeshProUGUI mainText;
    public RectTransform mainTextTransform;
    public List<MainTextEntry> mainTextEntries;
    private int currentMainTextIndex = -1;
    private bool isMainTextShown = false;

    // Cache the last dialogue used so we can repeat it later.
    private Dialogue lastDialogue;

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
        // Cache the dialogue for later repeating.
        lastDialogue = dialogue;

        if (animator != null)
        {
            animator.SetBool("IsOpen", true);
        }
        else
        {
            Debug.LogWarning("Animator not assigned in DialogueManager.");
        }

        if (isDialogueActive)
        {
            Debug.Log("Dialogue already in progress!");
            return;
        }

        isDialogueActive = true;
        nameText.text = dialogue.name;

        // Reset indices when starting a new dialogue
        currentDiagramIndex = -1;
        currentExtraInfoIndex = -1;
        currentMainTextIndex = -1;
        isMainTextShown = false;

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        dialogueBox.SetActive(true);
        if (nextButton != null)
            nextButton.interactable = true;

        DisplayNextSentence();
    }

    // This method allows repeating the dialogue sequence when the character is clicked again.
    public void RepeatDialogue()
    {
        if (!isDialogueActive && lastDialogue != null)
        {
            Debug.Log("Repeating dialogue.");
            StartDialogue(lastDialogue);
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            if (nextButton != null)
            {
                nextButton.interactable = false;
            }
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        if (sentence.Contains("[showDiagram]"))
        {
            sentence = sentence.Replace("[showDiagram]", "");
            currentDiagramIndex++;
            ShowDiagram(currentDiagramIndex);
        }

        if (sentence.Contains("[hideDiagram]"))
        {
            sentence = sentence.Replace("[hideDiagram]", "");
            HideDiagram();
        }

        if (sentence.Contains("[showExtraInfo]"))
        {
            sentence = sentence.Replace("[showExtraInfo]", "");
            currentExtraInfoIndex++;
            ShowExtraInfo(currentExtraInfoIndex);
        }

        if (sentence.Contains("[hideExtraInfo]"))
        {
            sentence = sentence.Replace("[hideExtraInfo]", "");
            HideExtraInfo();
        }

        if (sentence.Contains("[showMainText]") && !isMainTextShown)
        {
            sentence = sentence.Replace("[showMainText]", "");
            currentMainTextIndex++;
            ShowMainText(currentMainTextIndex);
            isMainTextShown = true;
        }
        else if (sentence.Contains("[switchMainText]") && isMainTextShown)
        {
            sentence = sentence.Replace("[switchMainText]", "");
            currentMainTextIndex++;
            ShowMainText(currentMainTextIndex);
        }

        if (sentence.Contains("[hideMainText]"))
        {
            sentence = sentence.Replace("[hideMainText]", "");
            HideMainText();
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        AudioManager.instance.Play("Dialogue");

        if (sentences.Count == 0 && nextButton != null)
        {
            nextButton.interactable = false;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");
        isDialogueActive = false;
        if (animator != null)
        {
            animator.SetBool("IsOpen", false);
        }
        PlayerControls player = FindObjectOfType<PlayerControls>();
        if (player != null)
        {
            player.hasTappedDialogue = false;
        }
    }

    void Update()
    {
        // Continue dialogue if active and user clicks
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            DisplayNextSentence();
        }
        // If dialogue is not active, you could check for a character click to repeat dialogue.
        // For example, you might have code like this (or call RepeatDialogue() from another script):
        // if (!isDialogueActive && Input.GetMouseButtonDown(0))
        // {
        //     RepeatDialogue();
        // }
    }

    public void ShowDiagram(int index)
    {
        if (diagramPanel == null || diagramImage == null || diagramSprites == null) return;

        if (index >= 0 && index < diagramSprites.Length)
        {
            diagramImage.sprite = diagramSprites[index];
            diagramPanel.SetActive(true);
        }
    }

    public void HideDiagram()
    {
        if (diagramPanel != null)
            diagramPanel.SetActive(false);
    }

    public void ShowExtraInfo(int index)
    {
        if (extraInfoPanel == null || extraInfoText == null || extraInfoTransform == null) return;

        if (index >= 0 && index < extraInfoEntries.Count)
        {
            extraInfoText.text = extraInfoEntries[index].text;
            extraInfoTransform.anchoredPosition = extraInfoEntries[index].position;
            extraInfoText.fontSize = extraInfoEntries[index].fontSize;
            extraInfoText.fontStyle = extraInfoEntries[index].fontStyle;
            extraInfoPanel.SetActive(true);
        }
    }

    public void HideExtraInfo()
    {
        if (extraInfoPanel != null)
            extraInfoPanel.SetActive(false);
    }

    public void ShowMainText(int index)
    {
        if (mainTextPanel == null || mainText == null || mainTextTransform == null) return;

        if (index >= 0 && index < mainTextEntries.Count)
        {
            mainText.text = mainTextEntries[index].text;
            mainTextTransform.anchoredPosition = mainTextEntries[index].position;
            mainText.fontSize = mainTextEntries[index].fontSize;
            mainText.fontStyle = mainTextEntries[index].fontStyle;
            mainTextPanel.SetActive(true);
        }
    }

    public void HideMainText()
    {
        if (mainTextPanel != null)
            mainTextPanel.SetActive(false);
    }
}

// ✅ FIX: Added ExtraInfoEntry & MainTextEntry at the bottom to resolve the errors
[System.Serializable]
public class ExtraInfoEntry
{
    public string text;
    public Vector2 position;
    public float fontSize = 36;
    public FontStyles fontStyle = FontStyles.Normal;
}

[System.Serializable]
public class MainTextEntry
{
    public string text;
    public Vector2 position;
    public float fontSize = 40;
    public FontStyles fontStyle = FontStyles.Bold;
}
