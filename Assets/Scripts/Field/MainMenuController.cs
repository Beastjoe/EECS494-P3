using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    private static int playerCount = 4;
    bool[] hasPressed = new bool[playerCount];
    public Text PressCount;
    public GameObject BlackShader;


    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < playerCount; i++)
        {
            hasPressed[i] = false;
        }
        StartCoroutine(Fading());
    }

    // Update is called once per frame
    void Update()
    {    
        // Check the input
        for (int i = 0; i < playerCount; i++)
        {
            Gamepad gp = Gamepad.all[i];
            if (gp.aButton.isPressed)
            {
                hasPressed[i] = true;
            }
        }
        
        // Update the x count
        int xCount = 0;
        for (int i = 0; i < playerCount; i++)
        {
            if (hasPressed[i]) xCount++;
        }

        PressCount.text = xCount.ToString();
        Debug.Log(xCount);

        if (xCount == playerCount)
        {
            Debug.Log("All four x pressed");
            StartCoroutine(Fadeout());
        }
    }

    IEnumerator Fading() {
        BlackShader.SetActive(true);
        for (int i = 0; i < 100; ++i)
        {
            Color c = BlackShader.GetComponent<Image>().color;
            c.a -= 0.01f;
            BlackShader.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.001f);
        }
        BlackShader.SetActive(false);
    }

    IEnumerator Fadeout() {
        BlackShader.SetActive(true);
        for (int i = 0; i < 100; ++i)
        {
            Color c = BlackShader.GetComponent<Image>().color;
            c.a += 0.01f;
            BlackShader.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.003f);
        }
        SceneManager.LoadScene("Selection");
    }

}
