using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public float Sound = 100;
    public Race SelectedRace = Race.HUMAN;
    public static GameManager instance;
    public BattleTree battleTree;

    private void Awake() {
        if (GameManager.instance != null)
        {
            GameObject.Destroy(this);
        }else{
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
