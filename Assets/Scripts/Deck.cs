using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deck : MonoBehaviour {
    List<int> cardsOrder;
    GameObject game;
    GameObject cardSpot;
    public GameObject prefabCard;
    public List<Sprite> CardsA;
    public List<Sprite> CardsB;

    public void Shuffle() {
        int cardsCount = game.GetComponent<Game>().cardsCount;
        
        cardsOrder = new List<int>();

        for (int i = 0; i < cardsCount; i++) {
            cardsOrder.Add(i);
        }

        int n = cardsCount;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            int temp = cardsOrder[k];
            cardsOrder[k] = cardsOrder[n];
            cardsOrder[n] = temp;
        }
    }

    void OnMouseDown() {
        if (game.GetComponent<Game>().cardsInLine < game.GetComponent<Game>().cardsCount && !game.GetComponent<Game>().isCardInGame) {
            prefabCard.GetComponent<SpriteRenderer>().sprite = CardsA[cardsOrder[game.GetComponent<Game>().cardsInLine]];
            prefabCard.GetComponent<cardInGame>().cardYear = CardsB[cardsOrder[game.GetComponent<Game>().cardsInLine]];
            prefabCard.GetComponent<cardInGame>().year = game.GetComponent<Game>().cardYearList[game.GetComponent<Game>().currentLevel][cardsOrder[game.GetComponent<Game>().cardsInLine]];
            game.GetComponent<Game>().cardsInLine += 1;
            game.GetComponent<Game>().isCardInGame = true;
            GameObject myCard = Instantiate(prefabCard, cardSpot.transform.position, Quaternion.identity);
            myCard.transform.SetParent(GameObject.Find("Canvas").transform, false);
            myCard.transform.position = new Vector2 (myCard.transform.position.x, myCard.transform.position.y);

        }
    }

    void OnMouseUp() {
        game.GetComponent<Game>().isPlacedInCardSpot = true;
    }

    void Start() {
        Scene scene = SceneManager.GetActiveScene();
        cardSpot = GameObject.Find("CardSpot");
        game = GameObject.Find("_GameLogic");
        game.GetComponent<Game>().currentLevel = int.Parse(scene.name)-1;
        game.GetComponent<Game>().isCardInGame = false;
        game.GetComponent<Game>().cardsCount = CardsA.Count;
        Shuffle();
    }
}