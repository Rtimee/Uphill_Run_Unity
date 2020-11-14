using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    #region Variables

    // Singleton
    public static UIManager Instance;

    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject gameScreen;
    [SerializeField] GameObject endScreen;
    [SerializeField] Text startCountingText;
    [SerializeField] Text gameCountingText;
    [SerializeField] Text coinText;


    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Other Methods

    public void StartGame()
    {
        startScreen.SetActive(false);
    }

    public IEnumerator GameStarted()
    {
        yield return new WaitForSeconds(1f);
        startCountingText.gameObject.SetActive(false);
        gameScreen.SetActive(true);
        GameManager.Instance.startCounter = true;
    }

    public void EndGame()
    {
        gameCountingText.enabled = false;
        endScreen.SetActive(true);
    }

    public void UpdateCounting(string counter)
    {
        startCountingText.text = counter;
    }

    public void UpdateGameCounter(int counter)
    {
        gameCountingText.text = counter.ToString();
    }

    public void UpdateCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }

    #endregion
}
