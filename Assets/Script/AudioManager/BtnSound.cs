using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnSound : MonoBehaviour,IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound(AudioManager.instance.audioClips[1]);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound(AudioManager.instance.audioClips[2]);
    }
}

