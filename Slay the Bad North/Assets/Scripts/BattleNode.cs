using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum BattleNodeType
{
    Normal,
    Hard,
    Free
}

public class BattleNode : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] Image image;
    public List<BattleNode> prevNode{get;set;}
    public int nodeLevel{get;set;}
    public int nodeLayer{get;set;}
    public bool completed{get;set;}
    public BattleNodeType nodeType{get;set;}
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
        if (completed)
        {
            nodeColor = Color.red;
        }else
        {
            if (prevNode.Find(n => n.completed==true)!=null)
            {
                nodeColor = Color.green;
            }
        }
    }


    public override string ToString(){
        var numberOfNodes = prevNode == null ? 0 : prevNode.Count;
        return $" PreviousNodes:{numberOfNodes}, NodeLevel:{nodeLevel}, Completed:{completed}";
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (completed)
        {
            image.color = Color.gray;
        }
        else{
            nodeColor = Color.yellow;
            completed = true;   
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
