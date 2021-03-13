using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    //public List<Cards> playerHand = new List<Cards>();
    //public Cards Card1;
    //public Cards Card2;
    public List<Card> playerHand;
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PHand;
    float draw_time;
    public static int numCards = 5;

    [SerializeField]
    BattleController controller;
    
    void Start()
    {
        CardUse cardUse1 = Card1.GetComponent<CardUse>();
        cardUse1.controller = controller;
        CardUse cardUse2 = Card2.GetComponent<CardUse>();
        cardUse2.controller = controller;


        draw_time = Time.time + 10.0f;
        for (int i = 0; i < 5; i++)
        {
            playerHand.Add(GameManager.Instance.deck.deck[i]);
        }
        foreach (var card in playerHand)
        {
            GameObject drawnCard = Instantiate(card.cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            drawnCard.GetComponent<CardUse>().hand = this;
            drawnCard.GetComponent<CardUse>().card = card;
            drawnCard.transform.SetParent(PHand.transform, false);
        }
    }

    public void RemoveCard(Card card){
        //faz coisas
        playerHand.Remove(card);
    }

    void Update()
    {
        if (Time.time > draw_time)
        {
            draw_time = Time.time + 10.0f;
            if (playerHand.Count < 10)
            {
                playerHand.Add(GameManager.Instance.deck.deck[5]);
            }
            foreach (Transform child in PHand.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (var card in playerHand)
            {
                GameObject drawnCard = Instantiate(card.cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                drawnCard.GetComponent<CardUse>().hand = this;
                drawnCard.GetComponent<CardUse>().card = card;
                drawnCard.transform.SetParent(PHand.transform, false);
            }
            
        }
        
    }
}
