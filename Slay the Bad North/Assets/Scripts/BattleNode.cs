using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

public class BattleNode : MonoBehaviour
{
    public List<BattleNode> nextNode{get;set;}
    public int nodeLevel{get;set;}
    public int nodeLayer{get;set;}
    public bool completed{get;set;}
    
    public BattleNode(List<BattleNode> previousNode,int nodeLevel,bool completed){
        this.nextNode = previousNode;
        this.nodeLevel = nodeLevel;
        this.completed = completed;
    }


    public override string ToString(){
        var numberOfNodes = nextNode == null ? 0 : nextNode.Count;
        return $" PreviousNodes:{numberOfNodes}, NodeLevel:{nodeLevel}, Completed:{completed}";
    }
}
