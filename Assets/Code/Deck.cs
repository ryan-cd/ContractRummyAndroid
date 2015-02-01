using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck{
    
    private List<Card> deck = new List<Card>();
    public int test = 1;
	// Use this for initialization
	void Start () {
        Debug.Log("deck");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void createDeck()
    {
        Debug.Log("creating deck");
        foreach (Card.SUITS suit in System.Enum.GetValues(typeof(Card.SUITS)))
        {
            for (int j = 2; j <= 14; j++)
            {
                Card card = new Card();
                card.setSuit(suit);
                card.setValue(j);
                deck.Add(card);
            }
        }

        for (int i = 52; i <= 103; i++ )
        {
            deck.Add(deck[i - 52]);
        }

        shuffleDeck();
    }

    private void shuffleDeck()
    {
        int randomIndex;
        for (int i = 0; i < 104; i++)
        {

            Card temp = deck[i];
            randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
            Debug.Log(deck[i].suit + " " + deck[i].value + "\n");
        }
    }
}
