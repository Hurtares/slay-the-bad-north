using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUse : MonoBehaviour
{
    public GameObject Card;
    public GameObject Unit;
    bool isSelected = false;
    static bool isUnique = false;
    
    [SerializeField]
    public BattleController controller;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Floor")
                {
                    if (isSelected)
                    {
                        var playerUnit = Instantiate(Unit, hit.point, Quaternion.identity);
                        playerUnit.tag = "Unit";
                        playerUnit.layer = 9;
                        var unitNav = playerUnit.GetComponent<UnitNavigation>();
                        unitNav.controller = controller;
                        controller.playerUnits.Add(playerUnit);
                        Destroy (Card);
                        Hand.numCards--;
                        isSelected = false;
                        isUnique = false;
                    }
                }
            }
        }

    }
    public void OnClick()
    {
        if (!isSelected && !isUnique)
        {
            Card.transform.localScale += new Vector3(0.2f, 0.2f, 0);
            isSelected = true;
            isUnique = true;
        }

        else
        {
                if (isSelected)
                {
                    Card.transform.localScale -= new Vector3(0.2f, 0.2f, 0);
                    isSelected = false;
                    isUnique = false;
                }
        }
    }
}
