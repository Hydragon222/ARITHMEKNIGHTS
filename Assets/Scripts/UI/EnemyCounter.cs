using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField] private PlayerControls playerControls;
    private TMP_Text enemiesRemainingTxt;
    private float enemiesRemaining;
    // Start is called before the first frame update
    void Start()
    {
        enemiesRemainingTxt = GetComponent<TMP_Text>();
        enemiesRemaining = playerControls.killsReqd;
        enemiesRemainingTxt.text = $"{enemiesRemaining}";
    }

    // Update is called once per frame
    public void Score()
    {
        enemiesRemaining -= 1;
        enemiesRemainingTxt.text = $"{enemiesRemaining}";
    }
}
