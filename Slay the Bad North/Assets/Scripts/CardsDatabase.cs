using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsDatabase : MonoBehaviour
{
    public static List<Card> cardDb = new List<Card>();
    
    void Start()
    {
        cardDb.Add(new Card(0, "Archers", "Unit of Archers", 8, 2, Card.CardType.Unit, GameObject.Find("Card1")));
        cardDb.Add(new Card(1, "Knights", "Unit of Knights", 5, 8, Card.CardType.Unit, GameObject.Find("Card2")));
        cardDb.Add(new Card(2, "Lancers", "Unit of Lancers", 10, 6, Card.CardType.Unit, GameObject.Find("Card1")));
        cardDb.Add(new Card(3, "Mages", "Unit of Mages", 12, 3, Card.CardType.Unit, GameObject.Find("Card2")));
    }


}
