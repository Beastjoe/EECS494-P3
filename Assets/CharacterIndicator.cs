using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIndicator : MonoBehaviour
{
    private bool isMoving;
    public int OriginalIndex;

    private float tmpCD = 1f;
    private float tmpCDcount = 1f;
    
    
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private int targetIndex;
    private int currenIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        // TODO: set the original position according to the OriginalIndex
        tmpCDcount = tmpCD;
    }

    // Update is called once per frame
    void Update()
    {
        // This is for experiment
        // DELETE HERE
        tmpCDcount -= Time.deltaTime;
        if (tmpCDcount > 0)
        {
            return;
        }

        isMoving = true;
        targetIndex = 2;
        Move(targetIndex);
        
        
        if (isMoving)
        {
            Move(targetIndex);
            return;
        }
        
        
        //TODO: update new index & blala....
        
    }

    void Move(int targetIndex)
    {
        Vector3 targetPosition = Vector3.zero;
        if (targetIndex == 0) targetPosition = new Vector3(-7.5f, 4f, 0f);
        else if (targetIndex == 1) targetPosition = new Vector3(-2.5f, 4f, 0f);
        else if (targetIndex == 2) targetPosition = new Vector3(2.5f, 4f, 0f);
        else targetPosition = new Vector3(7.5f, 4f, 0f);
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        Debug.Log(Vector3.Magnitude(transform.position - targetPosition));
        if (Vector3.Magnitude(transform.position - targetPosition) < 0.0001f)
        {
            Debug.Log("Inside here");
            isMoving = false;
            currenIndex = targetIndex;
            transform.position = targetPosition;
        }
    }

}
