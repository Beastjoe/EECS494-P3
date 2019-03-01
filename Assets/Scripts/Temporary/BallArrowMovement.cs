using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallArrowMovement : MonoBehaviour{
    Rigidbody rb;
    public float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        rb.AddForce(new Vector3(horizontal, 0, vertical) * speed);
    }

    /* COPY FROM HERE */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cuboid"))
        {
            // TODO: add cuboid to inventory
            Destroy(other.gameObject);
        }
    }

    /* Relative static transform to outer ground */
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //        PlayerController.instance.transform.parent = transform;
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //        PlayerController.instance.transform.parent = null;
    //}

    /* COPY END HERE */
}
