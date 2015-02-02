using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    public Sprite[] cardSprites;

    private Deck deck = null;
    //private Drawing draw = new Drawing();
    public Drawing draw;
    

    void Awake()
    {
        draw = gameObject.AddComponent<Drawing>();
        draw.Method();
        cardSprites = Resources.LoadAll<Sprite>("playingCards");
    }
    
    // Use this for initialization
	void Start () {
        print("hello world");
        draw.Method();
        //GameObject.Find("PlayerCard0").GetComponent("SpriteRenderer").sprite = sprites.GetSprite("Sprite1");
        GameObject go = new GameObject();
        //go = GameObject.Find("PlayerCardT");// new GameObject("go");
        //go.AddComponent("SpriteRenderer");
        go.AddComponent<SpriteRenderer>();
        //go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("JD");
        go.GetComponent<SpriteRenderer>().sprite = cardSprites[12];
        
        //go.GetComponent<SpriteRenderer>().sprite = sprites.GetSprite("Sprite1");

        deck = new Deck();
        
        print(deck == null);
        
        //this will create a deck, shuffle, and initialize all players with their hands
        deck.initialize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
