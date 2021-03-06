using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public float Sound = 100;
    public Race SelectedRace = Race.HUMAN;
    public static GameManager Instance;
    public BattleTree battleTree;
    public BattleNode currentNode;

    private void Awake() {
        if (GameManager.Instance != null)
        {
            GameObject.Destroy(this);
        }else{
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
