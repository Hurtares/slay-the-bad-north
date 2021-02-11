using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, ISelectHandler , IPointerEnterHandler
{

    public AudioSource audioSource;
    public AudioClip soundHighlight;
    public AudioClip soundClick;
    
     public void OnPointerEnter(PointerEventData eventData)
     {
        audioSource.PlayOneShot(soundHighlight, 1.0f);
     }

     public void OnSelect(BaseEventData eventData)
     {
         audioSource.PlayOneShot(soundClick, 1.0f);   
     }
}