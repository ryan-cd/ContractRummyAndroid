using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck{
    
    private List<Card> deck = new List<Card>();
    public List<Card> drawList = new List<Card>();
    public List<Card> discardList = new List<Card>();
    public List<Player> playerList = new List<Player>();
	private enum SORTS
	{
		VALUE, SUIT
	};
    public const int handSize = 13;
    public const int numPlayers = 4;
    private int playerTurn = 0;
    public GameObject lastGameObjectHit = null;
    public GameObject currentGameObjectHit = null;
    public ButtonWrapper lastButtonHit = null;
    public ButtonWrapper currentButtonHit = null;

    private enum GameState
    {
        MENU, DEALING, DRAWING, CHECK, CONTRACT, DISCARDING
    };
    private GameState gameState;

    

    public void initialize()
    {
        gameState = GameState.DEALING;
        _createDeck();
        _createPlayers();
        _initializeDrawPile();
        gameState = GameState.DRAWING;
    }

    /*
     * INPUT HANDLING
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     * */
    public void handleInput(GameObject gameObject, ButtonWrapper button)
    {
        _handleInputGameObject(gameObject);
        _handleInputButton(button);

        switch (gameState)
        {
            case GameState.MENU:
                break;
            case GameState.DEALING:
                //TODO, redeal, but dont need to replace the players like initialize
                break;
            case GameState.DRAWING:
                //this is handled above
                break;
            case GameState.CHECK:
                break;
            case GameState.CONTRACT:
                break;
            case GameState.DISCARDING:
                //handled above
                break;

        }
    }

    private void _handleInputGameObject(GameObject input)
    {
        this.currentGameObjectHit = input;

        if (this.currentGameObjectHit != this.lastGameObjectHit)
        {
            if (input != null)
                Debug.Log("Card: " + input.name + " Sprite: " + input.GetComponent<SpriteRenderer>().sprite);

            this.lastGameObjectHit = this.currentGameObjectHit;
            if (gameState == GameState.DRAWING)
            {
                _handleDraw(this.currentGameObjectHit);
                return;
            }

            if (gameState == GameState.DISCARDING)
            {
                _handleDiscard(this.currentGameObjectHit);
                return;
            }
        }
    }

    private void _handleInputButton(ButtonWrapper input)
    {
        this.currentButtonHit = input;

        if (this.currentButtonHit != this.lastButtonHit && input != null)
        {
            this.lastButtonHit = input;
            Debug.Log("Button Clicked: " + input.getText());

            if (input.getText() == Drawing.sortSuitButtonName)
            {
                playerList[0].sortBySuit();
            }

            else if (input.getText() == Drawing.sortValueButtonName)
            {
                playerList[0].sortByValue();
            }
        }
    }

    
    private void _handleDraw(GameObject currentGameObjectHit)
    {
        if (currentGameObjectHit.name == "DrawPile" && drawList.Count > 0)
        {
            playerList[playerTurn].drawCard(drawList[0]);
            drawList.RemoveAt(0);
            gameState = GameState.DISCARDING;
			/*_sortHand(0, SORTS.SUIT);
            _sortHand(1, SORTS.VALUE);
            _sortHand(2, SORTS.VALUE);
            _sortHand(3, SORTS.VALUE);*/
        }

        else if (currentGameObjectHit.name == "DiscardPile" && discardList.Count > 0)
        {
            playerList[playerTurn].drawCard(discardList[discardList.Count - 1]);
            discardList.RemoveAt(discardList.Count - 1);
            gameState = GameState.DISCARDING;
        }
    }

    private void _handleDiscard(GameObject currentGameObejectHit)
    {
        string cardNumberString = currentGameObjectHit.name.Substring(11);
        int cardNumber = int.Parse(cardNumberString);

        if (currentGameObjectHit.name.Substring(0, 11) == "Player0Card")
        {
            discardList.Add(playerList[playerTurn].hand[cardNumber]);
            playerList[playerTurn].discardCard(cardNumber);

            gameState = GameState.DRAWING;
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
                card.calculateSpriteNumber();
                deck.Add(card);
            }
        }

        for (int i = 52; i <= 103; i++)
        {
            Card card = new Card(deck[i - 52]);
            deck.Add(card);
        }

        _shuffleDeck();
    }

    private void _shuffleDeck()
    {
        int randomIndex;
        for (int i = 0; i < 104; i++)
        {

            Card temp = new Card(deck[i]);
            randomIndex = Random.Range(i, deck.Count);
            deck[i] = new Card(deck[randomIndex]);
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
    }

    private void _initializeDrawPile()
    {
        drawList = deck;
    }

}
