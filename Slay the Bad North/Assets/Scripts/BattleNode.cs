using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

public class BattleNode
{
    public List<BattleNode> previousNode{get;}
    public int nodeLevel{get;}
    public bool completed{get;set;}
    
    public BattleNode(List<BattleNode> previousNode,int nodeLevel,bool completed){
        this.previousNode = previousNode;
        this.nodeLevel = nodeLevel;
        this.completed = completed;
    }


    public override string ToString(){
        var numberOfNodes = previousNode == null ? 0 : previousNode.Count;
        return $" PreviousNodes:{numberOfNodes}, NodeLevel:{nodeLevel}, Completed:{completed}";
    }
}
