using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck{
    
    private List<Card> deck = new List<Card>();
    public List<Card> drawList = new List<Card>();
    public List<Card> discardList = new List<Card>();
    public List<Player> playerList = new List<Player>();
    
    //TODO: Move these to a new class
    List<Card> _currentSet = new List<Card>();
    int _lastHandIndex = -1;

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
        MENU, DEALING, DRAWING, CHECK, CONTRACT, PLACE_SET, PLACE_RUN, DISCARDING
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
        if (gameState == GameState.DRAWING 
            || gameState == GameState.CONTRACT 
            || gameState == GameState.PLACE_SET
            || gameState == GameState.PLACE_RUN
            || gameState == GameState.DISCARDING)
        {
            _handleInputGameObject(gameObject);
            _handleInputButton(button);
        }

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
                checkContracts(); //will set the next state to either CONTRACT or DISCARDING
                break;
            case GameState.CONTRACT:
                //this is handled above
                break;
            case GameState.PLACE_SET:
                //_handlePlaceContract();    
                //gameState = GameState.DISCARDING;
                break;
            case GameState.PLACE_RUN:
                gameState = GameState.DISCARDING;
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

            if (gameState == GameState.DISCARDING || gameState == GameState.CONTRACT)
            {
                _handleDiscard(this.currentGameObjectHit);
                return;
            }

            if (gameState == GameState.PLACE_SET)
            {
                _handlePlaceContract();
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
            
            //Lower any raised cards
            for (int i = 0; i < playerList[0].hand.Count; i++)
                if (playerList[0].hand[i].locationTag != Card.LOCATIONTAGS.DEFAULT)
                    playerList[0].hand[i].setLocationTag(Card.LOCATIONTAGS.DEFAULT);

            if (input.getText() == Drawing.sortSuitButtonName)
            {
                playerList[0].sortBySuit();
            }

            else if (input.getText() == Drawing.sortValueButtonName)
            {
                playerList[0].sortByValue();
            }

            else if (input.getText() == Drawing.contractButtonName)
            {
                if (playerList[0].hasContract() && gameState == GameState.CONTRACT)
                {
                    _handlePlaceContract();
                }

                //gameState = GameState.DISCARDING;
            }
        }
    }

    
    private void _handleDraw(GameObject currentGameObjectHit)
    {
        if (currentGameObjectHit.name == "DrawPile" && drawList.Count > 0)
        {
            playerList[playerTurn].drawCard(drawList[0]);
            drawList.RemoveAt(0);
            gameState = GameState.CHECK;
        }

        else if (currentGameObjectHit.name == "DiscardPile" && discardList.Count > 0)
        {
            playerList[playerTurn].drawCard(discardList[discardList.Count - 1]);
            discardList.RemoveAt(discardList.Count - 1);
            gameState = GameState.CHECK;
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

    private void _handlePlaceContract()
    {
        Debug.Log("Handling contract placement");
        switch(playerList[0].contractNumber)
        {
            case 1:
                _handlePlaceSet(2);
                break;
            case 2:
                _handlePlaceSet(1);
                _handlePlaceRun(4);
                break;
            case 3:
                _handlePlaceSet(2, 4);
                break;
            case 4:
                _handlePlaceRun(2);
                break;
            case 5:
                _handlePlaceSet(1, 4);
                _handlePlaceRun(1);
                break;
            case 6:
                _handlePlaceSet(3);
                break;
            case 7:
                _handlePlaceSet(1);
                _handlePlaceRun(1, 7);
                break;
            default:
                throw new UnityException("Trying to place unimplemented contract");
        }
    }

    private void _handlePlaceSet(int numberOfSets, int setLength = 3)
    {
        gameState = GameState.PLACE_SET;

        if (_currentSet.Count < setLength)
        {
            //TODO: Implement
            if (currentGameObjectHit.name.Length >= 11
                && currentGameObjectHit.name.Substring(0, 11) == "Player0Card")
            {
                _lastHandIndex = int.Parse(currentGameObjectHit.name.Substring(11));
                Debug.Log("trying to raise: " + _lastHandIndex);

                //Toggle if the card is raised or not.
                if (playerList[0].hand[_lastHandIndex].locationTag == Card.LOCATIONTAGS.DEFAULT)
                    playerList[0].hand[_lastHandIndex].setLocationTag(Card.LOCATIONTAGS.DRAWN);
                else if (playerList[0].hand[_lastHandIndex].locationTag == Card.LOCATIONTAGS.DRAWN)
                    playerList[0].hand[_lastHandIndex].setLocationTag(Card.LOCATIONTAGS.DEFAULT);
                //_currentSet.Add(playerList[0].hand[_lastHandIndex]);
            }
        }
        else
        {
            Debug.Log("The state moves on");
        }
    }

    private void _handlePlaceRun(int numberOfRuns, int runLength = 3)
    {
        gameState = GameState.PLACE_RUN;

        for (int i = 0; i < numberOfRuns; i++)
        {
            //TODO: Implement
        }
    }

    private void checkContracts()
    {
        if (playerList[0].hasContract())
        {
            Debug.Log("Player has contract");
            gameState = GameState.CONTRACT;
        }
        else
            gameState = GameState.DISCARDING;
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
