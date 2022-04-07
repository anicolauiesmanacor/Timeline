using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardInGame : MonoBehaviour {
    public int year;
    public Sprite cardYear;
    public int posInTimeLine;
    //bool inPosition;

    bool isDraggable;
    bool isDragging;
    public bool isCollidingTimeLine;
    public bool isCorrectlyPlacedInTimeLine;

    GameObject card;
    GameObject game;
    GameObject line;
    GameObject cardSpot;
    Collider2D objectCollider;
    SpriteRenderer spriteRenderer;

    public float duration = 2;
    float totalWaitingTime = 0;

    // Start is called before the first frame update
    void Start() {
        game = GameObject.Find("_GameLogic");
        cardSpot = GameObject.Find("CardSpot");
        objectCollider = GetComponent<Collider2D>();
        isCorrectlyPlacedInTimeLine = isDraggable = isDragging = isCollidingTimeLine = false;
        game = GameObject.Find("_GameLogic");
        line = GameObject.Find("Line");
    }

    // Update is called once per frame
    void Update() {
        if (!game.GetComponent<Game>().gameOver)
            DragAndDrop();
    }

    void DragAndDrop() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !isCollidingTimeLine && game.GetComponent<Game>().isPlacedInCardSpot && !isCorrectlyPlacedInTimeLine) {
            //inPosition = true;
            if (objectCollider == Physics2D.OverlapPoint(mousePosition)) {
                isDraggable = true;
            } else {
                isDraggable = false;
            }

            if (isDraggable) {
                isDragging = true;
            }
        }

        if (isDragging) {
            this.transform.position = new Vector3 (mousePosition[0], mousePosition[1], -5);
        }

        if (Input.GetMouseButtonUp(0)) {
            line.GetComponent<SpriteRenderer>().color = Color.white;
            if (isCollidingTimeLine) {
                isCollidingTimeLine = false;
                game.GetComponent<Game>().addCardinTimeLine(this.gameObject);

                //Carta revers: mostra any
                if (game.GetComponent<Game>().difficulty == 2) { 
                    if (isCorrectlyPlacedInTimeLine) { 
                        SpriteRenderer sprRend = this.GetComponent<SpriteRenderer>();
                        sprRend.sprite = cardYear;
                    }
                } else {
                    SpriteRenderer sprRend = this.GetComponent<SpriteRenderer>();
                    sprRend.sprite = cardYear;
                }
            } else {
                if (!isCorrectlyPlacedInTimeLine)
                    this.transform.position = new Vector3(cardSpot.transform.position.x, cardSpot.transform.position.y, -1);
            }
            isDraggable = isDragging = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        //No hi ha cartes. Col·loca carta com a primera carta
        if (col.gameObject.name == "Line") {
            isCollidingTimeLine = true;
            col.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name == "Line") {
            isCollidingTimeLine = isCorrectlyPlacedInTimeLine = false;
            col.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void OnMouseOver() {
        if (!game.GetComponent<Game>().gameOver) { 
            //If your mouse hovers over the GameObject with the script attached, output this message
            totalWaitingTime += Time.deltaTime;

            if (totalWaitingTime >= duration && !isDragging && !isDraggable) {
                this.gameObject.transform.localScale = new Vector2(150, 150);
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -6);
            } else {
                this.gameObject.transform.localScale = new Vector2(54, 54);
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -5);
            }
        }
    }

    void OnMouseExit() {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        totalWaitingTime = 0;
        this.gameObject.transform.localScale = new Vector2(54, 54);
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -5);
    }

}