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

    public static bool hasContract(List<Card> cards, int contractNumber)
    {
        switch (contractNumber)
        {
            case 1:
                return hasContract1(cards);

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


    private static bool hasContract1(List<Card> cards)
    {
        List<Card> sortedCards = sortByValue(cards);
        int lengthOfCurrentSet = 1;
        int numberOfSets = 0;

        for (int i = 0; i < sortedCards.Count - 1; i++)
        {
            if (sortedCards[i + 1].value == sortedCards[i].value)
            {
                lengthOfCurrentSet++;
                continue;
            }
            else if (lengthOfCurrentSet >= 3)
            {
                numberOfSets++;
                lengthOfCurrentSet = 1;
            }
            else
            {
                lengthOfCurrentSet = 1;
            }
        }

        return (numberOfSets >= 2) ? true : false;
    }
}
