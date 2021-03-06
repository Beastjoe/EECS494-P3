﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class CharacterIndicator : MonoBehaviour {
    private bool isMoving;
    public int playerIndex;

    private float smoothTime = 0.04f;
    private Vector3 velocity = Vector3.zero;
    private float yPosition;

    private int targetIndex;
    private int currentIndex;

    private bool selected;
    private bool floating = true;
    private float amplitude = 0.01f;

    Gamepad gp;

    // Start is called before the first frame update
    void Start() {
        currentIndex = -1;
        yPosition = transform.position.y;
        transform.position = new Vector3(-9f, yPosition, 0f);
        gp = Gamepad.all[playerIndex];
    }

    // Update is called once per frame
    void Update() {
        /*if (floating)
            Floating();*/

        if (selected)
        {
            if (!floating)
                Move(-1);
            else
            {
                Floating();
            }
            return;
        }

        if (isMoving)
        {
            floating = false;
            Move(targetIndex);
            return;
        }
        
        float threshold = 0.21f;
        float h_val = Mathf.Abs(gp.leftStick.x.ReadValue()) < threshold ? 0 : gp.leftStick.x.ReadValue();
        float right_h_val = Mathf.Abs(gp.rightStick.x.ReadValue()) < threshold ? 0 : gp.rightStick.x.ReadValue();

        float horizontal_val = Mathf.Abs(h_val) > Mathf.Abs(right_h_val) ? h_val : right_h_val;

        if (horizontal_val > 0)
        {
            if (currentIndex == -1)
                targetIndex = 0;
            else
                targetIndex = (currentIndex + 1) % 4;
            while (PlayerIndexAssignment.instance.robotSelected[targetIndex])
            {
                targetIndex = (targetIndex + 1) % 4;
            }
            Move(targetIndex);
        }

        else if (horizontal_val < 0)
        {
            if (currentIndex == -1 || currentIndex == 0)
            {
                targetIndex = 3;
            }
            else
                targetIndex = (currentIndex - 1) % 4;
            while (PlayerIndexAssignment.instance.robotSelected[targetIndex])
            {
                targetIndex = (targetIndex - 1) % 4;
            }

            Move(targetIndex);
        }

        else if (gp.aButton.isPressed && currentIndex != -1)
        {
            if (!PlayerIndexAssignment.instance.robotSelected[currentIndex])
            {
                PlayerIndexAssignment.instance.indices[currentIndex] = playerIndex;
                PlayerIndexAssignment.instance.robotSelected[currentIndex] = true;
                //GetComponent<SpriteRenderer>().color = GetColor(currentIndex);
                selected = true;
                Move(-1);
            }
        }

        /*if (gp.aButton.isPressed)
        {
            if (!PlayerIndexAssignment.instance.robotSelected[currentIndex])
            {
                PlayerIndexAssignment.instance.indices[currentIndex] = playerIndex;
                PlayerIndexAssignment.instance.robotSelected[currentIndex] = true;
                //GetComponent<SpriteRenderer>().color = GetColor(currentIndex);
                selected = true;
                Move(-1);
            }
        }*/

    }

    void Move(int targetIndex) {
        isMoving = true;
        Vector3 targetPosition = Vector3.zero;
        if (targetIndex == 0) targetPosition = new Vector3(-7.5f, yPosition, 0f);
        else if (targetIndex == 1) targetPosition = new Vector3(-2.5f, yPosition, 0f);
        else if (targetIndex == 2) targetPosition = new Vector3(2.5f, yPosition, 0f);
        else if (targetIndex == 3) targetPosition = new Vector3(7.5f, yPosition, 0f);
        else targetPosition = new Vector3(transform.position.x, 2.75f, 0);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        if (Vector3.Magnitude(transform.position - targetPosition) < 0.0001f)
        {
            isMoving = false;
            currentIndex = targetIndex;
            transform.position = targetPosition;
            if (targetIndex == -1)
                floating = true;
        }
    }

    //Vector4 GetColor(int index) {
    //    if (index == 0)
    //        return new Vector4(1, 0.286f, 0.463f, 1);
    //    else if (index == 1)
    //        return new Vector4(1, 0.29f, 0.906f, 1);
    //    else if (index == 2)
    //        return new Vector4(0.29f, 1, 0.757f, 1);
    //    else
    //        return new Vector4(0.345f, 0.518f, 1, 1);
    //}

    void Floating()
    {
        Vector3 tempPos = transform.position;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 1) * amplitude;
        transform.position = tempPos;
    }
}
