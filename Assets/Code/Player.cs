using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
    public List<Card> hand {get; private set;}
    public List<List<Card>> sets {get; private set;}
    public List<List<Card>> runs {get; private set;}
    public int contractNumber = 1;
    public int score = 0;
    
    //Constructor
    public Player(List<Card> hand)
    {
        if (hand.Count != 13)
            throw new UnityException("Player initialized with invalid number of cards");

        this.hand = new List<Card>(hand);

        sets = new List<List<Card>>();
        runs = new List<List<Card>>();
    }
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*
     * GETTERS
     * 
     * */

    public void printHand()
    {
        string result = "Player has " + hand.Count + " cards: ";
        
        for (int i = 0; i < hand.Count; i++)
        {
            result += ("" + i + "[" + hand[i].suit + " " + hand[i].value + "]");
        }
        Debug.Log(result);
    }

    public int getPoints()
    {
        int points = 0;

        for (int i = 0; i < hand.Count; i++)
        {
            points += hand[i].points;
        }

        return points;
    }

    public void sortByValue()
    {
        hand = Algorithms.sortByValue(hand);
    }

    public void sortBySuit()
    {
        hand = Algorithms.sortBySuit(hand);
    }

    public bool hasContract()
    {
        return Algorithms.hasContract(hand, contractNumber);
    }

    public bool hasPlacedContract()
    {
        return sets.Count > 0 || runs.Count > 0;
    }

    /*
     * MUTATORS
     * 
     * */

    public void drawCard(Card newCard)
    {
        newCard.setLocationTag(Card.LOCATIONTAGS.DRAWN);
        hand.Add(newCard);
        Debug.Log(Algorithms.hasContract(hand, contractNumber));
    }

    public void discardCard(int index)
    {
        hand.RemoveAt(index);
        foreach (Card c in hand)
        {
            c.setLocationTag(Card.LOCATIONTAGS.DEFAULT);
        }
    }

    //Contract
    public void addSet(List<Card> newSet)
    {
        if(newSet.Count < 3 || newSet.Count > 8)
            throw new UnityException("Invalid set added to player");
        
        sets.Add(newSet);
    }
    
    public void addRun(List<Card> newRun)
    {
        if(newRun.Count < 4 || newRun.Count > 10)
            throw new UnityException("Invalid run added to player");
        
        runs.Add(newRun);
    }
}
