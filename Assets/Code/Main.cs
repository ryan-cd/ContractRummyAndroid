using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    private Deck deck = new Deck();
    //private Drawing draw = new Drawing();
    public Drawing renderer = null;
    

    void Awake()
    {
        renderer = gameObject.AddComponent<Drawing>();
    }
    
    // This is the program entry point
	void Start () {
        //this will create a deck, shuffle, and initialize all players with their hands
        deck.initialize();
        //this will draw the game that the deck establishes
        renderer.updateState(deck.playerList, deck.drawList, deck.discardList);
        renderer.draw();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
