using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Cards> deck = new List<Cards>();
    public int x;
    public int deckSize;

    void Start()
    {
        x = 0;
        deckSize = 20;
        for (int i = 0; i < deckSize; i++)
        {
            x = Random.Range(1, 4);
            deck[i] = CardsDatabase.cardDb[x];
        }

    }

}
