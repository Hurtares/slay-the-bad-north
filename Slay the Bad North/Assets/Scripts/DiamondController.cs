using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondController : MonoBehaviour
{
    float health = 10f;
    public Image healthBar;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

      public void removeLife(float slice)
    {
        health -= slice;

        if (health < 0f)
        {
            Destroy(this.gameObject);
        }

        healthBar.fillAmount = health / 10f;
    }
}
