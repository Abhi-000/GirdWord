using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    public string wordFound;
    public bool added = false,found = false;
    public bool lerpToCenter = false,lerpToCenterRow =false;
    public static BoxScript instance;
    public int row;
    public int column;
    public int previousRow;
    public int previousColumn;
    public float targetX;
    public float targetZ;
    public bool mathced = false;
    private GameObject leftBox, rightBox, upBox, downBox;    
    private bool startLerping;
    private GameObject otherBox;
    private float speed = 0.3f;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        previousRow = row;
        previousColumn = column;
        if (this.gameObject.name == "N (2)")
        {
            previousRow = 2;
            previousColumn = 1;
        }
    }
    void Update()
    {
        if(lerpToCenter)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(GameManager.instance.centerColumn.transform.localPosition.x, transform.localPosition.y, transform.localPosition.z), speed);
        }
        else if(lerpToCenterRow)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, GameManager.instance.centerRow.transform.localPosition.z), speed);
        }
        FindMatches();
        targetX = GameManager.instance.allBoxLoc[row, column].x;
        targetZ = GameManager.instance.allBoxLoc[row, column].z;
            if (Mathf.Abs(targetX - transform.localPosition.x) > .01f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(targetX, transform.localPosition.y, transform.localPosition.z), speed);
                if (GameManager.instance.allBoxes[row, column] != this.gameObject)
                {
                    GameManager.instance.allBoxes[row, column] = this.gameObject;
                }
            }
            else
            {
                transform.localPosition = new Vector3(targetX, transform.localPosition.y, transform.localPosition.z);
            }
            if (Mathf.Abs(targetZ - transform.localPosition.z) > .01f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, targetZ), speed);
                if (GameManager.instance.allBoxes[row, column] != this.gameObject)
                {
                    GameManager.instance.allBoxes[row, column] = this.gameObject;
                }
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, targetZ);
            }
        if (gameObject.name == GameInput.input.name)
        {
            if (GameManager.instance.totalSwapes > 0 && !GameManager.instance.meaningPanel.activeSelf)
            {
                if ((GameInput.input.Su && row != 0) || (GameInput.input.Sd && row != 2) || (GameInput.input.Sr && column != 2) || (GameInput.input.Sl && column != 0))
                {
                    UISoundController.instance.SwipeSound();
                    GameManager.instance.swipes++;
                    GameManager.instance.totalSwapes--;
                    GameManager.instance.swapesNo.text = GameManager.instance.totalSwapes.ToString();
                    if (GameManager.instance.totalSwapes == 0)
                    {
                        StartCoroutine(GameManager.instance.AfterDelayShow());
                    }
                }
                if (GameInput.input.Su && row != 0)
                {
                    //swipe up
                    otherBox = GameManager.instance.allBoxes[row - 1, column];
                    otherBox.transform.GetComponent<BoxScript>().row++;
                    otherBox.transform.GetComponent<BoxScript>().found = false;
                    row--;
                }
                else if (GameInput.input.Sd && row != 2)
                {
                    // swipe down
                    otherBox = GameManager.instance.allBoxes[row + 1, column];
                    otherBox.transform.GetComponent<BoxScript>().row--;
                    otherBox.transform.GetComponent<BoxScript>().found = false;
                    row++;
                }
                else if (GameInput.input.Sr && column != 2)
                {
                    //swipe right
                    otherBox = GameManager.instance.allBoxes[row, column + 1];
                    otherBox.transform.GetComponent<BoxScript>().column--;
                    otherBox.transform.GetComponent<BoxScript>().found = false;
                    column++;
                }
                else if (GameInput.input.Sl && column != 0)
                {
                    //swipe left
                    otherBox = GameManager.instance.allBoxes[row, column - 1];
                    otherBox.transform.GetComponent<BoxScript>().column++;
                    otherBox.transform.GetComponent<BoxScript>().found = false;
                    column--;
                }
            }            

            //StartCoroutine(CheckMove()); //if swap positions again
        }
        

    }
    public IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(.5f);
        GameManager.instance.DestroMatches();
        StopCoroutine(CheckMove());
        /*if (otherBox != null)
        {
            *//*if (!mathced && !otherBox.GetComponent<BoxScript>().mathced)
            {
                otherBox.GetComponent<BoxScript>().row = row;
                otherBox.GetComponent<BoxScript>().column = column;
                row = previousRow;
                column = previousColumn;
            }*/
        /*GameObject upBox = GameManager.instance.allBoxes[row - 1, column];
        GameObject downBox = GameManager.instance.allBoxes[row + 1, column];
        GameObject leftBox = GameManager.instance.allBoxes[row, column - 1];
        GameObject rightBox = GameManager.instance.allBoxes[row, column + 1];*//*
        if (upBox != null && downBox != null && rightBox != null && leftBox != null)
        {
            if (mathced && upBox.GetComponent<BoxScript>().mathced && downBox.GetComponent<BoxScript>().mathced)
            {
                Debug.Log("match");
                GameManager.instance.DestroMatches();
            }
            else if (mathced && leftBox.GetComponent<BoxScript>().mathced && rightBox.GetComponent<BoxScript>().mathced)
            {
                Debug.Log("match");
                GameManager.instance.DestroMatches();
            }
        }
        otherBox = null;
    }*/

    }

    void FindMatches()
    {
        if(column>0 && column<GameManager.instance.column-1)
        {
            leftBox = GameManager.instance.allBoxes[row, column - 1];
            rightBox = GameManager.instance.allBoxes[row, column + 1];
            if (leftBox != null && rightBox != null)
            {
                string leftLetter = leftBox.tag;
                string rightLetter = rightBox.tag;
                string word = leftLetter + this.gameObject.tag + rightLetter;
                for (int i = 0; i < GameManager.instance.allWords.Length; i++)
                {
                    if (word == GameManager.instance.allWords[i])
                    {
                        if (GameManager.instance.foundWords.Count != 0)
                        {
                            for (int j = 0; j < GameManager.instance.foundWords.Count; j++)
                            {
                                if (word == GameManager.instance.foundWords[j])
                                {
                                    found = true;
                                }
                            }
                                    if (!added && !found)
                                    {
                                        wordFound = word;
                                        CheckRow();
                                        GameManager.instance.decreaseRow = true;
                                        found = false;
                                        added = true;
                                        StartCoroutine(CheckMove());
                                        GameManager.instance.foundWords.Add(GameManager.instance.allWords[i]);
                                        rightBox.GetComponent<BoxScript>().mathced = true;
                                        leftBox.GetComponent<BoxScript>().mathced = true;
                                        mathced = true;
                                    }
                        }
                        else
                        {
                            if (!added)
                            {
                                wordFound = word;
                                CheckRow();
                                GameManager.instance.decreaseRow = true;
                                added = true;
                                StartCoroutine(CheckMove());
                                GameManager.instance.foundWords.Add(GameManager.instance.allWords[i]);
                                rightBox.GetComponent<BoxScript>().mathced = true;
                                leftBox.GetComponent<BoxScript>().mathced = true;
                                mathced = true;
                            }
                        }
                    }
                }
            }

        }
        if (row > 0 && row < GameManager.instance.row - 1)
        {
            upBox = GameManager.instance.allBoxes[row-1, column];
            downBox = GameManager.instance.allBoxes[row+1, column];
            if (upBox != null && downBox != null)
            {
                string upLetter = upBox.tag;
                string downLetter = downBox.tag;
                string word = upLetter + this.gameObject.tag + downLetter;
                for (int i = 0; i < GameManager.instance.allWords.Length; i++)
                {
                    if (word == GameManager.instance.allWords[i])
                    {
                        if (GameManager.instance.foundWords.Count != 0)
                        {
                            for (int j = 0; j < GameManager.instance.foundWords.Count; j++)
                            {
                                if (word == GameManager.instance.foundWords[j])
                                {
                                    found = true;
                                }
                            }
                                    if (!added && !found) {
                                        wordFound = word;
                                         CheckColumn();
                                        GameManager.instance.decreaseCol = true;
                                         found = false;
                                        added = true;
                                        StartCoroutine(CheckMove());
                                        GameManager.instance.foundWords.Add(GameManager.instance.allWords[i]);
                                        downBox.GetComponent<BoxScript>().mathced = true;
                                        upBox.GetComponent<BoxScript>().mathced = true;
                                        mathced = true;
                                    }
                         }
                        else
                        {
                            if (!added)
                            {
                                wordFound = word;
                                CheckColumn();
                                GameManager.instance.decreaseCol = true;
                                added = true;
                                StartCoroutine(CheckMove());
                                GameManager.instance.foundWords.Add(GameManager.instance.allWords[i]);
                                downBox.GetComponent<BoxScript>().mathced = true;
                                upBox.GetComponent<BoxScript>().mathced = true;
                                mathced = true;
                            }
                        }
                    }
                }
            }
        }
    }
    public void CheckColumn()
    {
        upBox = GameManager.instance.allBoxes[row - 1, column];
        downBox = GameManager.instance.allBoxes[row + 1, column];
        if (upBox != null && downBox != null)
        {
            if (upBox.transform.GetComponent<BoxScript>().column == 1 && downBox.transform.GetComponent<BoxScript>().column == 1 && column == 1)
            {
                StartCoroutine(AfterDelay());
            }
            else if (upBox.transform.GetComponent<BoxScript>().column == 0 && downBox.transform.GetComponent<BoxScript>().column == 0 && column == 0)
            {
                if (GameManager.instance.column == 2)
                {
                    StartCoroutine(AfterDelay());
                }
            }
        }
    }
    public void CheckRow()
    {
        leftBox = GameManager.instance.allBoxes[row, column - 1];
        rightBox = GameManager.instance.allBoxes[row, column + 1];
        if(leftBox!=null && rightBox!=null)
        {
            if (leftBox.transform.GetComponent<BoxScript>().row == 1 && rightBox.transform.GetComponent<BoxScript>().row == 1 && row == 1)
            {
                Debug.Log("ROW SAME 1");
                StartCoroutine(AfterDelayForRow());
            }
            else if(leftBox.transform.GetComponent<BoxScript>().row == 0 && rightBox.transform.GetComponent<BoxScript>().row == 0 && row == 0)
            {
                if (GameManager.instance.row == 2)
                {
                    Debug.Log("ROW SAME 0");
                    StartCoroutine(AfterDelayForRow());
                }
                
            }
        }
    }
    IEnumerator AfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < GameManager.instance.row; i++)
        {
            if (GameManager.instance.allBoxes[i, 0] != null)
            {
                if (GameManager.instance.allBoxes[i, 2] != null)
                {
                    GameManager.instance.decreaseCol = false;
                    GameManager.instance.colDestroyed = true;
                    GameManager.instance.allBoxes[i, 0].GetComponent<BoxScript>().lerpToCenter = true;
                    GameManager.instance.allBoxes[i, 2].GetComponent<BoxScript>().lerpToCenter = true;
                }
                else if (GameManager.instance.allBoxes[i, 1] != null)
                {
                    GameManager.instance.allBoxes[i,1].GetComponent<BoxScript>().lerpToCenter = true;
                }
            }
        }
        if (upBox.transform.GetComponent<BoxScript>().column == 0 && downBox.transform.GetComponent<BoxScript>().column == 0 && column == 0)
        {
            GameManager.instance.centerColumn.transform.localPosition = new Vector3(GameManager.instance.centerColumn.transform.localPosition.x-1f, GameManager.instance.centerColumn.transform.localPosition.y, GameManager.instance.centerColumn.transform.localPosition.z);
        }
        else if (GameManager.instance.column == 2)
        {
            GameManager.instance.centerColumn.transform.localPosition = new Vector3(GameManager.instance.centerColumn.transform.localPosition.x+1f, GameManager.instance.centerColumn.transform.localPosition.y, GameManager.instance.centerColumn.transform.localPosition.z );
        }
    }
    IEnumerator AfterDelayForRow()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < GameManager.instance.column; i++)
        {
            if (GameManager.instance.allBoxes[0, i] != null)
            {
                if (GameManager.instance.allBoxes[2, i] != null)
                {
                    GameManager.instance.decreaseRow = false;
                    GameManager.instance.rowDestroyed = true;
                    GameManager.instance.allBoxes[0, i].GetComponent<BoxScript>().lerpToCenterRow = true;
                    GameManager.instance.allBoxes[2, i].GetComponent<BoxScript>().lerpToCenterRow = true;
                }
                else if(GameManager.instance.allBoxes[1, i] != null)
                {
                    GameManager.instance.allBoxes[1, i].GetComponent<BoxScript>().lerpToCenterRow = true;
                }
            }
        }
        if (leftBox.transform.GetComponent<BoxScript>().row == 0 && rightBox.transform.GetComponent<BoxScript>().row == 0 && row == 0)
        {
            GameManager.instance.centerRow.transform.localPosition = new Vector3(GameManager.instance.centerRow.transform.localPosition.x, GameManager.instance.centerRow.transform.localPosition.y, GameManager.instance.centerRow.transform.localPosition.z + 1.5f);
        }
        else if (GameManager.instance.row == 2)
        {
            GameManager.instance.centerRow.transform.localPosition = new Vector3(GameManager.instance.centerRow.transform.localPosition.x, GameManager.instance.centerRow.transform.localPosition.y, GameManager.instance.centerRow.transform.localPosition.z - 1.5f);
        }
    }
 }
