using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public GameObject[] patrolPoints;
    GameObject[] spawnPoints;
    GameObject[] playerSpawnPoint;

    List<GameObject> playerUnits;
    List<GameObject> aiUnits;

    public GameObject unitPrefab;
    public GameObject selectedUnit;


    // SELECTION BOX
    [SerializeField]
    RectTransform selectionRect;
    //   The start and end coordinates of the square we are making
    Vector3 squareStartPos;
    Vector3 squareEndPos;
    //To determine if we are clicking with left mouse or holding down left mouse
    float delay = 0.3f;
    float clickTime = 0f;
    
    void Start()
    {
        patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");

        playerUnits = new List<GameObject>();
        aiUnits = new List<GameObject>();

        findAllSpawnPoints();

        spawnWave();
        spawnPlayerUnit();
    }

    void Update()
    {
        //Are we clicking with left mouse or holding down left mouse
        bool isClicking = false;
        bool isHoldingDown = false;

        //Click the mouse button
        if (Input.GetMouseButtonDown(0))
        {
            clickTime = Time.time;

            //We dont yet know if we are drawing a square, but we need the first coordinate in case we do draw a square
            RaycastHit hit;
            //Fire ray from camera
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200f, 1 << 8))
            {
                //The corner position of the square
                squareStartPos = hit.point;
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            var changedSelectedUnit = SelectOnClick();
            if (!changedSelectedUnit && selectedUnit)
            {
                selectedUnit.GetComponent<Outline>().enabled = false;
                selectedUnit = null;
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedUnit)
        {
            bool hasAttack = AttackUnit();
            if (!hasAttack)
            {
                MoveOnClick();
            }
        }



        if (Input.GetMouseButtonDown(0))
        {
            squareEndPos = Input.mousePosition;
        }


        displaySelectionBox();
    }

    void displaySelectionBox()
    {
        //The start position of the square is in 3d space, or the first coordinate will move
        //as we move the camera which is not what we want
        Vector3 squareStartScreen = Camera.main.WorldToScreenPoint(squareStartPos);

        squareStartScreen.z = 0f;

        //Get the middle position of the square
        Vector3 middle = (squareStartScreen + squareEndPos) / 2f;

        //Set the middle position of the GUI square
        selectionRect.position = middle;
    }

    bool AttackUnit()
    {
        var unitNav = selectedUnit.GetComponent<UnitNavigation>();
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();
        int layerMask = 1 << 10;

        if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
        {
            unitNav.setUnitToAttack(hit.collider.gameObject);
            return true;
        }
        return false;
    }
    void MoveOnClick()
    {
        var unitNav = selectedUnit.GetComponent<UnitNavigation>();
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();
        int layerMask = 1 << 8;

        if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
        {
            unitNav.goal = hit.point;
            unitNav.setUnitToAttack(null);
            unitNav.MoveTo();
        }
    }

    bool SelectOnClick()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();
        int layerMask = 1 << 9;

        if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
        {
            if (selectedUnit)
            {
                selectedUnit.GetComponent<Outline>().enabled = false;
            }

            selectedUnit = hit.collider.gameObject;
            selectedUnit.GetComponent<Outline>().enabled = true;

            return true;
        }

        return false;
    }

    void findAllSpawnPoints()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        playerSpawnPoint = GameObject.FindGameObjectsWithTag("PlayerSpawn");
    }

    void spawnWave()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            var aiUnit = Instantiate(unitPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            var unitNav = aiUnit.GetComponent<UnitNavigation>();
            aiUnit.layer = 10;
            unitNav.isAI = true;
            unitNav.controller = this;
            playerUnits.Add(aiUnit);
        }
    }

    void spawnPlayerUnit()
    {
        foreach (var playerSpawnPoint in playerSpawnPoint)
        {
            var playerUnit = Instantiate(unitPrefab, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
            playerUnit.tag = "Unit";
            playerUnit.layer = 9;
            playerUnits.Add(playerUnit);
        }
    }
}
