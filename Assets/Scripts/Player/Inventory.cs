using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    
    public int numOfBlueTeamResource = 0;
    public int numOfRedTeamResource = 0;
    public int[] numOfPlayerResource;
    
    void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

}
