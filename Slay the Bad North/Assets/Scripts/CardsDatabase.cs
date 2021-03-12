using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsDatabase : MonoBehaviour
{
    public static List<Cards> cardDb = new List<Cards>();
    
    void Start()
    {
        cardDb.Add(new Cards(0, "Archers", "Unit of Archers", 8, 2, Cards.CardType.Unit, GameObject.Find("Card1")));
        cardDb.Add(new Cards(1, "Knights", "Unit of Knights", 5, 8, Cards.CardType.Unit, GameObject.Find("Card2")));
        cardDb.Add(new Cards(2, "Lancers", "Unit of Lancers", 10, 6, Cards.CardType.Unit, GameObject.Find("Card1")));
        cardDb.Add(new Cards(3, "Mages", "Unit of Mages", 12, 3, Cards.CardType.Unit, GameObject.Find("Card2")));
    }


}
