using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodeDisplay : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public BattleNode node;
    [SerializeField] Image nodeImage;
    void Start()
    {
        nodeImage.sprite = node.nodeSprite;
        nodeImage.color = node.nodeColor;
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        nodeImage.color = Color.cyan;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        nodeImage.color = node.nodeColor;
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        node.OnClickAction();
    }
}
