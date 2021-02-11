using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleNode : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] Image image;
    public List<BattleNode> prevNode{get;set;}
    public int nodeLevel{get;set;}
    public int nodeLayer{get;set;}
    public bool completed{get;set;}


    public override string ToString(){
        var numberOfNodes = prevNode == null ? 0 : prevNode.Count;
        return $" PreviousNodes:{numberOfNodes}, NodeLevel:{nodeLevel}, Completed:{completed}";
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        image.color = Color.green;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.cyan;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }
}
