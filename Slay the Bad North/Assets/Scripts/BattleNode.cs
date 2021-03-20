using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleNodeType
{
    Normal,
    Hard,
    Free
}

public enum NodeState{
    Close,
    Open,
    Completed
}

[System.Serializable]
public class BattleNode
{
    public Sprite nodeSprite;
    public List<BattleNode> prevNode{get;set;}
    public int nodeLevel{get;set;}
    public int nodeLayer{get;set;}
    public BattleNodeType nodeType{get;set;}
    public NodeState state = NodeState.Close;
    //Transformar isto num dicionario
    Color _nodeColor = Color.white;
    public Color nodeColor{
        get{
            return _nodeColor;
        }
        set{
            _nodeColor = value;
        }
    }

    public void UpdateNode(){
        if (state == NodeState.Completed)
        {
            nodeColor = Color.red;
        }else
        {
            if (prevNode.Find(n => n.state == NodeState.Completed==true)!=null)
            {
                nodeColor = Color.green;
                state = NodeState.Open;
            }
        }
    }


    public override string ToString(){
        var numberOfNodes = prevNode == null ? 0 : prevNode.Count;
        return $" PreviousNodes:{numberOfNodes}, NodeLevel:{nodeLevel}, Completed:{state == NodeState.Completed}";
    }

    public void OnClickAction(){
        if (state == NodeState.Completed)
        {
            //color/sound feedback que nao funciona
        }else
        {

            if (state == NodeState.Open)
            {
                GameManager.Instance.AddCardsToDeck();
                GameManager.Instance.currentNode = this;
                SceneManager.LoadScene(2);
            }
        }
    }
    
}
