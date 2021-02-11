using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleTree : MonoBehaviour
{
    [SerializeField] BattleNode battleNodePrefab;
    List<BattleNode> battleNodes;
    void Start()
    {
        battleNodes = new List<BattleNode>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateBattleTree()
    {

        for (int i = 0; i < 11; i++)
        {
            var battleNode = Instantiate(battleNodePrefab, this.transform);
            battleNode.prevNode = new List<BattleNode>();
            if (i > 5 && i <= 8)
            {
                battleNode.nodeLevel = i - 5;
                battleNode.nodeLayer = 1;
            }
            else if (i > 8)
            {
                battleNode.nodeLevel = i - 8;
                battleNode.nodeLayer = -1;
            }
            else
            {
                battleNode.nodeLevel = i;
                battleNode.nodeLayer = 0;
            }

            battleNode.completed = false;
            battleNodes.Add(battleNode);

            if (i != 0)
            {
                //se for o primeiro node de um dos ramos
                if (battleNode.nodeLayer != 0 && battleNodes[i - 1].nodeLayer != battleNode.nodeLayer)
                {
                    //se nao for o primeiro ramo fecha o ramo anterior
                    if (battleNodes[i - 1].nodeLayer != battleNode.nodeLayer && battleNodes[i - 1].nodeLayer != 0)
                    {
                        battleNodes.First(n => n.nodeLayer == 0 && n.nodeLevel == battleNodes[i - 1].nodeLevel + 1).prevNode.Add(battleNodes[i - 1]);
                    }
                    battleNode.prevNode.Add(battleNodes.First(n => n.nodeLayer == 0 && n.nodeLevel == battleNode.nodeLevel - 1));
                }
                battleNode.prevNode.Add(battleNodes[i - 1]);
            }
        }

        battleNodes.ForEach(n => PlaceBattleNode(n));
    }

    void PlaceBattleNode(BattleNode node)
    {
        node.transform.position = new Vector3(node.transform.position.x + (node.nodeLayer * 55), (node.nodeLevel * 55) + 25, node.transform.position.z);
    }
}