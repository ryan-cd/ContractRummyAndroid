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

    public void handleInput(GameObject gameObject, ButtonWrapper button)
    {
        _handleInputGameObject(gameObject);
        _handleInputButton(button);
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
                _sortHand(0, SORTS.SUIT);
            }

            else if (input.getText() == Drawing.sortValueButtonName)
            {
                _sortHand(0, SORTS.VALUE);
            }
        }
    }

    /*
     * INPUT HANDLING
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     * */
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

        if (currentGameObjectHit.name == "DiscardPile" && discardList.Count > 0)
        {
            playerList[playerTurn].drawCard(discardList[discardList.Count - 1]);
            discardList.RemoveAt(discardList.Count - 1);
            gameState = GameState.DISCARDING;
        }

		//_sortHand(0, SORTS.VALUE);
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

    private void _checkHands()
    {
        
    }

	private void _sortHand(int player, SORTS sortType)
	{
		if(playerList[player].hand.Count < 1)
			return;

		List<Card> originalHand = playerList[player].hand; 
		List<Card> newHand = new List<Card>();

		if(sortType == SORTS.VALUE)
		{
            newHand = _sortByValue(originalHand);
		}

        if (sortType == SORTS.SUIT)
        {
            newHand = _sortBySuit(originalHand);
        }

		playerList[player].hand = newHand;
	}

    private List<Card> _sortByValue(List<Card> originalHand)
    {
        List<Card> newHand = new List<Card>();
        Card compareCard = new Card();
        int lowestCardIndex = 0;
        while (originalHand.Count > 0)
        {
            compareCard.setValue(14);
            for (int i = 0; i < originalHand.Count; i++)
            {
                Card card = originalHand[i];

                if (card.value <= compareCard.value)
                {
                    compareCard = card;
                    lowestCardIndex = i;
                }
            }

            newHand.Add(originalHand[lowestCardIndex]);
            originalHand.RemoveAt(lowestCardIndex);
        }
        return newHand;
    }

    //Returns a list of only the specified portion sorted
    private List<Card> _sortByValue(List<Card> originalHand, int startIndex, int endIndex)
    {
        int originalHandLength = originalHand.Count;
        List<Card> newHand = new List<Card>();
        newHand.AddRange(originalHand);

        newHand.RemoveRange(endIndex, originalHand.Count - endIndex);
        newHand.RemoveRange(0, startIndex);

        newHand = _sortByValue(newHand);
        return newHand;
    }

    private List<Card> _sortBySuit(List<Card> originalHand)
    {
        List<Card> newHand = new List<Card>();
        int[] cardsOfSuit = {0, 0, 0, 0};
        //set the cards to be grouped together by suit
        foreach (Card.SUITS suit in System.Enum.GetValues(typeof(Card.SUITS)))
        {
            for (int i = 0; i < originalHand.Count; i++)
            {
                if (originalHand[i].suit == suit)
                {
                    cardsOfSuit[(int)suit] += 1;
                    newHand.Add(new Card(originalHand[i]));
                }
            }
            Debug.Log(cardsOfSuit[(int)suit]);
        }

        List<Card> tempHand = new List<Card>();
        //sort each suit section by value
        tempHand.AddRange(_sortByValue(newHand, 0, cardsOfSuit[0]));
        tempHand.AddRange(_sortByValue(newHand, cardsOfSuit[0], (cardsOfSuit[0]+cardsOfSuit[1])));
        tempHand.AddRange(_sortByValue(newHand, cardsOfSuit[0]+cardsOfSuit[1], (cardsOfSuit[0]+cardsOfSuit[1]+cardsOfSuit[2])));
        tempHand.AddRange(_sortByValue(newHand, cardsOfSuit[0]+cardsOfSuit[1]+cardsOfSuit[2], (cardsOfSuit[0]+cardsOfSuit[1]+cardsOfSuit[2]+cardsOfSuit[3])));
        newHand = tempHand;

        return newHand;
    }
}
