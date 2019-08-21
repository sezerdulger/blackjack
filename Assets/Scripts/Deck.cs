using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {
    List<int> cards = null;

    public bool isGameDeck;

    public bool HasCards
    {
        get { return cards != null && cards.Count > 0; }
    }

    public event CardEventHandler CardRemoved;
    public event CardEventHandler CardAdded;

    public int CardCount
    {
        get
        {
            if (cards == null)
            {
                return 0;
            }
            else
            {
                return cards.Count;
            }
        }
    }

    public IEnumerable<int> GetCards()
    {
        foreach (int i in cards)
        {
            yield return i;
        }
    }

    public int HandValue()
    {
        int total = 0;
        int aces = 0;
        foreach (int card in GetCards())
        {
            // 0 2
            // 1 3
            // 2 4
            // 3 5
            int cardRank = card % 13;
            

            if (cardRank > 0 && cardRank <= 9)
            {
                cardRank += 1;
                total += cardRank;
            }
            else if (cardRank > 9 && cardRank <= 12)
            {
                cardRank = 10;
                total += cardRank;
            }
            else
            {
                aces++;
            }
            
        }

        // TODO this is blackjack calculation
        for (int i = 0; i < aces; i++)
        {
            if (total + 11 < 21)
            {
                total += 11;
            }
            else
            {
                total += 1;
            }
        }
        return total;
    }

    public int Pop()
    {
        int temp = cards[0];
        cards.RemoveAt(0);

        if (CardRemoved != null)
        {
            CardRemoved(this, new CardEventArgs(temp));
        }
        return temp;
    }

    public void Push(int card)
    {
        cards.Add(card);

        if (CardAdded != null)
        {
            CardAdded(this, new CardEventArgs(card));
        }
    }

    public void Shuffle()
    {
        cards.Clear();
        for (int i = 0; i < 52; i++)
        {
            cards.Add(i);
        }
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;
        }
    }

	// Use this for initialization
	void Awake () {
        cards = new List<int>();
        if (isGameDeck)
        {
            Shuffle();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reset()
    {
        cards.Clear();
    }
}
