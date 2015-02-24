using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
    public List<Card> hand = new List<Card>();
    public int contract = 1;
    public int score = 0;
    
    //Constructor
    public Player(List<Card> hand)
    {
        if (hand.Count != 13)
            throw new UnityException("Player initialized with invalid number of cards");

        this.hand = new List<Card>(hand);
    }
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void printHand()
    {
        Debug.Log("\nplayer has " + hand.Count + " cards");
        for (int i = 0; i < hand.Count; i++)
        {
            Debug.Log(i + " " + hand[i].suit + " " + hand[i].value + "\n");
        }
    }
}
