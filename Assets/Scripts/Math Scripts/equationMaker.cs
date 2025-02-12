using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class equationMaker : MonoBehaviour
{
    public TextMeshProUGUI equationText;

    public float number1;
    public float number2;
    public float expectedAnswer;

    // Start is called before the first frame update
    void Start()
    {
        number1 = Random.Range(1, 10);
        number2 = Random.Range(2, 2);

        expectedAnswer = (number1 * number2);

        equationText.text = number1.ToString() + "x" + number2.ToString();
    }

}
