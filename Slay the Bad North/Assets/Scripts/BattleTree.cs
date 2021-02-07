using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTree : MonoBehaviour
{
    [SerializeField] GameObject battleNodePrefab;
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
        battleNodes.Add(new BattleNode( null,0,false ) );
        battleNodes.Add(new BattleNode(new List<BattleNode>(new BattleNode[]{battleNodes[0]}),1,false));
        battleNodes.Add(new BattleNode(new List<BattleNode>(new BattleNode[]{battleNodes[1]}),2,false));
        battleNodes.Add(new BattleNode(new List<BattleNode>(new BattleNode[]{battleNodes[2]}),3,false));

        Debug.Log(battleNodes[0].ToString());
        Debug.Log(battleNodes[1].ToString());
        Debug.Log(battleNodes[2].ToString());
        Debug.Log(battleNodes[3].ToString());
        battleNodes[0].completed = true;
        Debug.Log(battleNodes[0].ToString());
        battleNodes.ForEach( p => PlaceBattleNode(p));
    }

    void PlaceBattleNode(BattleNode node){
        var nodeObject = Instantiate(battleNodePrefab,transform);
        nodeObject.transform.position = new Vector3(nodeObject.transform.position.x,(node.nodeLevel*55)+25,nodeObject.transform.position.z);
    }
}
