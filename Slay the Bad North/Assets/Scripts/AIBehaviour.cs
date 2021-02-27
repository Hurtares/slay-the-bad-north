using UnityEngine;
using UnityEngine.AI;

namespace UnitAI
{
    public class AIBehaviour
    {
        public virtual void Start(UnitNavigation nav) { }
        public virtual void Update(UnitNavigation nav) { }
    }

    public class IdleBehaviour : AIBehaviour
    {
        float idleTime = float.MinValue;
        public override void Start(UnitNavigation nav)
        {
            idleTime = Time.time;
        }
        public override void Update(UnitNavigation nav)
        {

            if (Time.time > idleTime + 5)
            {
                nav.setActiveAIBehavior<PatrolBehaviour>();
            }

            int layerMask = 1 << 9;
            var collider = Physics.OverlapSphere(nav.transform.position, 2f, layerMask);

            Collider possibleEnemy = null;
            float minorDistance = -1f;

            foreach (var enemy in collider)
            {
                float dist = Vector3.Distance(enemy.transform.position, nav.transform.position);
                if (dist < minorDistance || minorDistance == -1)
                {
                    minorDistance = dist;
                    possibleEnemy = enemy;
                }
            }

            if (possibleEnemy)
            {
                nav.setUnitToAttack(possibleEnemy.gameObject);
                nav.setActiveAIBehavior<AttackBehaviour>();
            }
        }
    }

    public class PatrolBehaviour : AIBehaviour
    {
        public GameObject[] patrolPoints;
        private int destPoint = 0;
        private NavMeshAgent agent;


        public override void Start(UnitNavigation nav)
        {
            patrolPoints = nav.controller.patrolPoints;
            agent = nav.GetComponent<NavMeshAgent>();
            GotoNextPoint();
        }
        public override void Update(UnitNavigation nav)
        {

            int layerMask = 1 << 9;
            var collider = Physics.OverlapSphere(nav.transform.position, 2f, layerMask);

            Collider possibleEnemy = null;
            float minorDistance = -1f;

            foreach (var enemy in collider)
            {
                float dist = Vector3.Distance(enemy.transform.position, nav.transform.position);
                if (dist < minorDistance || minorDistance == -1)
                {
                    minorDistance = dist;
                    possibleEnemy = enemy;
                }
            }

            if (possibleEnemy)
            {
                nav.setUnitToAttack(possibleEnemy.gameObject);
                nav.setActiveAIBehavior<AttackBehaviour>();
            }

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }


        void GotoNextPoint()
        {
            if (patrolPoints.Length == 0)
                return;

            agent.destination = patrolPoints[destPoint].transform.position;
            destPoint = UnityEngine.Random.Range(0, patrolPoints.Length);
        }
    }


    public class AttackBehaviour : AIBehaviour
    {
        private NavMeshAgent agent;
        float attackSpeed = 1f;
        float lastAttack = float.MinValue;
    

        public override void Start(UnitNavigation nav)
        {
            agent = nav.GetComponent<NavMeshAgent>();
            GameObject unit = nav.getUnitToAttack();
            if (unit)
            {
                agent.destination = unit.transform.position;
            }
            else
            {
                nav.setActiveAIBehavior<PatrolBehaviour>();
            }
        }
        public override void Update(UnitNavigation nav)
        {
            GameObject unit = nav.getUnitToAttack();
            if (unit)
            {
                if (!agent.pathPending && agent.remainingDistance > 2f)
                {
                    agent.destination = unit.transform.position;
                } else {
                    var dist = Vector3.Distance(unit.transform.position, nav.transform.position);

                    if(dist > 2f) {
                        agent.destination = unit.transform.position;
                    } else {            
                        if (Time.time > lastAttack + attackSpeed)
                        {
                            lastAttack = Time.time;
                            var unitNav = nav.getUnitToAttack().GetComponent<UnitNavigation>();
                            unitNav.removeLife(0.1f);
                        }
                    }

                }

            }
            else
            {
                nav.setActiveAIBehavior<PatrolBehaviour>();
            }
        }

    }

    public class FleeBehaviour : AIBehaviour
    {

        public override void Start(UnitNavigation nav) { }
        public override void Update(UnitNavigation nav)
        {

        }
    }
}