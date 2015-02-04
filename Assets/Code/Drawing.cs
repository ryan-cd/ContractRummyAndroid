using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drawing : MonoBehaviour {
    public Sprite[] cardSprites;

    private List<Player> playerList;
    private List<Card> drawList;    
    public List<Card> discardList;

    void Awake()
    {
        cardSprites = Resources.LoadAll<Sprite>("playingCards");
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
    }

    public void draw()
    {
        _drawPlayerHand();
        _drawDrawPile();
    }

    private void _drawPlayerHand()
    {
        Vector3 translate = new Vector3(-6, -4, 0);


        for (int i = 0; i < playerList[0].hand.Count; i++)
        {
            GameObject go = new GameObject("Player"+0+"Card"+i);
            //go = GameObject.Find("PlayerCardT");// new GameObject("go");
            go.transform.Translate(translate);
            go.AddComponent<SpriteRenderer>();
            //Debug.Log("sprite number: " + playerList[0].hand[i].spriteNumber);
            go.GetComponent<SpriteRenderer>().sprite = cardSprites[playerList[0].hand[i].spriteNumber];

            go.AddComponent<BoxCollider>();
            Vector2 boxColliderSize = new Vector2(1.4f, 1.9f);
            go.GetComponent<BoxCollider>().size = boxColliderSize;
            translate.x += 1;
        }
    }

    private void _drawDrawPile()
    {
        Vector3 translate = new Vector3();
        translate[0] = 0;
        translate[1] = 0;
        translate[2] = 0;

        GameObject go = new GameObject("DrawPile");
        //go = GameObject.Find("PlayerCardT");// new GameObject("go");
        go.transform.Translate(translate);
        go.AddComponent<SpriteRenderer>();
        go.GetComponent<SpriteRenderer>().sprite = cardSprites[52]; //card backs start at index 52

        go.AddComponent<BoxCollider>();
        Vector2 boxColliderSize = new Vector2(1.4f, 1.9f);
        go.GetComponent<BoxCollider>().size = boxColliderSize;
    }
}
