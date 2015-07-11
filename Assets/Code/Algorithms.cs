using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Algorithms {

    public Algorithms() { }

    public static List<Card> sortByValue(List<Card> cards)
    {
        List<Card> newHand = new List<Card>();
        List<Card> originalHand = new List<Card>();
        originalHand.AddRange(cards);
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
                    compareCard.set(card);
                    lowestCardIndex = i;
                }
            }

            newHand.Add(originalHand[lowestCardIndex]);
            originalHand.RemoveAt(lowestCardIndex);
        }
        return newHand;
    }

    //Returns a list of only the specified portion sorted
    public static List<Card> sortByValue(List<Card> originalHand, int startIndex, int endIndex)
    {
        int originalHandLength = originalHand.Count;
        List<Card> newHand = new List<Card>();
        newHand.AddRange(originalHand);

        newHand.RemoveRange(endIndex, originalHand.Count - endIndex);
        newHand.RemoveRange(0, startIndex);

        newHand = sortByValue(newHand);
        return newHand;
    }

    public static List<Card> sortBySuit(List<Card> originalHand)
    {
        List<Card> newHand = new List<Card>();
        int[] cardsOfSuit = { 0, 0, 0, 0 };
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
        }

        List<Card> tempHand = new List<Card>();
        //sort each suit section by value
        tempHand.AddRange(sortByValue(newHand, 0, cardsOfSuit[0]));
        tempHand.AddRange(sortByValue(newHand, cardsOfSuit[0], (cardsOfSuit[0] + cardsOfSuit[1])));
        tempHand.AddRange(sortByValue(newHand, cardsOfSuit[0] + cardsOfSuit[1], (cardsOfSuit[0] + cardsOfSuit[1] + cardsOfSuit[2])));
        tempHand.AddRange(sortByValue(newHand, cardsOfSuit[0] + cardsOfSuit[1] + cardsOfSuit[2], (cardsOfSuit[0] + cardsOfSuit[1] + cardsOfSuit[2] + cardsOfSuit[3])));
        newHand = tempHand;

        return newHand;
    }

    public static int numWildCards(List<Card> cards)
    {
        int returnValue = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].value == 2)
                returnValue++;
        }
        return returnValue;
    }

    public static bool hasContract(List<Card> cards, int contractNumber)
    {
        switch (contractNumber)
        {
            //note contract "0" represents having extra contract cards
            case 0:
                return hasSet(cards);
            case 1:
                return hasSet(cards, 2);

            //TODO: Implement rest of cases
            case 2:
                return false;
            case 3:
                return false;
            case 4:
                return false;
            case 5:
                return false;
            case 6:
                return false;
            case 7:
                return false;
            default:
                throw new UnityException("A player has reached an illegal contract number");
        }
    }

    public static bool hasSet(List<Card> cards, int numSets = 1, int setSize = 3)
    {
        List<Card> sortedCards = sortByValue(cards);
        List<Card> tempCards = new List<Card>(sortedCards);
        int lastLength = sortedCards.Count;

        for (int i = 0; i < numSets; i++)
        {
            tempCards = _setHelper(tempCards, setSize);
            if (tempCards.Count == lastLength)
            {
                return false;
            }
            else
            {
                lastLength = tempCards.Count;
            }
        }

        return true;
    }

    //TODO: Make this function report back that it used a two to it's caller. (ie the case where
    //there is one 2 and two doubles in your hand hasSet will mistakenly say you have a contract

    /// <summary>
    /// Tries to find a set in the cards. First without using 2s then, using one more 2 at a time
    /// </summary>
    /// <param name="sortedCards">The cards to look through. They must be pre sorted by value</param>
    /// <param name="setSize">Size of the set to look for</param>
    /// <param name="numTwosAllowed">Number of twos in the provided cards that the algorithm
    /// can use.</param>
    /// <returns>If no set was found, it returns the inputted cards unchanged. If a set
    /// was found it removes it, and then returns the inputted cards without the set</returns>
    private static List<Card> _setHelper(List<Card> sortedCards, int setSize, int numTwosAllowed=0)
    {
        int baseSetLength = 1 + numTwosAllowed;
        int lengthOfCurrentSet = baseSetLength;
        int indexOfCurrentSet = 0;

        for (int i = 0; i < sortedCards.Count - 1; i++)
        {
            if (sortedCards[i].value == 2)
            {
                indexOfCurrentSet = i + 1;
                continue;
            }

            if (sortedCards[i + 1].value == sortedCards[i].value)
            {
                if (lengthOfCurrentSet < setSize)
                {
                    lengthOfCurrentSet++;
                }

                if (lengthOfCurrentSet >= setSize)
                {
                    try
                    {
                        sortedCards.RemoveRange(indexOfCurrentSet, (lengthOfCurrentSet - baseSetLength) + 1);
                    }
                    catch (UnityException ex)
                    {
                        throw new UnityException("Tried to remove an invalid range in the hasSet algorithm");
                    }
                    return new List<Card>(sortedCards);
                }
                else
                    continue;
            }

            else
            {
                lengthOfCurrentSet = baseSetLength;
                indexOfCurrentSet = i + 1;
            }
        }

        if (numWildCards(sortedCards) > numTwosAllowed && numTwosAllowed + 1 <= setSize)
        {
            return _setHelper(sortedCards, setSize, numTwosAllowed + 1);
        }
        else
            return sortedCards;
    }
}
