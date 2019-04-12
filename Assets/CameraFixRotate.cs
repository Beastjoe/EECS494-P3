using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixRotate : MonoBehaviour
{
    public Transform target; //This will be your citizen
    public Vector3 diffVec;

    private void Start() {
        //target = gameObject.transform.parent;
        diffVec = transform.position - target.position;
    }

    void Update() {
        transform.position = target.position + diffVec;
    }
}
