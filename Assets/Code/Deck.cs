using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck{
    
    private List<Card> deck = new List<Card>();
    public List<Card> drawList = new List<Card>();
    public List<Card> discardList = new List<Card>();
    public List<Player> playerList = new List<Player>();
    public const int handSize = 13;
    public const int numPlayers = 4;
    private int playerTurn = 0;
    public GameObject lastGameObjectHit = null;
    public GameObject currentGameObjectHit = null;

    private enum GameState
    {
        MENU, DEALING, CHECK, DRAWING, DISCARDING
    };
    private GameState gameState;

    /*void Awake()
    {
        Debug.Log("Awake function reached");
        gameState = GameState.DEALING;
    }
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //this function is never called since Deck doesnt implement MonoBehavior
        if (gameState == GameState.DEALING)
        {

        }
        else if (gameState == GameState.CHECK)
        {
            _checkHands();
            gameState = GameState.TURN;
        }
        else if (gameState == GameState.TURN)
        {
            
        }
	}*/

    public void initialize()
    {
        gameState = GameState.DEALING;
        _createDeck();
        _createPlayers();
        _initializeDrawPile();
        gameState = GameState.DRAWING;
    }

    public void handleInput(GameObject input)
    {

        //if (input != null)
            //Debug.Log("name: "+ input.name + " sprite: "+ input.GetComponent<SpriteRenderer>().sprite);
        
        this.currentGameObjectHit = input;
        
        if (this.currentGameObjectHit != this.lastGameObjectHit)
        {

            this.lastGameObjectHit = this.currentGameObjectHit;
            if (gameState == GameState.DRAWING)
            {
                if (currentGameObjectHit.name == "DrawPile")
                {
                    playerList[playerTurn].drawCard(drawList[0]);
                    drawList.RemoveAt(0);
                    gameState = GameState.DISCARDING;
                }

                return;
            }

            if (gameState == GameState.DISCARDING)
            {
                string cardNumberString = currentGameObjectHit.name.Substring(11);
                //int cardNumber = int.Parse(currentGameObjectHit.name.Substring(11));
                int cardNumber = int.Parse(cardNumberString);
                Debug.Log(currentGameObjectHit.name.Substring(11)+" "+cardNumber);
                if (currentGameObjectHit.name.Substring(0,11) == "Player0Card")
                {
                    discardList.Add(playerList[playerTurn].hand[cardNumber]);
                    playerList[playerTurn].discardCard(cardNumber);
                    
                    gameState = GameState.DRAWING;
                }
            }
        }
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
                card.setSpriteNumber(4 * j + (int)suit - 8);
                //card.calculateSpriteNumber();
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

        for(int i = 0; i < numPlayers; i++)
        {
            for (int j = 0; j < handSize; j++)
            {
                tempHand.Add(deck[i * handSize + j]);
                tempHand[j].setLocationTag(Card.LOCATIONTAGS.DEFAULT);
            }
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

    private void _checkHands()
    {
        
    }

    /*
     * Getters
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~
     * */
    
}
