using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleTree : MonoBehaviour
{
    [SerializeField] NodeDisplay battleNodePrefab;
    [SerializeField] RectTransform linePrefab;
    List<BattleNode> battleNodes;
    Dictionary<BattleNode,GameObject> NodeObjectRelation;
    void Start()
    {
        NodeObjectRelation = new Dictionary<BattleNode, GameObject>();
        if (GameManager.Instance.battleTree == null)
        {
            battleNodes = new List<BattleNode>();
            GenerateBattleTree();
        }
        else{
            battleNodes = GameManager.Instance.battleTree;
        }
        DrawBattleTree();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateBattleTree()
    {

        for (int i = 0; i < 11; i++)
        {
            var battleNode = new BattleNode(); 
            //ir buscar a imagem ao battle node e meter quando faz draw tree
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
            battleNodes.Add(battleNode);

            if (i != 0)
            {
                //se for o primeiro node de um dos ramos
                if (battleNodes.Where(n => n.nodeLayer == battleNode.nodeLayer).ToArray().Length == 1)
                {
                    if (battleNodes[i - 1].nodeLayer != 0)
                    {
                        battleNodes.First(n => n.nodeLayer == 0 && n.nodeLevel == battleNodes[i - 1].nodeLevel + 1).prevNode.Add(battleNodes[i - 1]);
                    }
                    battleNode.prevNode.Add(battleNodes.First(n => n.nodeLayer == 0 && n.nodeLevel == battleNode.nodeLevel - 1));
                    
                }
                else{
                    //se o anterior for o ultimo node vai buscar o node da linha principal para fechar
                    // if (battleNodes[i - 1].nodeLayer != battleNode.nodeLayer)
                    // {
                    //     battleNodes.First(n => n.nodeLayer == 0 && n.nodeLevel == battleNodes[i - 1].nodeLevel + 1).prevNode.Add(battleNodes[i - 1]);
                    // }
                    if(battleNodes.Where(n => n.nodeLayer == battleNode.nodeLayer).ToArray().Length != 1)
                        battleNode.prevNode.Add(battleNodes[i - 1]);
                }
            }

        }
        //execoes
        battleNodes[0].state = NodeState.Open;
        battleNodes[3].prevNode.Add(battleNodes[10]);

        GameManager.Instance.battleTree = battleNodes;
    }

    public void DrawBattleTree(){
        battleNodes.ForEach(n => { 
            PlaceBattleNode(n);
        });
        battleNodes.ForEach(n => { 
            n.prevNode.ForEach(p => DrawLine(n,p));
        });
    }

    void PlaceBattleNode(BattleNode node)
    {
        node.UpdateNode();
        var battleNodeDisplay = Instantiate(battleNodePrefab, this.transform);
        battleNodeDisplay.node = node;
        NodeObjectRelation.Add(node,battleNodeDisplay.gameObject);
        //preciso de ir buscar o objeto atraves do node
        battleNodeDisplay.transform.position = new Vector3(transform.position.x + (node.nodeLayer * 100), (node.nodeLevel * 100) + 25, transform.position.z);
        
    }
    void DrawLine(BattleNode prev, BattleNode next){
        RectTransform prevRect = NodeObjectRelation[prev].GetComponent<RectTransform>();
        RectTransform nextRect = NodeObjectRelation[next].GetComponent<RectTransform>();
        RectTransform aux;

        if (prevRect.localPosition.x>nextRect.localPosition.x)
        {
            aux = prevRect;
            prevRect = nextRect;
            nextRect = aux; 
        }

        RectTransform rectTransform = Instantiate(linePrefab,Vector3.zero,Quaternion.identity,this.transform);
        rectTransform.SetSiblingIndex(0);
        rectTransform.localPosition = (prevRect.localPosition + nextRect.localPosition) / 2;
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x,rectTransform.localPosition.y,-1);
        Vector3 dif = nextRect.localPosition - prevRect.localPosition;
        rectTransform.sizeDelta = new Vector3(dif.magnitude, 5);
        rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
    }
}