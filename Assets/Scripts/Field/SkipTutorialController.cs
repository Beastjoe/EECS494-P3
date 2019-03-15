using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;

public class SkipTutorialController : MonoBehaviour
{
    bool[] skipTutorial = new bool[4];
    // Start is called before the first frame update
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
            SceneManager.LoadScene("playLab");
        }
    }
}
