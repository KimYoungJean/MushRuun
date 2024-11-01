using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Input : MonoBehaviour
{
    public void Clear()
    {
        
        Debug.Log($"{gameObject.name} Clear");
        gameObject.GetComponent<TMP_InputField>().text = "";
    }

    public void Enter()
    {
        AudioManager.instance.PlaySound(AudioManager.instance.audioClips[3]);
    }
}
