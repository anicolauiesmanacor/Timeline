using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
    //MENU
    public bool isMusicEnabled;
    public bool isFXEnabled;
    public float musicVolume;
    public float fxVolume;
    public int difficulty;

    //GAME
    public int cardsPlaced;
    public bool isCardInGame;
    public bool isPlacedInCardSpot;
    public bool isGameSolved;
    public int cardsCount;
    public int cardsInLine;
    public List<List<int>> cardYearList;
    public int currentLevel;
    public List<int> cardsSortedByYear;
    public List<GameObject> cardsInTimeLine;
    public GameObject sndManager;
    public int numLifes;

    public GameObject lifesIcon;
    public Sprite lifeGreen;
    public Sprite lifeRed;
    public bool gameOver;
    public GameObject LifeBackPanel;

    // Start is called before the first frame update
    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("gameLogic");
        if (objs.Length > 1)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        currentLevel = 0;
        cardYearList = new List<List<int>> {   
            new List<int>() { 1773, 1776, 1751, 1700, 1707, 1713, 1715, 1778 },
            new List<int>() { 1789, 1790, 1791, 1792, 1793, 1794, 1797, 1799, 1801, 1804, 1805, 1808, 1815, 1821, 1814, 1830, 1829, 1871 }, 
            new List<int>() { 1764, 1837, 1868, 1868, 1831, 1764, 1776, 1880 },
            new List<int>() { 1812, 1807, 1808, 1814, 1823, 1830, 1811, 1746, 1814, 1833, 1844, 1854, 1873, 1885, 1894, 1898, 1833 },
            new List<int>() { 1903, 1885, 1881, 1898 },
            new List<int>() { 1906, 1914, 1914, 1916, 1918, 1917, 1919, 1917, 1918, 1922, 1924, 1927 } ,
            new List<int>() { 1919, 1929, 1932, 1919, 1921, 1924, 1918, 1933, 1933, 1934, 1933, 1938, 1940, 1929 },
            new List<int>() { 1902, 1912, 1909, 1923, 1921, 1930, 1931, 1936, 1937, 1938, 1939 },
            new List<int>() { 1939, 1936, 1941, 1944, 1944, 1945, 1945, 1948},
            new List<int>() { 1947, 1949, 1955, 1961, 1957, 1960, 1953, 1959, 1962, 1985, 1989, 1955, 1948, 1964},
            new List<int>() { 1955, 1952, 1953, 1941, 1962, 1969, 1973, 1975, 1973, 1975, 1976, 1977, 1978, 1981, 1982, 1983},
            new List<int>() { 2001, 2003, 1992, 2008, 2004, 1998, 2002},
        };

        loadSettings();
        initSound();
    }

    public void initGame() {
        cardsSortedByYear = new List<int>();
        cardsInTimeLine = new List<GameObject>();
        gameOver = isCardInGame = isPlacedInCardSpot = isGameSolved = false;
        cardsPlaced = cardsCount = cardsInLine = 0;
        numLifes = 3;
        if (difficulty != 0) {
            lifesIcon.transform.GetChild(0).gameObject.active = true;
            lifesIcon.transform.GetChild(1).gameObject.active = true;
            lifesIcon.transform.GetChild(2).gameObject.active = true;
            LifeBackPanel.active = true;
            lifesIcon.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = lifeGreen;
            lifesIcon.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = lifeGreen;
            lifesIcon.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = lifeGreen;
        } else {
            lifesIcon.transform.GetChild(0).gameObject.active = false;
            lifesIcon.transform.GetChild(1).gameObject.active = false;
            lifesIcon.transform.GetChild(2).gameObject.active = false;
            LifeBackPanel.active = false;
        }
    }

    void loadSettings() {
        if (PlayerPrefs.HasKey("isMusicEnabled")) {
            if (PlayerPrefs.GetInt("isMusicEnabled") == 1) {
                isMusicEnabled = true;
            } else {
                isMusicEnabled = false;
            }
        } else {
            isMusicEnabled = true;
        }

        if (PlayerPrefs.HasKey("isFXEnabled")) {
            if (PlayerPrefs.GetInt("isFXEnabled") == 1) {
                isFXEnabled = true;
            } else {
                isFXEnabled = false;
            }
        } else {
            isFXEnabled = true;
        }

        if (PlayerPrefs.HasKey("musicVolume")) {
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
        } else {
            musicVolume = 1;
        }
        sndManager.GetComponent<SoundManager>().setMusicVolume(musicVolume);

        if (PlayerPrefs.HasKey("fxVolume")) {
            fxVolume = PlayerPrefs.GetFloat("musicVolume");
        } else {
            fxVolume = 1;
        }
        sndManager.GetComponent<SoundManager>().setFXVolume(fxVolume);

        if (PlayerPrefs.HasKey("difficulty")) {
            difficulty = PlayerPrefs.GetInt("difficulty");
        } else {
            difficulty = 0;
        }
    }

    void initSound() { 
        sndManager = GameObject.Find("SoundManager");
        GameObject sndSource = GameObject.Find("SoundSource");
        if (!sndSource.GetComponent<AudioSource>().isPlaying) {
            sndManager.GetComponent<SoundManager>().PlayMusic(sndManager.GetComponent<SoundManager>().menuMusic);
        }
    }

    public void addCardinTimeLine(GameObject GO) {
        //Check if card is dropped in the correct place
        checkPlacement(GO);
    }

    void checkPlacement(GameObject GO) {
        //FIRST CARD
        if (cardsInTimeLine.Count == 0) {
            cardsInTimeLine.Add(GO);
            GO.GetComponent<cardInGame>().posInTimeLine = cardsInTimeLine.Count;
            GO.GetComponent<cardInGame>().isCorrectlyPlacedInTimeLine = true;
            GO.GetComponent<cardInGame>().isCollidingTimeLine = true;
            isCardInGame = false;
            sndManager.GetComponent<SoundManager>().Play(sndManager.GetComponent<SoundManager>().correctFX);

        //SECOND CARD
        } else if (cardsInTimeLine.Count == 1 && !GO.GetComponent<cardInGame>().isCorrectlyPlacedInTimeLine) {
            
            if (cardsInTimeLine[0].gameObject.transform.position.x < GO.transform.position.x) {
                if (cardsInTimeLine[0].GetComponent<cardInGame>().year <= GO.GetComponent<cardInGame>().year) {
                    cardsInTimeLine.Add(GO);
                    GO.GetComponent<cardInGame>().posInTimeLine = cardsInTimeLine.Count;
                    GO.GetComponent<cardInGame>().isCorrectlyPlacedInTimeLine = true;
                    isCardInGame = false;
                    sndManager.GetComponent<SoundManager>().Play(sndManager.GetComponent<SoundManager>().correctFX);
                } else {
                    GO.GetComponent<cardInGame>().isCorrectlyPlacedInTimeLine = false;
                    GO.GetComponent<cardInGame>().isCollidingTimeLine = false;
                    isCardInGame = true;
                    sndManager.GetComponent<SoundManager>().Play(sndManager.GetComponent<SoundManager>().incorrectFX);
                }
            } else {
                if (cardsInTimeLine[0].GetComponent<cardInGame>().year >= GO.GetComponent<cardInGame>().year) {
                    cardsInTimeLine.Insert(0,GO);
                    GO.GetComponent<cardInGame>().posInTimeLine = 0;
                    cardsInTimeLine[1].GetComponent<cardInGame>().posInTimeLine = cardsInTimeLine.Count;
                    GO.GetComponent<cardInGame>().isCorrectlyPlacedInTimeLine = true;
                    isCardInGame = false;
                    sndManager.GetComponent<SoundManager>().Play(sndManager.GetComponent<SoundManager>().correctFX);
                    if (cardsInTimeLine.Count > 8)
                        Camera.main.orthographicSize += 1.2f;
                }
            } 
        } else if (cardsInTimeLine.Count >= 2 && !GO.GetComponent<cardInGame>().isCorrectlyPlacedInTimeLine) {
            //FIRST
            if (cardsInTimeLine[0].gameObject.transform.position.x > GO.transform.position.x) {
                if (cardsInTimeLine[0].GetComponent<cardInGame>().year >= GO.GetComponent<cardInGame>().year) {
                    cardsInTimeLine.Insert(0, GO);
                    GO.GetComponent<cardInGame>().isCorrectlyPlacedInTimeLine = true;
                    isCardInGame = false;

                    for (int i = 0; i < cardsInTimeLine.Count; i++) {
                        cardsInTimeLine[i].GetComponent<cardInGame>().posInTimeLine = i;
                    }
                    sndManager.GetComponent<SoundManager>().Play(sndManager.GetComponent<SoundManager>().correctFX);
                    if (cardsInTimeLine.Count > 8)
                        Camera.main.orthographicSize += 1.2f;
                }
            //LAST
            } else if (cardsInTimeLine[cardsInTimeLine.Count - 1].gameObject.transform.position.x < GO.transform.position.x) {
                if (cardsInTimeLine[cardsInTimeLine.Count-1].GetComponent<cardInGame>().year <= GO.GetComponent<cardInGame>().year) {
                    cardsInTimeLine.Add(GO);
                    cardsInTimeLine[cardsInTimeLine.Count - 1].GetComponent<cardInGame>().posInTimeLine = cardsInTimeLine.Count - 1;
                    GO.GetComponent<cardInGame>().isCorrectlyPlacedInTimeLine = true;
                    isCardInGame = false;
                    sndManager.GetComponent<SoundManager>().Play(sndManager.GetComponent<SoundManager>().correctFX);
                    if (cardsInTimeLine.Count > 8)
                        Camera.main.orthographicSize += 1.2f;
                }
            //MIDDLE
            } else {
                for (int i = 0; i < cardsInTimeLine.Count-1; i++) {
                    if ((cardsInTimeLine[i].gameObject.transform.position.x < GO.transform.position.x) &&
                        (cardsInTimeLine[i+1].gameObject.transform.position.x > GO.transform.position.x)) {
                        if ((cardsInTimeLine[i].GetComponent<cardInGame>().year <= GO.GetComponent<cardInGame>().year) &&
                            (cardsInTimeLine[i+1].GetComponent<cardInGame>().year >= GO.GetComponent<cardInGame>().year)) {
                            cardsInTimeLine.Insert(i+1, GO);
                            GO.GetComponent<cardInGame>().isCorrectlyPlacedInTimeLine = true;
                            isCardInGame = false;
                            cardsInTimeLine[i+1].GetComponent<cardInGame>().posInTimeLine = i+1;
                            for (int j = i+1; j < cardsInTimeLine.Count; j++) {
                                cardsInTimeLine[j].GetComponent<cardInGame>().posInTimeLine = j;
                            }
                            sndManager.GetComponent<SoundManager>().Play(sndManager.GetComponent<SoundManager>().correctFX);
                            if (cardsInTimeLine.Count > 8)
                                Camera.main.orthographicSize += 1.2f;
                            break;
                        }
                    }
                }
            }
        }

        if (GO.GetComponent<cardInGame>().isCorrectlyPlacedInTimeLine) {
            //placing in TimeLine
            setPositionCardsInTimeLine(GO);
        } else if (!GO.GetComponent<cardInGame>().isCorrectlyPlacedInTimeLine) {
            isCardInGame = true;
            GO.GetComponent<cardInGame>().isCollidingTimeLine = false;
            GameObject cardSpot = GameObject.Find("CardSpot");
            GO.gameObject.transform.position = new Vector3(cardSpot.transform.position.x, cardSpot.transform.position.y, -1);
            sndManager.GetComponent<SoundManager>().Play(sndManager.GetComponent<SoundManager>().incorrectFX);
            //LIFE LESS
            if (difficulty != 0) {
                numLifes -= 1;
                GameObject go = GameObject.Find("Lifes");
                go.transform.GetChild(numLifes).gameObject.GetComponent<Image>().sprite = lifeRed;
                Debug.Log("red");
            }
        }

        //WIN
        if (cardsInTimeLine.Count >= cardYearList[currentLevel].Count || cardsInTimeLine.Count >= 8) {
            gameOver = true;
            GameObject panWinner = GameObject.Find("PanWinner");
            GameObject canvas = GameObject.Find("Canvas");
            panWinner.transform.position = new Vector2(canvas.transform.position.x, canvas.transform.position.y);
            sndManager.GetComponent<SoundManager>().Play(sndManager.GetComponent<SoundManager>().winnerFX);
            if (cardsInTimeLine.Count > 8)
                Camera.main.orthographicSize += 1.2f;
        }

        //LOST
        if (numLifes <= 0) {
            gameOver = true;
            GameObject panLost = GameObject.Find("PanLost");
            GameObject canvas = GameObject.Find("Canvas");
            panLost.transform.position = new Vector2(canvas.transform.position.x, canvas.transform.position.y);
            sndManager.GetComponent<SoundManager>().Play(sndManager.GetComponent<SoundManager>().lostFX);
        }
    }

    void setPositionCardsInTimeLine(GameObject GO) {
        GameObject line = GameObject.Find("Line");
        
        float xInit = line.transform.position.x - (float)(cardsInTimeLine.Count / 2) * ((GO.GetComponent<RectTransform>().rect.width / 5)*2);
        for (int i = 0; i < cardsInTimeLine.Count; i++) {
            float x = xInit + (i * (GO.GetComponent<RectTransform>().rect.width/5)*2);
            cardsInTimeLine[i].gameObject.transform.position = new Vector3(x, line.transform.position.y, -5);
        }
    }
}