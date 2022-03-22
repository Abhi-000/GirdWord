using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    private float time = 2f;
    bool showed = false;
    bool playOnce = false;
    bool changed = false;
    bool startLerping = false;
    private float timer;
    public int swipes= 0;
    public Text swapesNo,keyNeeded,totalKeys,totalGems;
    private int gems,tempGems,tempCoins,tempKeys;
    private GameObject continueBtn, endGameBtn, buyUsingGemBtn, watchAdBtn,continueOrEnd,WatchOrBuy,allOutOfMoves,outOfGems,buyGems,cross;
    public Text[] wordToFind;
    public bool rowDestroyed, colDestroyed,decreaseRow,decreaseCol;
    int z = 0;
    public static GameManager instance;
    public int totalSwapes;
    public int column = 3;
    public int row = 3;
    public GameObject meaningPanel,meaningPrefab;
    public GameObject outOfMoves, outOfMovesPrefab;
    public GameObject grid,destroyEffect,box,cam,WinScrenn,fireworks,fireWorkLoc,nextBtn,restartBtn,canvas,winPrefab,topPanelPrefab,topPanel,loseScreenPrefab,loseScreen,bgPrefab;
    public GameObject boxes;
    public GameObject[,] allBoxes = new GameObject[9,9];
    public Vector3[,] allBoxLoc = new Vector3[9, 9];
    public string[] alphabetsUsed = { "O", "W", "N" };
    public string[] alphabetOrder = { "O", "W", "W", "N", "W", "N", "O", "N", "O" };
    public Material bgMat;
    public Material[] allMat;
    public List<string> hintWords = new List<string>();
    public List<string> oldHintWords = new List<string>();
    public string[] allWords = { "WON", "OWN", "NOW" };
    public List<string> foundWords = new List<string>();
    public List<int> meaningShown = new List<int>();
    public GameObject centerColumn, centerRow,lerpDown,lerpLeft;
    private GameObject background;
    private Slider gemCollected;
    private Image[] diamonds;
    private GameObject endScreen;
    private void Awake()
    {
        diamonds = new Image[allWords.Length];
        int btnCount;
        WinScrenn = Instantiate(winPrefab, canvas.transform);
        WinScrenn.SetActive(false);
        if(SceneManager.GetActiveScene().name == "Level1")
        GameObject.Find("Canvas/GameObject").transform.localPosition = new Vector3(GameObject.Find("Canvas/GameObject").transform.localPosition.x, 320, GameObject.Find("Canvas/GameObject").transform.localPosition.z);
        else
            GameObject.Find("Canvas/GameObject").transform.localPosition = new Vector3(GameObject.Find("Canvas/GameObject").transform.localPosition.x, 285, GameObject.Find("Canvas/GameObject").transform.localPosition.z);
        /*GameObject framePrefab,frame;
        framePrefab = Resources.Load("Frame") as GameObject;
        frame = Instantiate(framePrefab, new Vector3(0.2f,6.5f,-10.5f),new Quaternion(-270,0,0, -270));
        frame.SetActive(true);*/
        GameObject.Find("Directional Light").transform.position = new Vector3(6.8f, 8.4f, -5.3f);
        GameObject.Find("Directional Light").transform.rotation = Quaternion.Euler(97.3f, 8.4f, -5.3f);
        InitBG();
        InitTopPanel();
        InitLoseScreen();
        InitMeaningPanel();
        InitGetMoves();
        instance = this;
    }
    void InitGetMoves()
    {
        outOfMovesPrefab = Resources.Load("GetMovesPopUp") as GameObject;
        outOfMoves = Instantiate(outOfMovesPrefab, canvas.transform);
        outOfMoves.SetActive(false);
        cross = outOfMoves.transform.FindChild("Image/Cross").gameObject;
        buyGems = outOfMoves.transform.FindChild("Image/OutOfGems/BuyGems").gameObject;
        allOutOfMoves = outOfMoves.transform.Find("Image/AllOutOfMoves").gameObject;
        outOfGems = outOfMoves.transform.FindChild("Image/OutOfGems").gameObject;
        continueOrEnd = outOfMoves.transform.FindChild("Image/AllOutOfMoves/ContinueOrEnd").gameObject;
        WatchOrBuy = outOfMoves.transform.FindChild("Image/AllOutOfMoves/WatchOrBuy").gameObject;
        continueBtn = outOfMoves.transform.FindChild("Image/AllOutOfMoves/ContinueOrEnd/Continue").gameObject;
        endGameBtn = outOfMoves.transform.FindChild("Image/AllOutOfMoves/ContinueOrEnd/EndGame").gameObject;
        watchAdBtn = outOfMoves.transform.FindChild("Image/AllOutOfMoves/WatchOrBuy/WatchAd").gameObject;
        buyUsingGemBtn = outOfMoves.transform.FindChild("Image/AllOutOfMoves/WatchOrBuy/BuyUsingGem").gameObject;
        cross.GetComponent<Button>().onClick.AddListener(() => BackForOutOfGems());
        continueBtn.GetComponent<Button>().onClick.AddListener(() => Continue());
        endGameBtn.GetComponent<Button>().onClick.AddListener(() => EndGame());
        watchAdBtn.GetComponent<Button>().onClick.AddListener(() => WatchAd());
        buyUsingGemBtn.GetComponent<Button>().onClick.AddListener(() => BuyUsingGem());
        buyGems.GetComponent<Button>().onClick.AddListener(() => BuyGems());
    }
    void InitBG()
    {
        bgPrefab = Resources.Load("Bg") as GameObject;
        background = Instantiate(bgPrefab, new Vector3(0, 7.21f, -9.65f),Quaternion.identity);
        boxes = background.transform.GetChild(1).gameObject;
        for (int i = 0; i < boxes.transform.childCount-1; i++) {
            boxes.transform.GetChild(i).GetComponent<MeshRenderer>().material = allMat[i];
                }
        boxes.transform.GetChild(boxes.transform.childCount-1).GetComponent<MeshRenderer>().material = allMat[1];
        background.transform.GetChild(0).transform.GetComponent<MeshRenderer>().material = bgMat;
        gemCollected = background.transform.GetComponentInChildren<Slider>();
        for(int i=0;i<allWords.Length;i++)
        {
            diamonds[i] = background.transform.FindChild("Canvas/Slider/Diamonds").GetChild(i).GetComponent<Image>();
        }
        //diamonds = background.transform.GetComponentsInChildren<Image>();
        /*for (int i=0; i < allWords.Length; i++){
            diamonds[i] = GetComponentsInChildren
                }*/
    }
    void InitMeaningPanel()
    {
        meaningPrefab = Resources.Load("MeaningPanel") as GameObject;
        meaningPanel = Instantiate(meaningPrefab, background.transform.FindChild("Canvas").transform);
        meaningPanel.SetActive(false);
        meaningPanel.transform.FindChild("PopUp/CrossBtn").GetComponent<Button>().onClick.AddListener(() => Back());
    }
    void InitTopPanel()
    {
        topPanelPrefab = Resources.Load("TopPanel") as GameObject;
        topPanel = Instantiate(topPanelPrefab, canvas.transform);
        keyNeeded = topPanel.transform.FindChild("Hint/KeyVal").GetComponent<Text>();
        totalKeys = topPanel.transform.FindChild("Keys/TotalKeys").GetComponent<Text>();
        totalGems = topPanel.transform.FindChild("Gems/TotalGems").GetComponent<Text>();
        swapesNo = topPanel.transform.FindChild("Swaps/SwapesNo").GetComponent<Text>();
        topPanel.transform.FindChild("Hint/Button").GetComponent<Button>().onClick.AddListener(() => Hint());
    }
    void InitLoseScreen()
    {
        loseScreenPrefab = Resources.Load("LoseScreenNew") as GameObject;
        loseScreen = Instantiate(loseScreenPrefab, canvas.transform);
        loseScreen.SetActive(false);
        loseScreen.transform.FindChild("Home").transform.GetComponent<Button>().onClick.AddListener(() => Home());
        loseScreen.transform.FindChild("Restart").transform.GetComponent<Button>().onClick.AddListener(() => RestartGame());
    }
    private void Start()
    {
        lerpDown = GameObject.Find("LerpDown");
        lerpLeft = GameObject.Find("LerpLeft");
        keyNeeded.text = "1";
        totalKeys.text = PlayerPrefs.GetInt("Keys",100).ToString();
        totalGems.text = PlayerPrefs.GetInt("Gems", 0).ToString();
        gems = PlayerPrefs.GetInt("Gems", 0);
        swapesNo.text = totalSwapes.ToString();       
        float PosX =-8, PosY = -50f, PosZ = 6f;
        for (int i=0;i<row;i++)
        {
            PosX = -5.4f;
            for(int j=0; j<column;j++)
            {
                GameObject placedBox;
                placedBox= Instantiate(box, Vector3.zero, Quaternion.Euler(270,0,0),grid.transform);
                placedBox.transform.localPosition = new Vector3(PosX, PosY, PosZ);
                placedBox.transform.GetChild(0).transform.GetComponent<TextMeshPro>().text = alphabetOrder[z];
                placedBox.transform.name = "B" + i + j;
                for (int m = 0; m < alphabetsUsed.Length; m++)
                {
                    if(alphabetOrder[z] == alphabetsUsed[m])
                    {
                        placedBox.transform.GetComponent<MeshRenderer>().material = allMat[m];
                        placedBox.transform.tag = alphabetsUsed[m];
                    }
                }
                z++; 
                PosX += 1.4f;
            }
            PosZ -= 1.4f;
        }
        grid.transform.GetChild(0).SetSiblingIndex(grid.transform.childCount);
        grid.transform.GetChild(0).SetSiblingIndex(grid.transform.childCount);
        cam.GetComponent<Camera>().orthographicSize = row * 2;
        cam.transform.position = new Vector3(cam.transform.position.x + row - 3, cam.transform.position.y , cam.transform.position.z - (row - 3));
        int k = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                allBoxes[i, j] = grid.transform.GetChild(k).gameObject;
                allBoxLoc[i, j] = allBoxes[i, j].transform.localPosition;
                grid.transform.GetChild(k).GetComponent<BoxScript>().row = i;
                grid.transform.GetChild(k).GetComponent<BoxScript>().column = j;
                k++;
            }
        }

    }
    void Update()
    {
        if (startLerping)
        {
            startLerping = false;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (allBoxes[i, j].transform.GetComponent<BoxScript>().column == 0)
                    {
                        if (allBoxes[i, j].transform.GetComponent<BoxScript>().row == 0)
                            allBoxes[i, j].transform.position = Vector3.Lerp(allBoxes[i, j].transform.position, new Vector3(allBoxes[i, j].transform.position.x, allBoxes[i, j].transform.position.y, lerpDown.gameObject.transform.position.z), 0.1f);
                        else if (allBoxes[i, j].transform.GetComponent<BoxScript>().row == 1)
                            allBoxes[i, j].transform.position = Vector3.Lerp(allBoxes[i, j].transform.position, new Vector3(allBoxes[i, j].transform.position.x, allBoxes[i, j].transform.position.y, lerpDown.gameObject.transform.position.z - 2f), 0.1f);
                    }
                    else if (allBoxes[i, j].transform.GetComponent<BoxScript>().column == 1)
                    {
                        if (allBoxes[i, j].transform.GetComponent<BoxScript>().row == 0)
                            allBoxes[i, j].transform.position = Vector3.Lerp(allBoxes[i, j].transform.position, new Vector3(lerpLeft.transform.position.x, allBoxes[i, j].transform.position.y, allBoxes[i, j].transform.position.z), 0.1f);
                        else if (allBoxes[i, j].transform.GetComponent<BoxScript>().row == 1)
                            allBoxes[i, j].transform.position = Vector3.Lerp(allBoxes[i, j].transform.position, new Vector3(allBoxes[i, j].transform.position.x, allBoxes[i, j].transform.position.y, lerpDown.gameObject.transform.position.z - 2f), 0.1f);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!outOfMoves.activeSelf)
                Back();
            else
                BackForOutOfGems();
        }
        if(!meaningPanel.activeSelf)
        timer += Time.deltaTime;
    }
    #region SetupAfterWordFound
    public void Setup()
    {
        if (rowDestroyed)
        {
            rowDestroyed = false;
            row--;
            for (int i = 0; i < grid.transform.childCount - 2; i++)
            {
                if (grid.transform.GetChild(i).transform.GetComponent<BoxScript>().row == 2)
                {
                    grid.transform.GetChild(i).transform.GetComponent<BoxScript>().row = 1;
                }
                else if(grid.transform.GetChild(i).transform.GetComponent<BoxScript>().row == 1)
                {
                    grid.transform.GetChild(i).transform.GetComponent<BoxScript>().row = 0;
                }
            }
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int z = 0; z < grid.transform.childCount; z++)
                    {
                        if (grid.transform.GetChild(z) != null)
                        {
                            if (grid.transform.GetChild(z).GetComponent<BoxScript>() != null)
                            {
                                if (grid.transform.GetChild(z).GetComponent<BoxScript>().row == i && grid.transform.GetChild(z).GetComponent<BoxScript>().column == j)
                                {
                                    allBoxes[i, j] = grid.transform.GetChild(z).gameObject;
                                    allBoxLoc[i, j] = allBoxes[i, j].transform.localPosition;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
           
        }
        else if(colDestroyed)
        {
            colDestroyed = false;
            column--;
            for (int i = 0; i < grid.transform.childCount - 2; i++)
            {
                if (grid.transform.GetChild(i).transform.GetComponent<BoxScript>().column == 2)
                {
                    grid.transform.GetChild(i).transform.GetComponent<BoxScript>().column = 1;
                }
                else if (grid.transform.GetChild(i).transform.GetComponent<BoxScript>().column == 1)
                {
                    grid.transform.GetChild(i).transform.GetComponent<BoxScript>().column = 0;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    for (int z = 0; z < grid.transform.childCount; z++)
                    {
                        if (grid.transform.GetChild(z).GetComponent<BoxScript>() != null)
                        {
                            if (grid.transform.GetChild(z).GetComponent<BoxScript>().row == i && grid.transform.GetChild(z).GetComponent<BoxScript>().column == j)
                            {
                                allBoxes[i, j] = grid.transform.GetChild(z).gameObject;
                                allBoxLoc[i, j] = allBoxes[i, j].transform.localPosition;
                                break;
                            }
                        }
                    }
                }
            }
            
        }
        if (!changed)
        {
            changed = true;
            if (grid.transform.childCount == 6)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (allBoxes[i, j].transform.GetComponent<BoxScript>().column == 0)
                        {
                            startLerping = true;
                        }
                        else if (allBoxes[i, j].transform.GetComponent<BoxScript>().column == 1)
                        {
                            startLerping = true;
                        }
                    }
                }
                for (int k = 0; k < grid.transform.childCount - 2; k++)
                {
                    if (grid.transform.GetChild(k).transform.GetComponent<BoxScript>().column == 0)
                    {
                        grid.transform.GetChild(k).transform.GetComponent<BoxScript>().row++;
                        allBoxes[grid.transform.GetChild(k).transform.GetComponent<BoxScript>().row - 1, 0] = grid.transform.GetChild(k).gameObject;
                        allBoxLoc[grid.transform.GetChild(k).transform.GetComponent<BoxScript>().row - 1, 0] = allBoxes[grid.transform.GetChild(k).transform.GetComponent<BoxScript>().row - 1, 0].transform.localPosition;
                    }
                    else if (grid.transform.GetChild(k).transform.GetComponent<BoxScript>().column == 1)
                    {
                        if (grid.transform.GetChild(k).transform.GetComponent<BoxScript>().row == 0)
                        {
                            grid.transform.GetChild(k).transform.GetComponent<BoxScript>().column--;
                            allBoxes[0, grid.transform.GetChild(k).transform.GetComponent<BoxScript>().column + 1] = grid.transform.GetChild(k).gameObject;
                            allBoxLoc[0, grid.transform.GetChild(k).transform.GetComponent<BoxScript>().column + 1] = allBoxes[0, grid.transform.GetChild(k).transform.GetComponent<BoxScript>().column + 1].transform.localPosition;
                        }
                        else if (grid.transform.GetChild(k).transform.GetComponent<BoxScript>().row == 1)
                        {
                            grid.transform.GetChild(k).transform.GetComponent<BoxScript>().row++;
                            allBoxes[grid.transform.GetChild(k).transform.GetComponent<BoxScript>().row - 1, 1] = grid.transform.GetChild(k).gameObject;
                            allBoxLoc[grid.transform.GetChild(k).transform.GetComponent<BoxScript>().row - 1, 1] = allBoxes[grid.transform.GetChild(k).transform.GetComponent<BoxScript>().row - 1, 1].transform.localPosition;
                        }
                    }
                }
            }
        }
    }
    #endregion
    #region Hint
    public void Hint()
    {
        Debug.Log("HINT");
        bool found = false;
        if (foundWords.Count != 0)
        {
            for (int i = 0; i < allWords.Length; i++)
            {
                found = false;
                for (int j = 0; j < wordToFind.Length; j++)
                {
                    if (allWords[i] == wordToFind[j].text)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Debug.Log("NOT FOUND");
                    for (int k = 0; k < wordToFind.Length; k++)
                    {
                        if (!wordToFind[k].gameObject.activeSelf)
                        {
                            wordToFind[k].gameObject.SetActive(true);
                            wordToFind[k].text = allWords[i];
                            hintWords.Add(allWords[i]);
                            int hint,keyTotal;
                            int.TryParse(keyNeeded.text.ToString(), out hint);
                            int.TryParse(totalKeys.text.ToString(), out keyTotal);
                            keyTotal -= hint;
                            totalKeys.text = keyTotal.ToString();
                            PlayerPrefs.SetInt("Keys", keyTotal);
                            hint *= 2;
                            keyNeeded.text = hint.ToString();
                            break;
                        }
                    }
                    break;
                }
            }
        }
        else if(foundWords.Count == 0)
        {
            for (int i = 0; i < wordToFind.Length; i++)
            {
                if (!wordToFind[i].gameObject.activeSelf)
                {
                    wordToFind[i].gameObject.SetActive(true);
                    wordToFind[i].text = allWords[i];
                    hintWords.Add(allWords[i]);
                    int hint, keyTotal;
                    int.TryParse(keyNeeded.text.ToString(), out hint);
                    int.TryParse(totalKeys.text.ToString(), out keyTotal);
                    keyTotal -= hint;
                    totalKeys.text = keyTotal.ToString();
                    PlayerPrefs.SetInt("Keys", keyTotal);
                    hint *= 2;
                    keyNeeded.text = hint.ToString();
                    break;
                }
            }
        }

    }
    #endregion
    #region DestroyMatchesAndPopWord
    void DestroyMatchesAt(int row, int column)
    {
        if (allBoxes[row, column].GetComponent<BoxScript>().mathced)
        {
            for (int i = 0; i < allWords.Length; i++)
            {
                if (allWords[i] == allBoxes[row, column].GetComponent<BoxScript>().wordFound)
                {
                    for (int j = 0; j < wordToFind.Length; j++) 
                    {
                        int k;
                        if (!wordToFind[j].gameObject.activeSelf)
                        //check if the word is not active, that means its a new word and not hint word, so make it active and pop animation
                        {
                            StartCoroutine(PlaySoundThrice());
                            wordToFind[j].transform.gameObject.SetActive(true);
                            wordToFind[j].text = foundWords[foundWords.Count - 1];
                            wordToFind[j].GetComponent<Animator>().SetBool("PopUp", true);
                            break;
                        }
                        else //if active
                        {
                            //if its active then check whether there are any hint words, if not that means make another word active by continue
                            int l;
                            int count = hintWords.Count;
                            if (hintWords.Count == 0) continue; 
                            //if no hint words are there that means try next.
                            for (k = 0; k < hintWords.Count; k++) //for traversing all the hint words
                            {
                                for ( l= 0; l < foundWords.Count; l++) { //check whether any of the found word is hint word
                                    if (foundWords[l] == hintWords[k]) 
                                    {
                                        //if yes then remove it from hint word and add to old hint word
                                        oldHintWords.Add(hintWords[k]);
                                        hintWords.RemoveAt(k);
                                        for (int m = 0; m < wordToFind.Length; m++)
                                        {
                                            //this loop for finding the correct wordToFind index to enable
                                            if (wordToFind[m].gameObject.activeSelf)
                                            {
                                                // if the word is active check whether its been found
                                                if (wordToFind[m].text == foundWords[l]) 
                                                {
                                                    //if its been found enable it
                                                    StartCoroutine(PlaySoundThrice());
                                                    wordToFind[m].transform.gameObject.SetActive(true);
                                                    wordToFind[m].text = foundWords[l];
                                                    wordToFind[m].GetComponent<Animator>().SetBool("PopUp", true);
                                                    //after enabling break the loop as the correct index is found
                                                    break;
                                                }
                                            }
                                        }
                                        //after finding the correct index also break the loop of found words.
                                        break;
                                    }                                   
                                }
                                //if l is not traversed entirey that means it is breaked from between as word and index was found
                                if (l != foundWords.Count) break;
                            }
                            if (k != count) break; //if hintWords is not traversed entirely break the loop as word is found
                        }
                        
                    }
                }
            }
            int x = 0;
                try
                {
                    for (int i = 0; i < wordToFind.Length; i++) 
                    {
                        if (wordToFind[i] != null)
                        {
                            if (wordToFind[i].gameObject.activeSelf)
                            {
                            //if word is acitve then check if not hints are taken
                                if (hintWords.Count == 0)
                                {
                                    if (foundWords[x] != null)
                                    {
                                    //check if found word is equal to the active wordToFind text
                                    if (foundWords[x] == wordToFind[i].text)
                                        {
                                        //if its equal change the text as text might not be changed in case of more than two words found at a time
                                            Debug.Log(wordToFind[i].text);
                                            wordToFind[i].text = foundWords[x];
                                            x++;
                                        }
                                    //if the text is not equal 
                                        else if (foundWords[x] != wordToFind[i].text)
                                        {
                                            if (oldHintWords.Count == 0) 
                                            //if no hints are taken, if hints are taken then dont enter because it will change the text of wrong word
                                            {
                                                wordToFind[i].text = foundWords[x];
                                                x++;
                                            }
                                        }
                                    
                                }
                                }
                            }
                        }
                    }
                }
                catch(Exception e) {  }
            //x = 0;
            GameObject particle = Instantiate(destroyEffect, allBoxes[row, column].transform.position, new Quaternion(-90,Quaternion.identity.y,Quaternion.identity.z,90));
            particle.transform.GetChild(0).transform.GetComponent<ParticleSystemRenderer>().material.color = allBoxes[row, column].transform.GetComponent<MeshRenderer>().material.color;
            //destroyEffect.transform.position = allBoxes[row, column].transform.position;
            Destroy(allBoxes[row, column]);
            allBoxes[row, column] = null;
        }
    }
    #endregion
    #region Meaning
    IEnumerator ShowMeaning()
    {
        bool found = false;
        int j;
        yield return new WaitForSeconds(1f);
        int index = 0;
        //Words[] words;
        for (int i = 0; i < meaningPanel.transform.GetChild(0).transform.childCount; i++)
        {
            meaningPanel.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < allWords.Length; i++)
        {
            for (j = 0; j < foundWords.Count; j++)
            {
                if (allWords[i] == foundWords[j])
                {
                    index = i;
                    Debug.Log("INDEX:" + index);
                    if (meaningShown.Count > 0)
                    {
                        for (int k = 0; k < meaningShown.Count; k++)
                        {
                            if (index != meaningShown[k])
                            {
                                found = false;
                            }
                            else
                            {
                                found = true;
                                break;
                            }
                        }
                        Debug.Log(found);
                        if (!found) break;
                    }
                    else
                    {
                        found = true;
                        meaningShown.Add(index);
                        break;
                    }
                }
            }
            Debug.Log("J:" + j);
            if (j != foundWords.Count) { Debug.Log("BREAK"); break; }
        }
        if (!found)
            meaningShown.Add(index);
        //meaningPanel.transform.GetChild(0).GetComponent<Image>().color = allMat[index].color;
        meaningPanel.transform.GetChild(0).transform.GetComponent<Animator>().SetBool("PopUp", true);
        meaningPanel.SetActive(true);
        meaningPanel.transform.FindChild("PopUp/Title").GetComponent<TextMeshProUGUI>().text = allWords[index];
        for (int i = 1; i <= 3; i++)
        {
            meaningPanel.transform.FindChild("PopUp/Point" + i + "/p" + i).GetComponent<TextMeshProUGUI>().text = words[index].meaning[i - 1]; ;
        }
    }
    #endregion
    #region BackFromMeaning
    public void Back()
    {
        bool found = false;
        if (meaningPanel.activeSelf)
        {
            meaningPanel.SetActive(false);
            for (int i = 0; i < allWords.Length; i++)
            {
                for (int j = 0; j < foundWords.Count; j++)
                {
                    if (allWords[i] == foundWords[j])
                    {
                        for (int k = 0; k < meaningShown.Count; k++)
                        {
                            if (i != meaningShown[k])
                            {
                                found = false;
                            }
                            else
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!found)
            {
                StartCoroutine(ShowMeaning());
            }
            else
            {
                StartCoroutine(EndScreen());
            }
        }
        
    }
    #endregion
    IEnumerator PlaySoundThrice()
    {
        int times = 0;
        while (times != 3)
        {
            times++;
            yield return new WaitForSeconds(0.05f);
            UISoundController.instance.DestroySound();
        }
    }
    #region DestroyMatches
    public void DestroMatches()
    {
        for (int i = 0; i <row;i++)
        {
            for(int j=0; j<column;j++)
            {
                if(allBoxes[i,j]!=null)
                {
                   DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(IncreaseProgressBar());
        StartCoroutine(ShowMeaning());
        StartCoroutine(AfterDelayCall());
    }
    #endregion
    void Continue()
    {
        continueOrEnd.SetActive(false);
        WatchOrBuy.SetActive(true);
    }
    void EndGame()
    {
        outOfMoves.SetActive(false);
        StartCoroutine(EndScreen());
    }
    void BuyUsingGem()
    {
        if(gems<10)
        {
            allOutOfMoves.SetActive(false);
            outOfGems.SetActive(true);
        }
        else
        {
            DecreaseGems(10);
            totalSwapes += 3;
            swapesNo.text = totalSwapes.ToString();
            outOfMoves.SetActive(false);
            
        }
    }
    void BuyGems()
    {
        IncreaseGems(5);
        BackForOutOfGems();
    }
    void WatchAd()
    {
        totalSwapes += 3;
        swapesNo.text = totalSwapes.ToString();
        outOfMoves.SetActive(false);
    }
    void IncreaseGems(int count)
    {
        tempGems += count;
        gems = PlayerPrefs.GetInt("Gems", gems);
        PlayerPrefs.SetInt("Gems", gems + count);
        totalGems.text = PlayerPrefs.GetInt("Gems").ToString();
    }
    void DecreaseGems(int count)
    {
        gems = PlayerPrefs.GetInt("Gems");
        PlayerPrefs.SetInt("Gems", gems - count);
        totalGems.text = PlayerPrefs.GetInt("Gems").ToString();
    }
    void BackForOutOfGems()
    {
        if(outOfGems.activeSelf)
        {
            outOfGems.SetActive(false);
            allOutOfMoves.SetActive(true);
            WatchOrBuy.SetActive(true);
        }
        else if(WatchOrBuy.activeSelf)
        {
            WatchOrBuy.SetActive(false);
            continueOrEnd.SetActive(true);
        }
        else
        {
            outOfMoves.SetActive(false);
            StartCoroutine(EndScreen());
        }
    }
    public IEnumerator AfterDelayShow()
    {
        yield return new WaitForSeconds(0.5f);
        if (allWords.Length != foundWords.Count)
        {
            if (!showed)
            {
                yield return new WaitForSeconds(0.5f);
                outOfMoves.SetActive(true);
                showed = true;
            }
            else if (showed)
            {
                StartCoroutine(EndScreen());
            }
        }
        
    }
    public IEnumerator IncreaseProgressBar()
    {
        //yield return new WaitForSeconds(1f);
       while(gemCollected.value <= foundWords.Count*(1.0f/allWords.Length) && gemCollected.value!=1)    
        {
            gemCollected.value += 0.01f;
           yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < foundWords.Count; i++)
        {
            diamonds[i].color = Color.white;
        }
        IncreaseGems(1);

    }
    IEnumerator AfterDelayCall()
    {
        yield return new WaitForSeconds(0.001f);
        Setup();     
    }
    #region EndScreen
    IEnumerator EndScreen()
    {
        int minTaken, secTaken;
        bool win = true;
        yield return new WaitForSeconds(time);
        time = 0;
        if (foundWords.Count == allWords.Length || totalSwapes == 0)
        {
            if (!WinScrenn.activeSelf && !loseScreen.activeSelf && !meaningPanel.activeSelf)
            {
                endScreen = null;
                if (foundWords.Count == allWords.Length) {
                    win = true;
                    endScreen = WinScrenn;
                }
                else if (totalSwapes == 0) {
                    win = false;
                    endScreen = loseScreen;
                }
                if (!playOnce)
                {
                    playOnce = true;
                    UISoundController.instance.WinSound();
                }
                for (int i = 0; i < wordToFind.Length; i++)
                {
                    wordToFind[i].gameObject.SetActive(false);
                }
                background.SetActive(false);
                topPanel.SetActive(false);
                if (timer > 60)
                {
                    minTaken = (int)(timer / 60);
                    secTaken = Mathf.FloorToInt(timer - minTaken * 60);
                }
                else
                {
                    minTaken = 0;
                    secTaken = Mathf.FloorToInt(timer - minTaken * 60);
                } // for calcuate time
                if (win)
                {
                    //calcucate stars
                    if (timer > 60)
                        StartCoroutine(ShowStars(1));
                    else
                    {
                        if (secTaken < 10 && totalSwapes >= 2)
                        {
                            StartCoroutine(ShowStars(3));
                        }
                        else if (secTaken < 20)
                        {
                            StartCoroutine(ShowStars(2));
                        }
                        else
                        {
                            StartCoroutine(ShowStars(1));
                        }
                    }

                    //give rewards
                    tempCoins = foundWords.Count * 10;
                    endScreen.transform.FindChild("Gems/GemAmt").transform.GetComponent<Text>().text = "+" + tempGems;
                    endScreen.transform.FindChild("Coins/CoinsAmt").transform.GetComponent<Text>().text = "+" + tempCoins;
                    endScreen.transform.FindChild("Keys/KeyAmt").transform.GetComponent<Text>().text = "+" + tempKeys;
                    endScreen.transform.FindChild("Restart").transform.GetComponent<Button>().onClick.AddListener(() => RestartGame());
                    endScreen.transform.FindChild("Next").transform.GetComponent<Button>().onClick.AddListener(() => NextLevel());
                }
                endScreen.SetActive(true);
            }
        }
    }
    #endregion
    IEnumerator ShowStars(int count)
    {
        for(int i=0;i<count;i++)
        {
            endScreen.transform.FindChild("Star"+(i+1)).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void Home()
    {
        SceneManager.LoadScene("HomePage");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    [System.Serializable]
    public class Words
    {
        public string[] meaning = new string[3];
    }

    public Words[] words = new Words[3];
}
