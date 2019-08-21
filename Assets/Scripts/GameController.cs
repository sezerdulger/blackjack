using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    int dealersFirstCard = -1;

    public Deck player;
    public Deck dealer;
    public Deck deck;

    public Text resultTextObject;

    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    /*
     * Cards dealt to each player
     * First player hits/sticks/bust
     * Dealer's turn; must have minimum of 17 score hand
     * Dealers card; first card is hidden, subsequent cards are facing
     * 
     */

    #region Unit messages
    void StartGame()
    {
        

        for (int i = 0; i < 2; i++)
        {
            player.Push(deck.Pop());
            HitDealer();
        }

        //Vector3 temp = new Vector3(0.0f, 4.0f, 0.0f);
        //Debug.Log("dealer " + dealer.transform.position);
        //resultTextObject.transform.position = dealer.transform.position + temp;

        //Debug.Log("position " + resultTextObject.transform.position);
    }
    #endregion

    void HitDealer()
    {
        int card = deck.Pop();
        if (dealersFirstCard < 0)
        {
            dealersFirstCard = card;
        }

        dealer.Push(card);
        if (dealer.CardCount >= 2)
        {
            DeckView view = dealer.GetComponent<DeckView>();
            view.Toggle(card, true);
        }
    }

    #region Public methods
    public void Hit()
    {
        player.Push(deck.Pop());
        if (player.HandValue() > 21)
        {
            // TODO: Player is bust
            hitButton.interactable = false;
            stickButton.interactable = false;
            StartCoroutine(DealersTurn());
        }
    }

    public void Stick()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;
        
        // TODO: Dealer
        StartCoroutine(DealersTurn());
    }

    IEnumerator DealersTurn()
    {
        hitButton.interactable = false;
        stickButton.interactable = false;

        DeckView view = dealer.GetComponent<DeckView>();
        view.Toggle(dealersFirstCard, true);
        view.ShowCards();
        yield return new WaitForSeconds(1f);

        while (dealer.HandValue() < 17)
        {
            HitDealer();
            yield return new WaitForSeconds(1f);
        }

        if (player.HandValue() > 21 || (dealer.HandValue() > player.HandValue() && dealer.HandValue() <= 21))
        {
            resultTextObject.text = "Sorry you lose.";
        }
        else if (dealer.HandValue() > 21 || (player.HandValue() <= 21 && player.HandValue() > dealer.HandValue()))
        {
            resultTextObject.text = "You win.";
        }
        else
        {
            resultTextObject.text = "Dealer wins.";
        }
        yield return new WaitForSeconds(1f);
        playAgainButton.interactable = true;
    }

    public void PlayAgain()
    {
        playAgainButton.interactable = false;
        player.GetComponent<DeckView>().Clear();
        dealer.GetComponent<DeckView>().Clear();
        deck.GetComponent<DeckView>().Clear();
        deck.Shuffle();

        hitButton.interactable = true;
        stickButton.interactable = true;
        dealersFirstCard = -1;
        resultTextObject.text = "";
        StartGame();
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion

    // Use this for initialization
    void Start () {
        StartGame();
	}
	
	// Update is called once per frame
	void Update () {

    }

    void Awake()
    {
        
    }
}
