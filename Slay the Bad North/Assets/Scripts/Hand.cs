using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<Card> playerHand;
    public GameObject PHand;
    float draw_time;

    [SerializeField]
    BattleController controller;
    
    void Start()
    {
        draw_time = Time.time + 10.0f;
        for (int i = 0; i < 5; i++)
        {
            //pedir cartas ao deck em vez de ir buscar por index
            playerHand.Add(GameManager.Instance.deck.deck[i]);
        }
        foreach (var card in playerHand)
        {
            GameObject drawnCard = Instantiate(card.cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            drawnCard.GetComponent<CardUse>().hand = this;
            drawnCard.GetComponent<CardUse>().card = card;
            drawnCard.GetComponent<CardUse>().controller = controller;
            drawnCard.transform.SetParent(PHand.transform, false);
        }
    }

    public void RemoveCard(Card card){
        playerHand.Remove(card);
    }

    void Update()
    {
        if (Time.time > draw_time)
        {
            draw_time = Time.time + 10.0f;
            if (playerHand.Count < 10)
            {
                //pedir cartas ao deck em vez de ir buscar por index
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
                drawnCard.GetComponent<CardUse>().controller = controller;
                drawnCard.transform.SetParent(PHand.transform, false);
            }
            
        }
        
    }
}
