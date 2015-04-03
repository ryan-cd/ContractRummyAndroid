using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Drawing : MonoBehaviour {
    /*public struct handParameters
    {
        public Vector3 translate;
        public int deltaX;
        public int deltaY;
        public int deltaZ;
    };

    public List<handParameters> handParametersList = new List<handParameters>();
    public handParameters player1Parameters = new handParameters();*/
    private Vector3 player1Position = new Vector3(-4, -4, 20);
    private Vector3 player2Position = new Vector3(-7, 4, 20);
    private Vector3 player3Position = new Vector3(-4, 4, 20);
    private Vector3 player4Position = new Vector3(7, 4, 20);
    public float cardOffset = 0.7f;
    public List<Vector3> playerPositionList = new List<Vector3>();
    private Vector3 drawPilePosition = new Vector3(-1, 0, 20);
    private Vector3 discardPilePosition = new Vector3(1, 0, 20);
    public List<Vector3> pilePositions = new List<Vector3>();

    public Sprite[] cardSprites;

    private List<Player> playerList;
    private List<Card> drawList;    
    public List<Card> discardList;

    public Card lastDiscardedCard = null;

    private bool firstDraw = true;
    
    void Awake()
    {
        cardSprites = Resources.LoadAll<Sprite>("playingCards");
        playerPositionList.Add(player1Position);
        playerPositionList.Add(player2Position);
        playerPositionList.Add(player3Position);
        playerPositionList.Add(player4Position);

        pilePositions.Add(drawPilePosition);
        pilePositions.Add(discardPilePosition);
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateState(List<Player> playerList, List<Card> drawList, List<Card> discardList)
    {
        this.playerList = playerList;
        this.drawList = drawList;
        this.discardList = discardList;
        if (discardList.Count > 0)
            lastDiscardedCard = discardList[discardList.Count - 1];
    }

    public void draw()
    {
        _drawPlayerHands();
        _drawPiles();
    }

    private void _drawPlayerHands()
    {
        for (int i = 0; i < 4; i++)
        {
            //delete gameobjects that are no longer in use
            for (int w = 13; w >= 0 && w > playerList[i].hand.Count-1; w--)
            {
                GameObject garbageObject = GameObject.Find("Player" + i + "Card" + w);
                if (garbageObject != null)
                {
                    Destroy(garbageObject);
                }
            }

            Vector3 tempTranslate = playerPositionList[i];
            //Debug.Log("Player has: " + playerList[0].hand.Count + " cards");
            
            for (int j = 0; j < playerList[i].hand.Count; j++)
            {
                GameObject go;
                go = GameObject.Find("Player" + i + "Card" + j);
                if (go == null)
                {
                    go = new GameObject("Player" + i + "Card" + j);
                    go.AddComponent<SpriteRenderer>();
                    go.AddComponent<BoxCollider>();
                    go.GetComponent<Transform>().localScale = new Vector3(0.7f, 0.7f, 1f);
                    Vector2 boxColliderSize = new Vector2(1.4f, 1.9f);
                    go.GetComponent<BoxCollider>().size = boxColliderSize;
                }

                if (playerList[i].hand[j].locationTag == Card.LOCATIONTAGS.DRAWN
                    && j == playerList[i].hand.Count-1)
                {
                    tempTranslate.y += cardOffset;
                }
                go.transform.position = tempTranslate;
               
                go.GetComponent<SpriteRenderer>().sprite = cardSprites[playerList[i].hand[j].spriteNumber];


                if (i % 2 == 0)
                    tempTranslate.x += cardOffset;
                else
                    tempTranslate.y -= cardOffset;
                tempTranslate.z -= cardOffset;
            }
        }
    }

    private void _drawPiles()
    {
        if (discardList.Count == 0 && GameObject.Find("DiscardPile") != null)
        {
            Destroy(GameObject.Find("DiscardPile"));
        }
        if (drawList.Count == 0 && GameObject.Find("DrawPile") != null)
        {
            Destroy(GameObject.Find("DrawPile"));
        }
        _drawDrawPile();
        _drawDiscardPile();
        
        /*for (int i = 0; i < 2; i++)
        {
            Vector3 translate = pilePositions[i];

            GameObject go = null;
            if (i == 0)
                go = GameObject.Find("DrawPile");
            else if (i == 1)
                go = GameObject.Find("DiscardPile");

            if (go == null && drawList.Count > 0)
            {
                if (i == 0)
                    go = new GameObject("DrawPile");
                else if (i == 1)
                    go = new GameObject("DiscardPile");
                go.AddComponent<SpriteRenderer>();
                go.AddComponent<BoxCollider>();
                go.GetComponent<Transform>().localScale = new Vector3(0.7f, 0.7f, 1f);
                Vector2 boxColliderSize = new Vector2(1.4f, 1.9f);
                go.GetComponent<BoxCollider>().size = boxColliderSize;
            }
            
            go.transform.position = translate;
            if(i == 0)
                go.GetComponent<SpriteRenderer>().sprite = cardSprites[52]; //card backs start at index 52
            else if (i == 1)
            {
                if (lastDiscardedCard == null)
                    break;
                else
                    go.GetComponent<SpriteRenderer>().sprite = cardSprites[lastDiscardedCard.spriteNumber];
            }

        }*/
        
    }

    private void _drawDrawPile()
    {
        Vector3 translate = pilePositions[0];

        GameObject go = GameObject.Find("DrawPile");
        
        if (go == null && drawList.Count > 0)
        {
            go = new GameObject("DrawPile");

            go.AddComponent<SpriteRenderer>();
            go.AddComponent<BoxCollider>();
            go.GetComponent<Transform>().localScale = new Vector3(0.7f, 0.7f, 1f);
            Vector2 boxColliderSize = new Vector2(1.4f, 1.9f);
            go.GetComponent<BoxCollider>().size = boxColliderSize;
            go.transform.position = translate;
            go.GetComponent<SpriteRenderer>().sprite = cardSprites[52]; //card backs start at index 52
        
        }
        
        
    }

    private void _drawDiscardPile()
    {
        Vector3 translate = pilePositions[1];

        GameObject go = GameObject.Find("DiscardPile");

        if (discardList.Count > 0)
        {
            if (go == null)
            {
                go = new GameObject("DiscardPile");
                go.AddComponent<SpriteRenderer>();
                go.AddComponent<BoxCollider>();
            }
            go.GetComponent<Transform>().localScale = new Vector3(0.7f, 0.7f, 1f);
            Vector2 boxColliderSize = new Vector2(1.4f, 1.9f);
            go.GetComponent<BoxCollider>().size = boxColliderSize;
            go.transform.position = translate;

            if (lastDiscardedCard == null)
                return;
            else
                go.GetComponent<SpriteRenderer>().sprite = cardSprites[lastDiscardedCard.spriteNumber];
        
        }

        
    }

}
