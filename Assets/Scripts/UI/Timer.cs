using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public GameObject blackPanel;

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
            text.text = "";
            blackPanel.SetActive(true);
            int redScore = Inventory.instance.numOfRedTeamResource;
            int blueScore = Inventory.instance.numOfBlueTeamResource;
            if (redScore > blueScore) {
                winText.text = "Red Team Wins!";
            }
            else if (redScore == blueScore) {
                winText.text = "Draw";
            }
            else {
                winText.text = "Blue Team Wins!";
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("playLab");
            }
        }
    }
}
