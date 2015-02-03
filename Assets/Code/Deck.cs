using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck{
    
    private List<Card> deck = new List<Card>();

    public List<Card> drawList = new List<Card>();
    public List<Card> discardList = new List<Card>();
    public List<Player> playerList = new List<Player>();

     
    private int contractNumber = 1;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void initialize()
    {
        _createDeck();
        _createPlayers();
        _initializeDrawPile();
    }

    /*
     * INTERNAL FUNCTIONS
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     * */

    private void _createDeck()
    {
        foreach (Card.SUITS suit in System.Enum.GetValues(typeof(Card.SUITS)))
        {
            for (int j = 2; j <= 14; j++)
            {
                Card card = new Card();
                card.setSuit(suit);
                card.setValue(j);
                card.calculateSpriteNumber();
                deck.Add(card);
            }
        }

        for (int i = 52; i <= 103; i++ )
        {
            deck.Add(deck[i - 52]);
        }

        _shuffleDeck();
    }

    private void _shuffleDeck()
    {
        int randomIndex;
        for (int i = 0; i < 104; i++)
        {

            Card temp = deck[i];
            randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    private void _createPlayers()
    {
        List<Card> tempHand = new List<Card>();
        Player tempPlayer;

        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 13; j++)
            {
                tempHand.Add(deck[i * 13 + j]);
            }
            Debug.Log(tempHand.Count);
            tempPlayer = new Player(tempHand);
            playerList.Add(tempPlayer);
            tempHand.Clear();
        }
        
        deck.RemoveRange(0, 52);

        /*
        Debug.Log("Deck size = " + deck.Count);

        for (int i = 0; i < playerList.Count; i++)
        {
            Debug.Log("\nPlayer " + i);
            playerList[0].printHand();
        }*/
    }

    private void _initializeDrawPile()
    {
        drawList = deck;
    }

    /*
     * Getters
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~
     * */
    
}
