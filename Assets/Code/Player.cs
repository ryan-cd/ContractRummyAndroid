using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
    public List<Card> hand = new List<Card>();
    public int contractNumber = 1;
    public int score = 0;
    public bool hasContract {get; private set;}
    
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

    /*
     * MUTATORS
     * 
     * */

    public void drawCard(Card newCard)
    {
        newCard.setLocationTag(Card.LOCATIONTAGS.DRAWN);
        hand.Add(newCard);
    }

    public void discardCard(int index)
    {
        hand.RemoveAt(index);
        foreach (Card c in hand)
        {
            c.setLocationTag(Card.LOCATIONTAGS.DEFAULT);
        }
    }

    public void checkIfContractComplete()
    {
        switch(contractNumber)
        {
            case 1:
                
                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            default:
                throw new UnityException("A player has reached an illegal contract number");
        }
    }
}
