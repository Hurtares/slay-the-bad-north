using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

public class BattleNode : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] Image image;
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
            image.color = _nodeColor;
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

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
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

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.cyan;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        image.color = nodeColor;
    }
}
