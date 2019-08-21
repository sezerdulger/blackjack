using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Deck))]
public class DeckView : MonoBehaviour {
    Deck deck;

    public Vector3 start;
    public float cardOffset;
    public GameObject cardPrefab;
    public GameObject panel;

    public bool faceUp = false;
    public bool reverseLayerOrder = false;

    Dictionary<int, CardView> fetchedCards;

    public void Toggle(int card, bool isFaceUp)
    {
        fetchedCards[card].IsFaceUp = isFaceUp;
    }

    void Awake()
    {
        deck = GetComponent<Deck>();
        fetchedCards = new Dictionary<int, CardView>();
        ShowCards();

        deck.CardRemoved += deck_CardRemoved;
        deck.CardAdded += deck_CardAdded;
        
    }

    void deck_CardAdded(object sender, CardEventArgs e)
    {
        float co = cardOffset * deck.CardCount;
        Vector3 temp = start + new Vector3(co, 0f);
        
        float w = panel.GetComponent<RectTransform>().rect.width;
        

        float x = panel.transform.position.x;
        float y = panel.transform.position.y;

        Debug.Log("w " + w);
        Debug.Log("x " + x);
        temp.x = x;
        temp.y = y;
        temp += new Vector3(co, 0f);
        AddCard(temp, e.CardIndex, deck.CardCount, sender);
    }

    void deck_CardRemoved(object sender, CardEventArgs e)
    {
        if (fetchedCards.ContainsKey(e.CardIndex))
        {
            Destroy(fetchedCards[e.CardIndex].Card);
            fetchedCards.Remove(e.CardIndex);
        }
    }

    void Start()
    {
        ShowCards();
    }

    void Update()
    {
        ShowCards();
    }

    public void ShowCards()
    {
        int cardCount = 0;

        if (deck.HasCards)
        {
            foreach (int i in deck.GetCards())
            {
                float co = cardOffset * cardCount;
                Vector3 temp = start + new Vector3(co, 0f);

                float w = panel.GetComponent<RectTransform>().rect.width;
                float x = panel.transform.position.x;
                float y = panel.transform.position.y;

                temp.x = x;
                temp.y = y;
                temp += new Vector3(co, 0f);
                AddCard(temp, i, cardCount, deck);
                cardCount++;
            }
        }
    }

    void AddCard(Vector3 position, int cardIndex, int positionIndex, object sender)
    {
        if (fetchedCards.ContainsKey(cardIndex))
        {
            if (!faceUp)
            {
                CardModel model = fetchedCards[cardIndex].Card.GetComponent<CardModel>();
                model.ToggleFace(fetchedCards[cardIndex].IsFaceUp);
            }
            return;
        }
        GameObject cardCopy = (GameObject)Instantiate(cardPrefab, position, Quaternion.identity) as GameObject;
        //UnityEditor.Selection.activeObject = cardCopy;
        cardCopy.transform.SetParent(panel.transform, false);

        cardCopy.transform.position = position;
        cardCopy.transform.localScale = new Vector3(50f, 50f, 0f);
        //cardCopy.transform.parent = panel.transform;

        //Debug.Log("position " + position);

        CardModel cardModel = cardCopy.GetComponent<CardModel>();
        cardModel.cardIndex = cardIndex;
        cardModel.ToggleFace(faceUp);

        SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
        if (reverseLayerOrder)
        {
            spriteRenderer.sortingOrder = 51 - positionIndex;
        }
        else
        {
            spriteRenderer.sortingOrder = positionIndex;
        }
        

        fetchedCards.Add(cardIndex, new CardView(cardCopy));

        //Debug.Log("Hand value: " + deck.HandValue());
    }

    public void Clear()
    {
        deck.Reset();

        foreach(CardView view in fetchedCards.Values)
        {
            Destroy(view.Card);
        }
        fetchedCards.Clear();
    }
}
