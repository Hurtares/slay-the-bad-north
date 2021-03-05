using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public List<Cards> collection = new List<Cards>();
    void Start()
    {
        collection.Add(new Cards("Archers", 0, "Unit of Archers", 8, 2, Cards.CardType.Unit,GameObject.Find("Card1")));
        collection.Add(new Cards("Knights", 1, "Unit of Knights", 5, 8, Cards.CardType.Unit,GameObject.Find("Card2")));
    }

    
}
