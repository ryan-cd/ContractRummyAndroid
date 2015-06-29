using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Drawing : MonoBehaviour {
    private Vector3 player1Position = new Vector3(-4, -4, 20);
    private Vector3 player2Position = new Vector3(-7, 4, 20);
    private Vector3 player3Position = new Vector3(-4, 4, 20);
    private Vector3 player4Position = new Vector3(7, 4, 20);
    public float cardOffset = 0.65f;
    public List<Vector3> playerPositionList = new List<Vector3>();
    private Vector3 drawPilePosition = new Vector3(-1, 0, 20);
    private Vector3 discardPilePosition = new Vector3(1, 0, 20);
    public List<Vector3> pilePositions = new List<Vector3>();

    public Sprite[] cardSprites;

    private List<Player> playerList;
    private List<Card> drawList;    
    public List<Card> discardList;

    public Card lastDiscardedCard = null;

    private ButtonWrapper sortSuitButton;
    private ButtonWrapper sortValueButton;
    private ButtonWrapper contractButton;
    public static string sortSuitButtonName = "Sort By Suit"; //this object is directly referenced by Deck
    public static string sortValueButtonName = "Sort By Value"; //this object is directly referenced by Deck
    public static string contractButtonName = "Place Contract"; //this object is directly referenced by Deck
    public static string setButtonName = "Place Set"; //this object is directly referenced by Deck
    public static string runButtonName = "Place Run"; //this object is directly referenced by Deck
    public static Vector3 sortSuitButtonPosition = new Vector3(300, -200, 10);
    public static Vector3 sortValueButtonPosition = new Vector3(300, -230, 10);
    public static Vector3 contractButtonPosition = new Vector3(-290, -200, 10);
    
    void Awake()
    {
        cardSprites = Resources.LoadAll<Sprite>("playingCards");
        playerPositionList.Add(player1Position);
        playerPositionList.Add(player2Position);
        playerPositionList.Add(player3Position);
        playerPositionList.Add(player4Position);

        pilePositions.Add(drawPilePosition);
        pilePositions.Add(discardPilePosition);
        
        sortSuitButton = gameObject.AddComponent<ButtonWrapper>();
        sortValueButton = gameObject.AddComponent<ButtonWrapper>();
        contractButton = gameObject.AddComponent<ButtonWrapper>();
        sortSuitButton.construct(
            "Button", 
            sortSuitButtonPosition, 
            Quaternion.identity, 
            sortSuitButtonName, 
            ()=>Inputs.setLastButtonHit(sortSuitButton)
        );
        sortValueButton.construct(
            "Button", 
            sortValueButtonPosition, 
            Quaternion.identity, 
            sortValueButtonName,
            ()=>Inputs.setLastButtonHit(sortValueButton)
        );
        contractButton.construct(
            "Button",
            contractButtonPosition,
            Quaternion.identity,
            contractButtonName,
            () => Inputs.setLastButtonHit(contractButton)
        );
    }
    
    public static void hello()
    {
        Debug.Log("Hello");
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateState(Deck.GameState gameState, List<Player> playerList, List<Card> drawList, List<Card> discardList)
    {
        switch (gameState)
        {
            case Deck.GameState.PLACE_RUN:
                contractButton.setText(runButtonName);
                break;
            case Deck.GameState.PLACE_SET:
                contractButton.setText(setButtonName);
                break;
            default:
                contractButton.setText(contractButtonName);
                break;
        }
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
            _flushHands(i);

            Vector3 tempTranslate = playerPositionList[i];
            
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

                if (playerList[i].hand[j].locationTag == Card.LOCATIONTAGS.DRAWN)
                {
                    tempTranslate.y += cardOffset;
                }
                go.transform.position = tempTranslate;
                if (playerList[i].hand[j].locationTag == Card.LOCATIONTAGS.DRAWN)
                {
                    tempTranslate.y -= cardOffset;
                }
               
                go.GetComponent<SpriteRenderer>().sprite = cardSprites[playerList[i].hand[j].spriteNumber];


                if (i % 2 == 0)
                    tempTranslate.x += cardOffset;
                else
                    tempTranslate.y -= cardOffset;
                tempTranslate.z -= cardOffset;
            }
        }
    }

    //delete gameobjects that are no longer in use by that player
    private void _flushHands(int player)
    {
        for (int w = 13; w >= 0 && w > playerList[player].hand.Count - 1; w--)
        {
            GameObject garbageObject = GameObject.Find("Player" + player + "Card" + w);
            if (garbageObject != null)
            {
                Destroy(garbageObject);
            }
        }
    }

    private void _drawPiles()
    {
        _flushDrawPiles();
        _drawDrawPile();
        _drawDiscardPile();
    }

    //remove the pile graphic if it is empty
    private void _flushDrawPiles()
    {
        if (discardList.Count == 0 && GameObject.Find("DiscardPile") != null)
        {
            Destroy(GameObject.Find("DiscardPile"));
        }
        if (drawList.Count == 0 && GameObject.Find("DrawPile") != null)
        {
            Destroy(GameObject.Find("DrawPile"));
        }
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
