using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class CharacterIndicator : MonoBehaviour
{
    private bool isMoving;
    public int playerIndex;
    
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private float yPosition;

    private int targetIndex;
    private int currenIndex;

    private bool selected;

    Gamepad gp;
    
    // Start is called before the first frame update
    void Start()
    {
        currenIndex = -1;
        yPosition = transform.position.y;
        transform.position = new Vector3(-9f, yPosition, 0f);
        gp = Gamepad.all[playerIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
            return;

        if (isMoving)
        {
            Move(targetIndex);
            return;
        }

        float threshold = 0.21f;
        float h_val = Mathf.Abs(gp.leftStick.x.ReadValue()) < threshold ? 0 : gp.leftStick.x.ReadValue();
        float right_h_val = Mathf.Abs(gp.rightStick.x.ReadValue()) < threshold ? 0 : gp.rightStick.x.ReadValue();

        float horizontal_val = Mathf.Max(h_val, right_h_val);

        if (horizontal_val > 0)
        {
            while (PlayerIndexAssignment.instance.robotSelected[targetIndex])
            {
                targetIndex = (currenIndex + 1) % 4;
            }
            Move(targetIndex);
        }

        else if (horizontal_val < 0)
        {
            if (currenIndex == -1)
            {
                targetIndex = 3;
                Move(targetIndex);
                return;
            }

            while (PlayerIndexAssignment.instance.robotSelected[targetIndex])
            {
                targetIndex = (currenIndex - 1) % 4;
            }

            Move(targetIndex);
        }

        else if (gp.aButton.isPressed && currenIndex != -1)
        {
            if (!PlayerIndexAssignment.instance.robotSelected[currenIndex])
            {
                PlayerIndexAssignment.instance.indices[currenIndex] = playerIndex;
                PlayerIndexAssignment.instance.robotSelected[currenIndex] = true;
                GetComponent<SpriteRenderer>().color = GetColor(currenIndex);
                selected = true;
            }
        }

    }

    void Move(int targetIndex)
    {
        isMoving = true;
        Vector3 targetPosition = Vector3.zero;
        if (targetIndex == 0) targetPosition = new Vector3(-7.5f, yPosition, 0f);
        else if (targetIndex == 1) targetPosition = new Vector3(-2.5f, yPosition, 0f);
        else if (targetIndex == 2) targetPosition = new Vector3(2.5f, yPosition, 0f);
        else targetPosition = new Vector3(7.5f, 4f, 0f);
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        Debug.Log(Vector3.Magnitude(transform.position - targetPosition));
        if (Vector3.Magnitude(transform.position - targetPosition) < 0.0001f)
        {
            //Debug.Log("Inside here");
            isMoving = false;
            currenIndex = targetIndex;
            transform.position = targetPosition;
        }
    }

    Vector4 GetColor(int index)
    {
        if (index == 0)
            return new Vector4(1, 0.286f, 0.463f, 1);
        else if (index == 1)
            return new Vector4(1, 0.29f, 0.906f, 1);
        else if (index == 2)
            return new Vector4(0.29f, 1, 0.757f, 1);
        else
            return new Vector4(0.345f, 0.518f, 1, 1);
    }
}
