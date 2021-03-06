using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    //public List<Cards> playerHand = new List<Cards>();
    //public Cards Card1;
    //public Cards Card2;
    public List<GameObject> playerHand = new List<GameObject>();
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PHand;
    float draw_time;
    public static int numCards = 5;
    void Start()
    {
        draw_time = Time.time + 10.0f;
        playerHand.Add(Card1);
        playerHand.Add(Card2);
        for (var i = 0; i < 5; i++)
        {
            GameObject drawnCard = Instantiate(playerHand[Random.Range(0, playerHand.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            drawnCard.transform.SetParent(PHand.transform, false);
        }
    }

    void Update()
    {
        if (Time.time > draw_time)
        {
            draw_time = Time.time + 10.0f;
            if (numCards < 10)
            {
                GameObject drawnCard = Instantiate(playerHand[Random.Range(0, playerHand.Count)], new Vector3(0, 0, 0), Quaternion.identity);
                drawnCard.transform.SetParent(PHand.transform, false);
                numCards++;
            }
            
        }
        
    }
}
