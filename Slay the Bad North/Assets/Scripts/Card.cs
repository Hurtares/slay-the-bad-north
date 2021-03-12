using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public int cardID;
    public string cardName;
    public int cardAttack;
    public int cardDefense;
    public string cardText;
    public Texture2D cardImg;
    public CardType cardType;
    public GameObject cardPrefab;
    public enum CardType
    {
        Unit,
        Magic,
        Passive
    }

    public Card (int id, string name, string text, int attack, int defense, CardType type, GameObject prefab)
    {
        cardID = id;
        cardName = name;
        cardText = text;
        cardAttack = attack;
        cardDefense = defense;
        cardType = type;
        cardPrefab = prefab;
    }
}
