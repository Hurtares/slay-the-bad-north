using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using UnitAI;

public class UnitNavigation : MonoBehaviour
{
    // CENAS QUE VAO VIR DAS CARTAS
    public float health;
    float attackSpeed = 1f;
    // FIM DE CENAS QUE VAO VIR DAS CARTAS

    float lastAttack = float.MinValue;
    public Image healthBar;
    List<UnitAI.AIBehaviour> behaviors = new List<UnitAI.AIBehaviour>();
    UnitAI.AIBehaviour activeBehaviour = null;

    public BattleController controller;
    public Vector3 goal;
    private NavMeshAgent agent;

    private GameObject unitToAttack = null;

    public bool isAI = false;

    [SerializeField]
    Sprite[] healthBarSprites; // 0 - PLAYER; 1 - AI

    // Start is called before the first frame update
    void Start()
    {
        health = 1f;

        goal = gameObject.transform.position;
        agent = GetComponent<NavMeshAgent>();



        if (isAI)
        {
            
            healthBar.sprite = healthBarSprites[1];

            behaviors.Add(
                new IdleBehaviour()
            );
            behaviors.Add(
                new PatrolBehaviour()
            );
            behaviors.Add(
               new AttackBehaviour()
           );
            behaviors.Add(
               new AttackDiamondBehaviour()
           );
            setActiveAIBehavior<IdleBehaviour>();
        } else {
            healthBar.sprite = healthBarSprites[0];
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isAI)
        {
            activeBehaviour.Update(this);
        }
        else
        {
            if (unitToAttack)
            {
                if (!agent.pathPending && agent.remainingDistance > 2f)
                {
                    agent.destination = unitToAttack.transform.position;
                }
                else
                {
                    var dist = Vector3.Distance(unitToAttack.transform.position, transform.position);

                    if (dist > 2f)
                    {
                        agent.destination = unitToAttack.transform.position;
                    }
                    else
                    {       
                        if (Time.time > lastAttack + attackSpeed)
                        {
                            lastAttack = Time.time;
                            var unitNav = unitToAttack.GetComponent<UnitNavigation>();
                            unitNav.removeLife(0.1f);
                        }
                    }
                }
            }


        }


    }

    public void setActiveAIBehavior<T>()
    {
        if (isAI)
        {
            foreach (var behavior in behaviors)
            {
                if (behavior is T)
                {
                    if (activeBehaviour != null)
                    {
                        //activeBehaviour.Stop();
                    }
                    activeBehaviour = behavior;
                    activeBehaviour.Start(this);
                    break;
                }
            }
        }
    }

    public void setUnitToAttack(GameObject unit)
    {

        if (!isAI && unit)
        {
            agent.SetDestination(unit.transform.position);
        }

        unitToAttack = unit;
    }

    public GameObject getUnitToAttack()
    {
        return unitToAttack;
    }

    public void MoveTo()
    {
        agent.destination = goal;
    }

    public void removeLife(float slice)
    {
        health -= slice;

        if (health < 0f)
        {
            Destroy(this.gameObject);
        }

        healthBar.fillAmount = health;
    }
}
