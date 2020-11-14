using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    // Singleton
    public static GameManager Instance;

    public Pool splatPool;
    public Pool textPool;

    public bool isGameStarted;
    public bool startCounter;

    [SerializeField] Transform[] finishPoints;
    [SerializeField] Character[] players;
    [SerializeField] GameObject[] playerPrefabs;
    [SerializeField] GameObject[] winFx;
    [SerializeField] GameObject finishTable;
    [SerializeField] float gameTime;

    int coin;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (startCounter && isGameStarted)
        {
            gameTime -= Time.deltaTime;
            UIManager.Instance.UpdateGameCounter((int)gameTime);
            if (gameTime <= 0)
            {
                CalculateScores();
            }
        }
    }

    #endregion

    #region Other Methods

    void StartGame()
    {
        StartCoroutine(UIManager.Instance.GameStarted());
        Projectile3D.Instance.ActiveLine();

        foreach (Character character in players)
        {
            character.StartRunning();
        }
    }

    IEnumerator StartCointing()
    {
        isGameStarted = true;
        int counter = 3;
        for (int i = 0; i < 3; i++)
        {
            UIManager.Instance.UpdateCounting(counter.ToString());
            counter--;
            yield return new WaitForSeconds(1f);
        }
        UIManager.Instance.UpdateCounting("GO!");
        StartGame();
    }
    
    // For button event
    public void StartCounting()
    {
        UIManager.Instance.StartGame();
        CameraFollow.Instance.DefaultView();
        StartCoroutine(StartCointing());
    }

    public void EarnCoin()
    {
        coin += 50;
        UIManager.Instance.UpdateCoinText(coin);
    }

    void EndGame()
    {
        Projectile3D.Instance.PassiveLine();
        isGameStarted = false;
        foreach (Character ch in players)
            Destroy(ch.gameObject);
        CameraFollow.Instance.EndGame();
    }

    void CalculateScores()
    {
        int[] scores = new int[4];

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = (int)players[i].score;
        }
        QuickSort.Instance.Sort(scores, playerPrefabs, 0,3);
        SelectWinnners(playerPrefabs);
    }

    void SelectWinnners(GameObject[] ch)
    {
        finishTable.SetActive(true);
        for (int i = ch.Length - 1; i > 0; i--)
            Instantiate(ch[i], finishPoints[finishPoints.Length - i].position, finishPoints[finishPoints.Length - i].rotation);
        StartCoroutine(PlayFx());
        EndGame();
    }

    IEnumerator PlayFx()
    {
        yield return new WaitForSeconds(1.25f);
        for (int i = 0; i < winFx.Length; i++)
        {
            winFx[i].SetActive(true);
            yield return new WaitForSeconds(.25f);
        }
        UIManager.Instance.EndGame();
    }

    #endregion
}
