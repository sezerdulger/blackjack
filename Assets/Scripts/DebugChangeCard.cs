using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugChangeCard : MonoBehaviour {
    CardModel cardModel;
    int cardIndex = 0;
    CardFlipper flipper;

    public GameObject card;
    public Text DisplayName;
    public Button HitMeButton;
    public Button LogoutButton;

    void Awake()
    {
        cardModel = card.GetComponent<CardModel>();
        flipper = card.GetComponent<CardFlipper>();

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
            //FB.API("/me", Facebook.Unity.HttpMethod.GET, meCallback);
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    void meCallback(IGraphResult result)
    {
        Text displayName = DisplayName.GetComponent<Text>();
        displayName.text = "test";
        string response= "";
        if (result.Error != null)
        {
            response = "Error Response:\n" + result.Error;
            displayName.text = response;
            Debug.Log(response);
        }
        else if (!FB.IsLoggedIn)
        {
            response = "Login cancelled by Player";
            displayName.text = response;
            Debug.Log(response);
        }
        else
        {
            string fbname = result.ResultDictionary["name"].ToString();
            displayName.text = fbname;
            Debug.Log(fbname);
        }
    }

    void Logout ()
    {
        FB.LogOut();
        SceneManager.LoadScene("Login");
    }

    void HitMe()
    {
        if (cardIndex >= cardModel.faces.Length)
        {
            cardIndex = 0;
            //cardModel.ToogleFace(false);

            flipper.FlipCard(cardModel.faces[cardModel.faces.Length - 1], cardModel.cardBack, -1);
        }
        else
        {
            if (cardIndex < 0)
            {
                flipper.FlipCard(cardModel.faces[cardIndex - 1], cardModel.faces[cardIndex], cardIndex);
            }
            else
            {
                flipper.FlipCard(cardModel.cardBack, cardModel.faces[cardIndex], cardIndex);
            }
            //cardModel.cardIndex = cardIndex;
            //cardModel.ToogleFace(true);
            cardIndex++;
        }
    }

    // Use this for initialization
    void Start()
    {
        Button btn1 = HitMeButton.GetComponent<Button>();
        btn1.onClick.AddListener(() => { HitMe(); });

        btn1 = LogoutButton.GetComponent<Button>();
        btn1.onClick.AddListener(() => { Logout(); });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
