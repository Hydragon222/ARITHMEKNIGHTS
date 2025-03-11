using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mobilesafer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Transfer());
    }

    private IEnumerator Transfer()
    {
        yield return new WaitForSeconds(0.52f);
        SceneManager.LoadScene("MainMenu");
    }
}
