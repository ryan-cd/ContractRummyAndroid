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
        Debug.Log("Draw class updating self");
        this.playerList = playerList;
        this.drawList = drawList;
        this.discardList = discardList;
    }

    public void draw()
    {
        Vector3 translate = new Vector3();
        translate[0] = 0;
        translate[1] = 0;
        translate[2] = 0;

        

        for (int i = 0; i < playerList[0].hand.Count; i++)
        {
            GameObject go = new GameObject();
            //go = GameObject.Find("PlayerCardT");// new GameObject("go");
            go.transform.Translate(translate);
            go.AddComponent<SpriteRenderer>();
            Debug.Log("sprite number: " + playerList[0].hand[i].spriteNumber);
            go.GetComponent<SpriteRenderer>().sprite = cardSprites[playerList[0].hand[i].spriteNumber];
            translate.x += 1;
        }
    }
}
