using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    private Deck deck = new Deck(); //model
    private Inputs inputs = null;   //controller
    public Drawing renderer = null; //view

    private bool _firstRun = true;

    void Awake()
    {
        renderer = gameObject.AddComponent<Drawing>();
        inputs = gameObject.AddComponent<Inputs>();
    }
    
    // This is the program entry point
	void Start () {
        //this will create a deck, shuffle, and initialize all players with their hands
        deck.initialize();
	}
	
	// Update is called once per frame
	void Update () {
        if (inputs.inputExists() || _firstRun)
        {
            deck.handleInput(inputs.getLastGameObjectHit(), inputs.getLastButtonHit());
            _firstRun = false;
        }
        
        //this will draw the game that the deck establishes
        renderer.updateState(deck.gameState, deck.playerList, deck.drawList, deck.discardList);
        renderer.draw();

        inputs.resetInputs();
	}
}
