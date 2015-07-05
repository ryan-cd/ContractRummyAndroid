using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck{
    
    private List<Card> deck = new List<Card>();
    public List<Card> drawList = new List<Card>();
    public List<Card> discardList = new List<Card>();
    public List<Player> playerList = new List<Player>();

    private Inputs.InputTypes lastInputType = Inputs.InputTypes.GAME_OBJECT;
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

    public enum GameState
    {
        MENU, DEALING, DRAWING, CHECK, CONTRACT, PLACE_SET, PLACE_RUN, DISCARDING
    };
    public GameState gameState { get; private set; }
    

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

        if (input != null
            && (this.currentGameObjectHit != this.lastGameObjectHit
            || lastInputType == Inputs.InputTypes.BUTTON
            || (currentGameObjectHit.name.Length >= 11
                && currentGameObjectHit.name.Substring(0, 11) == "Player0Card")))
        {
            Debug.Log("Card: " + input.name + " Sprite: " + input.GetComponent<SpriteRenderer>().sprite);

            this.lastGameObjectHit = this.currentGameObjectHit;
            this.lastInputType = Inputs.InputTypes.GAME_OBJECT;

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

        if ((this.currentButtonHit != this.lastButtonHit
            || lastInputType == Inputs.InputTypes.GAME_OBJECT)
            && input != null)
        {
            this.lastButtonHit = input;
            this.lastInputType = Inputs.InputTypes.BUTTON;
            Debug.Log("Button Clicked: " + input.getText());
            
            //Lower any raised cards if the player isn't placing a contract
            if (gameState != GameState.PLACE_SET && gameState != GameState.PLACE_RUN)
            {
                for (int i = 0; i < playerList[0].hand.Count; i++)
                    if (playerList[0].hand[i].locationTag != Card.LOCATIONTAGS.DEFAULT)
                        playerList[0].hand[i].setLocationTag(Card.LOCATIONTAGS.DEFAULT);
            }

            if (input.getText() == Drawing.sortSuitButtonName)
            {
                Debug.Log(gameState);
                playerList[0].sortBySuit();
            }

            else if (input.getText() == Drawing.sortValueButtonName)
            {
                playerList[0].sortByValue();
            }

            //Note the contract button is used to enter contract placement mode
            //and to confirm a selected run or set to place
            else if (input.getText() == Drawing.contractButtonName)
            {
                if (playerList[0].hasContract() && gameState == GameState.CONTRACT)
                {
                    _handlePlaceContract();
                }

                if (playerList[0].hasPlacedContract() 
                    && playerList[0].hasBonusContract()
                    && (gameState == GameState.CONTRACT || gameState == GameState.DISCARDING))
                {
                    _handlePlaceContract();
                }
            }

            else if (input.getText() == Drawing.setButtonName)
            {
                if (gameState == GameState.PLACE_SET)
                {
                    if (playerList[0].hasPlacedContract())
                    {
                        gameState = GameState.DISCARDING;
                    }
                    if (selectedSetIsValid())
                    {
                        Debug.Log("Valid Set");
                        _placeSelectedCards();
                    }
                    else
                        Debug.Log("Selected set is not valid");
                }
            }

            else if (input.getText() == Drawing.runButtonName)
            {
                if (gameState == GameState.PLACE_RUN)
                {
                    if (selectedSetIsValid())
                    {
                        Debug.Log("Valid Run");
                    }
                    else
                        Debug.Log("Selected run is not valid");
                }
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

    private void _handleDiscard(GameObject currentGameObjectHit)
    {
        string cardNumberAsString;
        int cardNumber;

        try
        {
            cardNumberAsString = currentGameObjectHit.name.Substring(11);
            cardNumber = int.Parse(cardNumberAsString);
        }
        catch
        {
            Debug.Log(currentGameObjectHit.name + " cannot be discarded");
            return;
        }

        if (currentGameObjectHit.name.Substring(0, 11) == "Player0Card")
        {
            //if the player has their contract and they can discard a card onto another player's
            //contract, forcibly do that
            for (int i = 0; playerList[0].hasPlacedContract() && i < numPlayers; i++)
            {
                if (playerList[i].canAddToContract(playerList[playerTurn].hand[cardNumber]))
                {
                    playerList[i].addToContract(playerList[playerTurn].hand[cardNumber]);
                    playerList[0].hand.RemoveAt(cardNumber);
                    return;
                }
            }

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

        int selectedCard = -1;

        //if (!(selectedCards().Count >= setLength)
            //&& selectedSetIsValid())
        //{
            //TODO: Implement taking contracts into their own piles
            if (currentGameObjectHit != null
                && currentGameObjectHit.name.Length >= 11
                && currentGameObjectHit.name.Substring(0, 11) == "Player0Card")
            {
                selectedCard = int.Parse(currentGameObjectHit.name.Substring(11));

                //Toggle if the card is raised or not.
                if (playerList[0].hand[selectedCard].locationTag == Card.LOCATIONTAGS.DEFAULT)
                    playerList[0].hand[selectedCard].setLocationTag(Card.LOCATIONTAGS.DRAWN);
                else if (playerList[0].hand[selectedCard].locationTag == Card.LOCATIONTAGS.DRAWN)
                    playerList[0].hand[selectedCard].setLocationTag(Card.LOCATIONTAGS.DEFAULT);
                
            }
        //}
        //else
        //{
            
        //}
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
        else if (playerList[0].hasPlacedContract() && playerList[0].hasBonusContract())
        {
            Debug.Log("Player may place additional sets");
            gameState = GameState.CONTRACT;
        }
        else
            gameState = GameState.DISCARDING;
    }

    /// <summary>
    /// The hand index of the cards that are selected
    /// </summary>
    /// <returns></returns>
    private List<int> selectedCards()
    {
        List<int> cards = new List<int>();

        for (int i = 0; i < playerList[0].hand.Count; i++)
        {
            if (playerList[0].hand[i].locationTag == Card.LOCATIONTAGS.DRAWN)
            {
                cards.Add(i);
            }
        }

        return cards;
    }
    /// <summary>
    /// TODO: Make this ensure the set is as long as contract requires.
    /// TODO: Make this function use the selectedCards() function
    /// </summary>
    private bool selectedSetIsValid()
    {
        List<int> selectedSet = new List<int>();
        for (int i = 0; i < playerList[0].hand.Count; i++)
        {
            if (playerList[0].hand[i].locationTag == Card.LOCATIONTAGS.DRAWN)
            {
                if (selectedSet.Count > 1)
                {
                    if (playerList[0].hand[i].value != selectedSet[0])
                        return false;
                }
                selectedSet.Add(playerList[0].hand[i].value);
            }
        }

        if (selectedSet.Count < 3)
            return false;
        
        return true;
    }

    private void _placeSelectedCards()
    {
        List<Card> cards = new List<Card>();
        for (int i = 0; i < selectedCards().Count; i++)
        {
            cards.Add(new Card(playerList[0].hand[selectedCards()[i]]));
        }
        playerList[0].sets.Add(cards);
        for (int i = selectedCards().Count - 1; i >= 0; i--)
        {
            playerList[0].hand.RemoveAt(selectedCards()[i]);
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
