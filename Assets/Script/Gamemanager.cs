using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public static bool isClear=false;
    public static bool portalSpawned = false;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        isClear = false;
    }

    private void Update()
    {
        if (UIManager.time >= 20)
        {
            isClear = true;
        }
    }
}
