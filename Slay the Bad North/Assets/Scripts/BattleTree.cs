using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void GenerateBattleTree(){
        
        for (int i = 0; i < 5; i++)
        {
            var battleNode = Instantiate(battleNodePrefab,this.transform);
            battleNode.nextNode = new List<BattleNode>();
            battleNode.nodeLevel = i;
            battleNode.completed = false;
            battleNodes.Add( battleNode );
            if (i!=0)
            {
                battleNodes[i-1].nextNode.Add(battleNode);
            }
        }
        battleNodes.ForEach(n => PlaceBattleNode(n));
    }

    void PlaceBattleNode(BattleNode node){
        node.transform.position = new Vector3(node.transform.position.x,(node.nodeLevel*55)+25,node.transform.position.z);
    }
}