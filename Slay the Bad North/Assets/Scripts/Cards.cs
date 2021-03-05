using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cards
{
    public string cardName;
    public int cardID;
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

    public Cards(string name, int id, string text, int attack, int defense, CardType type, GameObject prefab)
    {
        cardName = name;
        cardID = id;
        cardText = text;
        cardAttack = attack;
        cardDefense = defense;
        cardType = type;
        cardPrefab = prefab;
    }
}
