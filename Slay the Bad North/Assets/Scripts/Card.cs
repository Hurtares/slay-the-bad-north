using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardType
{
    Unit,
    Magic,
    Passive
}
[CreateAssetMenu(fileName = "NewCard",menuName = "Card")]
public class Card : ScriptableObject
{
    public int cardID;
    public string cardName;
    public int cardAttack;
    public int cardDefense;
    public string cardText;
    public Sprite cardImg;
    public CardType cardType;
    public GameObject unitPrefab;
     
}
