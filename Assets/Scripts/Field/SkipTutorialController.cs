using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.WSA.WebCam;

public class SkipTutorialController : MonoBehaviour
{
    bool[] skipTutorial = new bool[4];
    // Start is called before the first frame update
    public GameObject BlackShader;
    void Start()
    {
        for (int i = 0; i < skipTutorial.Length; ++i)
        {
            skipTutorial[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < skipTutorial.Length; i++)
        {
            Gamepad gp = Gamepad.all[i];
            if (gp.xButton.isPressed)
            {
                skipTutorial[i] = true;
            }
        }

        bool pass = true;
        for (int i = 0; i < skipTutorial.Length; ++i)
        {
            pass &= skipTutorial[i];
        }

        if (pass)
        {
            StartCoroutine(Fading());
        }
    }
       
    IEnumerator Fading()
    {
        BlackShader.SetActive(true);
        for (int i = 0; i < 100; ++i)
        {
            Color c = BlackShader.GetComponent<Image>().color;
            c.a += 0.01f;
            BlackShader.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.015f);
        }
        SceneManager.LoadScene("playLab");
    }
    
}
