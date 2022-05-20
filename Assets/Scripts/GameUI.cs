using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject winUI, loseUI;

    private void Start ()
    {
        winUI.SetActive(false);
        loseUI.SetActive(false);

        Player.OnEndOfLevelReached += ShowWin;
        Guard.OnPlayerSpotted += ShowLose;
    }

    private void ShowWin ()
    {
        OnGameOver(winUI);
    }

    private void ShowLose ()
    {
        OnGameOver(loseUI);
    }

    private void OnGameOver (GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        Guard.OnPlayerSpotted -= ShowLose;
        Player.OnEndOfLevelReached -= ShowWin;
    }

    private void OnDestroy ()
    {
        Guard.OnPlayerSpotted -= ShowLose;
        Player.OnEndOfLevelReached -= ShowWin;
    }
}