using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{
    public GameObject[] patrolPoints;
    GameObject[] spawnPoints;
    GameObject[] playerSpawnPoint;

    public List<GameObject> playerUnits;
    List<GameObject> aiUnits;

    public GameObject unitPrefab;
    public GameObject enemyUnit;
    public List<GameObject> selectedUnits = new List<GameObject>();

    [SerializeField]
    GameObject diamondObject = null;

    // SELECTION BOX
    [SerializeField]
    RectTransform selectionRect;

    //We have hovered above this unit, so we can deselect it next update
    //and dont have to loop through all units
    GameObject highlightThisUnit;

    //   The start and end coordinates of the square we are making
    Vector3 squareStartPos;
    Vector3 squareEndPos;
    //To determine if we are clicking with left mouse or holding down left mouse
    float delay = 0.3f;
    float clickTime = 0f;
    bool hasCreatedSquare;
    Vector3 TL, TR, BL, BR;

    void Start()
    {

        selectionRect.gameObject.SetActive(false);

        patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");

        playerUnits = new List<GameObject>();
        aiUnits = new List<GameObject>();

        findAllSpawnPoints();

        spawnWave();
    }

    void Update()
    {

        var enemies = getPlayerUnitsAI();
        if (enemies.Count == 0)
        {
            changeSceneWin();
        }

        if (diamondObject.GetComponent<DiamondController>().health <= 0)
        {
            changeSceneLose();
        }

        /*
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

       


*/
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
        //Release the mouse button
        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - clickTime <= delay)
            {
                isClicking = true;
            }

            //Select all units within the square if we have created a square
            if (hasCreatedSquare)
            {
                hasCreatedSquare = false;

                //Deactivate the square selection image
                selectionRect.gameObject.SetActive(false);

                //Clear the list with selected unit
                selectedUnits.Clear();

                //Select the units
                for (int i = 0; i < playerUnits.Count; i++)
                {
                    GameObject currentUnit = playerUnits[i];

                    if (currentUnit)
                    {
                        //Is this unit within the square
                        if (IsWithinPolygon(currentUnit.transform.position))
                        {
                            currentUnit.GetComponent<Outline>().enabled = true;
                            selectedUnits.Add(currentUnit);
                        }
                        //Otherwise deselect the unit if it's not in the square
                        else
                        {
                            currentUnit.GetComponent<Outline>().enabled = false;
                        }
                    }
                }
            }
        }

        // RIGHT CLICK
        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            bool hasAttack = AttackUnit();
            if (!hasAttack)
            {
                MoveOnClick();
            }
        }
        //Holding down the mouse button
        if (Input.GetMouseButton(0))
        {
            if (Time.time - clickTime > delay)
            {
                isHoldingDown = true;
            }
        }

        //Select one unit with left mouse and deselect all units with left mouse by clicking on what's not a unit
        if (isClicking)
        {
            //Deselect all units
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (selectedUnits[i])
                {
                    selectedUnits[i].GetComponent<Outline>().enabled = false;
                }
            }

            //Clear the list with selected units
            selectedUnits.Clear();

            //Try to select a new unit
            RaycastHit hit;
            int layerMask = 1 << 9;
            //Fire ray from camera
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200f, layerMask))
            {
                GameObject activeUnit = hit.collider.gameObject;
                //Set this unit to selected
                activeUnit.GetComponent<Outline>().enabled = true;
                //Add it to the list of selected units, which is now just 1 unit
                selectedUnits.Add(activeUnit);
            }
        }

        //Drag the mouse to select all units within the square
        if (isHoldingDown && selectionRect)
        {
            //Activate the square selection image
            if (!selectionRect.gameObject.activeInHierarchy)
            {
                selectionRect.gameObject.SetActive(true);
            }

            //Get the latest coordinate of the square
            squareEndPos = Input.mousePosition;

            //Display the selection with a GUI image
            DisplaySquare();

            //Highlight the units within the selection square, but don't select the units
            if (hasCreatedSquare)
            {
                for (int i = 0; i < playerUnits.Count; i++)
                {
                    GameObject currentUnit = playerUnits[i];

                    if (currentUnit)
                    {
                        //Is this unit within the square
                        if (IsWithinPolygon(currentUnit.transform.position))
                        {
                            currentUnit.GetComponent<Outline>().enabled = true;
                        }
                        //Otherwise deactivate
                        else
                        {
                            currentUnit.GetComponent<Outline>().enabled = false;
                        }
                    }
                }
            }
        }
    }

    //Display the selection with a GUI square
    void DisplaySquare()
    {
        //The start position of the square is in 3d space, or the first coordinate will move
        //as we move the camera which is not what we want
        Vector3 squareStartScreen = Camera.main.WorldToScreenPoint(squareStartPos);

        squareStartScreen.z = 0f;

        //Get the middle position of the square
        Vector3 middle = (squareStartScreen + squareEndPos) / 2f;

        //Set the middle position of the GUI square
        selectionRect.position = middle;

        //Change the size of the square
        float sizeX = Mathf.Abs(squareStartScreen.x - squareEndPos.x);
        float sizeY = Mathf.Abs(squareStartScreen.y - squareEndPos.y);

        //Set the size of the square
        selectionRect.sizeDelta = new Vector2(sizeX, sizeY);

        //The problem is that the corners in the 2d square is not the same as in 3d space
        //To get corners, we have to fire a ray from the screen
        //We have 2 of the corner positions, but we don't know which,  
        //so we can figure it out or fire 4 raycasts
        TL = new Vector3(middle.x - sizeX / 2f, middle.y + sizeY / 2f, 0f);
        TR = new Vector3(middle.x + sizeX / 2f, middle.y + sizeY / 2f, 0f);
        BL = new Vector3(middle.x - sizeX / 2f, middle.y - sizeY / 2f, 0f);
        BR = new Vector3(middle.x + sizeX / 2f, middle.y - sizeY / 2f, 0f);

        //From screen to world
        RaycastHit hit;
        int i = 0;
        //Fire ray from camera
        if (Physics.Raycast(Camera.main.ScreenPointToRay(TL), out hit, 200f, 1 << 8))
        {
            TL = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(TR), out hit, 200f, 1 << 8))
        {
            TR = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(BL), out hit, 200f, 1 << 8))
        {
            BL = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(BR), out hit, 200f, 1 << 8))
        {
            BR = hit.point;
            i++;
        }

        //Could we create a square?
        hasCreatedSquare = false;

        //We could find 4 points
        if (i == 4)
        {
            //Display the corners for debug
            //sphere1.position = TL;
            //sphere2.position = TR;
            //sphere3.position = BL;
            //sphere4.position = BR;

            hasCreatedSquare = true;
        }
    }

    //Highlight a unit when mouse is above it
    void HighlightUnit()
    {
        //Change material on the latest unit we highlighted
        if (highlightThisUnit != null)
        {
            //But make sure the unit we want to change material on is not selected
            bool isSelected = false;
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (selectedUnits[i] == highlightThisUnit)
                {
                    isSelected = true;
                    break;
                }
            }

            if (!isSelected)
            {
                highlightThisUnit.GetComponent<Outline>().enabled = false;
            }

            highlightThisUnit = null;
        }

        //Fire a ray from the mouse position to get the unit we want to highlight
        RaycastHit hit;
        int layerMask = 1 << 9;
        //Fire ray from camera
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200f, layerMask))
        {
            //Get the object we hit
            GameObject currentObj = hit.collider.gameObject;

            //Highlight this unit if it's not selected
            bool isSelected = false;
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (selectedUnits[i] == currentObj)
                {
                    isSelected = true;
                    break;
                }
            }

            if (!isSelected)
            {
                highlightThisUnit = currentObj;
                highlightThisUnit.GetComponent<Outline>().enabled = true;
            }
        }
    }

    //Is a unit within a polygon determined by 4 corners
    bool IsWithinPolygon(Vector3 unitPos)
    {
        bool isWithinPolygon = false;

        //The polygon forms 2 triangles, so we need to check if a point is within any of the triangles
        //Triangle 1: TL - BL - TR
        if (IsWithinTriangle(unitPos, TL, BL, TR))
        {
            return true;
        }

        //Triangle 2: TR - BL - BR
        if (IsWithinTriangle(unitPos, TR, BL, BR))
        {
            return true;
        }

        return isWithinPolygon;
    }

    //Is a point within a triangle
    //From http://totologic.blogspot.se/2014/01/accurate-point-in-triangle-test.html
    bool IsWithinTriangle(Vector3 p, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        bool isWithinTriangle = false;

        //Need to set z -> y because of other coordinate system
        float denominator = ((p2.z - p3.z) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.z - p3.z));

        float a = ((p2.z - p3.z) * (p.x - p3.x) + (p3.x - p2.x) * (p.z - p3.z)) / denominator;
        float b = ((p3.z - p1.z) * (p.x - p3.x) + (p1.x - p3.x) * (p.z - p3.z)) / denominator;
        float c = 1 - a - b;

        //The point is within the triangle if 0 <= a <= 1 and 0 <= b <= 1 and 0 <= c <= 1
        if (a >= 0f && a <= 1f && b >= 0f && b <= 1f && c >= 0f && c <= 1f)
        {
            isWithinTriangle = true;
        }

        return isWithinTriangle;
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

        //Change the size of the square
        float sizeX = Mathf.Abs(squareStartScreen.x - squareEndPos.x);
        float sizeY = Mathf.Abs(squareStartScreen.y - squareEndPos.y);

        //Set the size of the square
        selectionRect.sizeDelta = new Vector2(sizeX, sizeY);

        //The problem is that the corners in the 2d square is not the same as in 3d space
        //To get corners, we have to fire a ray from the screen
        //We have 2 of the corner positions, but we don't know which,  
        //so we can figure it out or fire 4 raycasts
        TL = new Vector3(middle.x - sizeX / 2f, middle.y + sizeY / 2f, 0f);
        TR = new Vector3(middle.x + sizeX / 2f, middle.y + sizeY / 2f, 0f);
        BL = new Vector3(middle.x - sizeX / 2f, middle.y - sizeY / 2f, 0f);
        BR = new Vector3(middle.x + sizeX / 2f, middle.y - sizeY / 2f, 0f);

        //From screen to world
        RaycastHit hit;
        int i = 0;
        //Fire ray from camera
        if (Physics.Raycast(Camera.main.ScreenPointToRay(TL), out hit, 200f, 1 << 8))
        {
            TL = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(TR), out hit, 200f, 1 << 8))
        {
            TR = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(BL), out hit, 200f, 1 << 8))
        {
            BL = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(BR), out hit, 200f, 1 << 8))
        {
            BR = hit.point;
            i++;
        }

        //Could we create a square?
        hasCreatedSquare = false;

        //We could find 4 points
        if (i == 4)
        {
            //Display the corners for debug
            //sphere1.position = TL;
            //sphere2.position = TR;
            //sphere3.position = BL;
            //sphere4.position = BR;

            hasCreatedSquare = true;
        }
    }

    bool AttackUnit()
    {

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        int layerMask = 1 << 10;

        if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
        {
            foreach (var selectedUnit in selectedUnits)
            {
                if (selectedUnit)
                {
                    var unitNav = selectedUnit.GetComponent<UnitNavigation>();
                    unitNav.setUnitToAttack(hit.collider.gameObject);
                }
            }
            return true;
        }
        return false;
    }
    void MoveOnClick()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();
        int layerMask = 1 << 8;

        if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
        {
            foreach (var selectedUnit in selectedUnits)
            {
                if (selectedUnit)
                {
                    var unitNav = selectedUnit.GetComponent<UnitNavigation>();
                    unitNav.goal = hit.point;
                    unitNav.setUnitToAttack(null);
                    unitNav.MoveTo();
                }
            }
        }
    }

    /*
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
        }*/

    void findAllSpawnPoints()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        playerSpawnPoint = GameObject.FindGameObjectsWithTag("PlayerSpawn");
    }

    void spawnWave()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            var aiUnit = Instantiate(enemyUnit, spawnPoint.transform.position, spawnPoint.transform.rotation);
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
            var unitNav = playerUnit.GetComponent<UnitNavigation>();
            playerUnit.tag = "Unit";
            playerUnit.layer = 9;
            unitNav.controller = this;
            playerUnits.Add(playerUnit);
        }
    }

    public GameObject getDiamondObject()
    {
        return diamondObject;
    }

    public List<GameObject> getPlayerUnitsAI()
    {
        List<GameObject> filtered = new List<GameObject>();

        foreach (var pu in playerUnits)
        {
            var unitNav = pu.GetComponent<UnitNavigation>();
            if (unitNav.isAI)
            {
                filtered.Add(pu);
            }
        }
        return filtered;
    }

    public void changeSceneWin()
    {
        GameManager.Instance.currentNode.state = NodeState.Completed;
        SceneManager.LoadScene("BrunoTestScene", LoadSceneMode.Single);
    }

    public void changeSceneLose()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
