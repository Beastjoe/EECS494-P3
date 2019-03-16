using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Input;

public class Timer : MonoBehaviour
{
    public GameObject blackPanel;
    public GameObject instruction;

    float timer;
    Text text;
    Text winText;

    void Start() {
        text = GetComponent<Text>();
        winText = transform.GetChild(0).GetComponent<Text>();
        winText.text = "";
        timer = float.Parse(text.text);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer >= 0.0f) {
            text.text = Mathf.Ceil(timer).ToString("F0");
        }
        else {
            text.text = "00";
            blackPanel.SetActive(true);
            instruction.SetActive(true);
            int redScore = Inventory.instance.numOfRedTeamResource;
            int blueScore = Inventory.instance.numOfBlueTeamResource;
            if (redScore > blueScore) {
                winText.text = "Red Team Wins!";
                blackPanel.GetComponent<Image>().color = new Vector4(1, 0.6f, 0.77f, 0.36f);
            }
            else if (redScore == blueScore) {
                winText.text = "Draw";
            }
            else {
                winText.text = "Blue Team Wins!";
                blackPanel.GetComponent<Image>().color = new Vector4(0.6f, 1, 1, 0.36f);

            }

            if (Gamepad.all[0].aButton.isPressed || Gamepad.all[1].aButton.isPressed ||
            Gamepad.all[2].aButton.isPressed || Gamepad.all[3].aButton.isPressed)
            {
                SceneManager.LoadScene("playLab");
            }

            if (Gamepad.all[0].bButton.isPressed || Gamepad.all[1].bButton.isPressed ||
            Gamepad.all[2].bButton.isPressed || Gamepad.all[3].bButton.isPressed)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
