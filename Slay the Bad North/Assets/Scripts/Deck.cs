using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public List<Card> deck;
    public int deckSize;

    void RandomizeDeck()
    {
        /*
        int x = 0;
        deckSize = 20;
        for (int i = 0; i < deckSize; i++)
        {
            x = Random.Range(1, 4);
            deck[i] = CardsDatabase.cardDb[x];
        }*/

    }

    public void PopulateDeck(Race race){
        deckSize = 20;
        for (int i = 0; i < deckSize; i++)
        {
            deck[i] = CardsDatabase.cardDb[i%5];
        }
    }

}
