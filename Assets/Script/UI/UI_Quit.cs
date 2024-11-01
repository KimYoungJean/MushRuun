using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Quit : MonoBehaviour
{
    public void Quit()
    {
        //ºÎ¸ð¸¦ ²û
        transform.parent.gameObject.SetActive(false);
    }
}
