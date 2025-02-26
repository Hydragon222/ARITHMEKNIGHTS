using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;  // ✅ Make sure this is at the top
    private bool hasTriggered = false; // ✅ Prevent multiple triggers

    public void TriggerDialogue()
    {
        DialogueManager dm = FindAnyObjectByType<DialogueManager>();
        if (dm == null)
        {
            Debug.LogError("❌ ERROR: DialogueManager not found in the scene!");
            return;
        }

        if (dm.IsDialogueActive()) // ✅ Prevents multiple dialogues at once
        {
            Debug.Log("Dialogue is already in progress. Ignoring duplicate trigger.");
            return;
        }

        dm.StartDialogue(dialogue);
    }


    private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(2f); // Adjust time if needed
        hasTriggered = false; // ✅ Allow re-trigger after delay
        Debug.Log("Dialogue trigger reset.");
    }
}
