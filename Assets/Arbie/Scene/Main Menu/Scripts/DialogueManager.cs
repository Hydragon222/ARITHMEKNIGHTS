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

    // ✅ Diagram Variables
    public GameObject diagramPanel;
    public Image diagramImage;
    public Sprite[] diagramSprites;
    private int currentDiagramIndex = -1;

    // ✅ Extra Info Variables
    public GameObject extraInfoPanel;
    public TextMeshProUGUI extraInfoText;
    public RectTransform extraInfoTransform;
    public List<ExtraInfoEntry> extraInfoEntries;
    private int currentExtraInfoIndex = -1;

    // ✅ Main Text Variables (Now with showMainText & switchMainText)
    public GameObject mainTextPanel;
    public TextMeshProUGUI mainText;
    public RectTransform mainTextTransform;
    public List<MainTextEntry> mainTextEntries;
    private int currentMainTextIndex = -1;
    private bool isMainTextShown = false; // ✅ Tracks if main text is initially shown

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

        dialogueBox.SetActive(true);
        nextButton.gameObject.SetActive(true);
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

        // ✅ Handle Diagram Display
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

        // ✅ Handle Extra Info Display
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

        // ✅ Handle Main Text (First time using [showMainText], then [switchMainText])
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

        if (sentences.Count == 0)
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
        animator.SetBool("IsOpen", false);
        FindObjectOfType<PlayerControls>().hasTappedDialogue = false;
    }

    void Update()
    {
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            DisplayNextSentence();
        }
    }

    // ✅ Show Diagram Function
    public void ShowDiagram(int index)
    {
        if (diagramPanel == null || diagramImage == null || diagramSprites == null) return;

        if (index >= 0 && index < diagramSprites.Length)
        {
            diagramImage.sprite = diagramSprites[index];
            diagramPanel.SetActive(true);
        }
    }

    // ✅ Hide Diagram Function
    public void HideDiagram()
    {
        if (diagramPanel != null)
            diagramPanel.SetActive(false);
    }

    // ✅ Show Extra Info Function
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

    // ✅ Hide Extra Info Function
    public void HideExtraInfo()
    {
        if (extraInfoPanel != null)
            extraInfoPanel.SetActive(false);
    }

    // ✅ Show Main Text Function (First time with [showMainText], then [switchMainText])
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

    // ✅ Hide Main Text Function
    public void HideMainText()
    {
        if (mainTextPanel != null)
            mainTextPanel.SetActive(false);
    }
}

// ✅ Extra Info Struct (Editable in Inspector)
[System.Serializable]
public class ExtraInfoEntry
{
    public string text;
    public Vector2 position;
    public float fontSize = 36;
    public FontStyles fontStyle = FontStyles.Normal;
}

// ✅ Main Text Struct (Editable in Inspector)
[System.Serializable]
public class MainTextEntry
{
    public string text;
    public Vector2 position;
    public float fontSize = 40;
    public FontStyles fontStyle = FontStyles.Bold;
}
