using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingDirectionSpriteController : MonoBehaviour {

    SpriteRenderer rend;
    ArrowKeyMovement am;
    Material originalMaterial;
    public Material whiteMaterial;
    float deltaTime = 0f;
    float zpos = 0f;

    public Sprite[] chargingSprites;
    // Start is called before the first frame update
    void Start() {
        rend = GetComponent<SpriteRenderer>();
        am = transform.parent.gameObject.GetComponent<ArrowKeyMovement>().teamMember.GetComponent<ArrowKeyMovement>();
        originalMaterial = rend.material;
    }

    // Update is called once per frame
    void Update() {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1.4f + Mathf.PingPong(Time.time*0.5f + +am.chargingAmount, 0.2f));
        if (am.chargingAmount<0.2f)
        {
            deltaTime = 0;
            transform.localScale = new Vector3(0.05f, 0.1f, 1);
            rend.sprite = chargingSprites[0];
            rend.material = originalMaterial;
        } else if (am.chargingAmount<0.3f)
        {
            rend.sprite = chargingSprites[1];
        } else if (am.chargingAmount<0.6f)
        {
            rend.sprite = chargingSprites[2];
        } else
        {
            deltaTime += Time.deltaTime;
            transform.localScale = new Vector3(0.05f + Mathf.PingPong(Time.time * 0.5f + am.chargingAmount, 0.03f), 0.05f + Mathf.PingPong(Time.time * 0.5f + am.chargingAmount, 0.06f), 1);
            if (deltaTime<0.1f)
            {
                rend.material = whiteMaterial;
            } else if (deltaTime<0.2f)
            {
                rend.material = originalMaterial;
            } else if (deltaTime>=0.2f)
            {
                deltaTime = 0;
            }

        }
    }
}
