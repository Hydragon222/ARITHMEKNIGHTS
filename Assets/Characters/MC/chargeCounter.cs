using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class chargeCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public int counter = 0;
    public int maxCounter = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll > 0 && counter < maxCounter)
        {
            counter++;
        }
        else if (scroll < 0 && counter > 0)
        {
            counter--;
        }
        counterText.text = counter.ToString();
    }
}
