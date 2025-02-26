using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.AudioSettings;

public class healCooldown : MonoBehaviour
{
    [SerializeField] private TMP_Text cooldownText;
    private float cooldownDuration = 30f;
    [SerializeField] private mobileUI mobileui;

    public void StartCooldown()
    {
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        float remainingTime = cooldownDuration;

        while (remainingTime > 0)
        {
            cooldownText.text = remainingTime.ToString("F0");
            remainingTime -= 1;
            yield return new WaitForSeconds(1f);
        }

        if (mobileui.IsRunningOnPC())
        {
            cooldownText.text = "F"; // Reset text when cooldown ends
        }
        else { cooldownText.text = " "; }
    }
}
