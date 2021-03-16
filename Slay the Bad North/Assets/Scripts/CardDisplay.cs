using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    [SerializeField] Image cardImage;

    void Start() {
        cardImage.sprite =  card.cardImg;
    }
}
