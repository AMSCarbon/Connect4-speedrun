using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameState gameModel; 
    public bool gameOver = false;
    public bool restarting = false;

    public PlayerAgent redPlayer;
    public PlayerAgent yellowPlayer;

    public int redWins = 0;
    public int yellowWins = 0;
    public int ties = 0;

    public float newGameWait = 0.1f;

    public float gameOverPause = 0.1f;
    public float gameOverLeavePause = 0.1f;

    public Academy academy;
    public bool academyWorking;

    public float timeScale = 1.0f;
    void Start()
    {
        academy.Setup();
        StartCoroutine(NewGame());
    }

    void Update()
    {
        if (FindObjectOfType<PauseMenuManager>().isPaused) return;
        Time.timeScale = timeScale;

        if (Input.GetKey(KeyCode.W))
        {
            timeScale += 0.1f;
        }
        else if (Input.GetKey(KeyCode.S)) {
            timeScale -= 0.1f;
            timeScale = Mathf.Max(timeScale, 1.0f);
        }
        if (restarting) return;
        if (gameOver) {
            gameOver = false;
            StartCoroutine(GameOver());
            return;
        }
    }


    IEnumerator NewGame()
    {
        while (academyWorking)
        {
            Debug.Log("Academy working ...");
            yield return new WaitForSeconds(1.0f);
        }
        gameModel = new GameState();
        yield return new WaitForSeconds(newGameWait);
        redPlayer.startTurn();
    }

    IEnumerator GameOver()
    {
        restarting = true;
        yield return new WaitForSeconds(gameOverPause);
        FindObjectOfType<BottomColliderController>().open();
        foreach (PieceMovement piece in FindObjectsOfType<PieceMovement>())
        {
            Destroy(piece.gameObject, 2.0f);
        }
        yield return new WaitForSeconds(gameOverLeavePause);
        academy.GameEnd();
        StartCoroutine(NewGame());
        restarting = false;
    }


    public bool AddPiece(int column)
    {
        if (gameOver || restarting || !gameModel.board.canPlaceInColumn(column)) return false;
        gameModel.AddPiece(column);
        return true;
    }

    private PlayerAgent CurrentAgent() {
        return gameModel.playerOneTurn ? redPlayer : yellowPlayer;
    }

    public bool CanAddAtColumn(int col) {
        return gameModel.board.canPlaceInColumn(col);
    }

    //Editor util functions. 
    public void printBoard()
    {
        gameModel.board.PrintBoard();
    }

    public void SwitchTurn()
    {
        CurrentAgent().EndTurn();
        gameModel.switchTurn();
        if (TestWin()) return;
        if (restarting) return; //restarting can be triggered from the pause menu, so catch it here to not start a new turn.
        CurrentAgent().startTurn();
    }

    public bool TestWin() {
        int winState = gameModel.board.TestWin();
        if (gameModel.board.TestWin() != 0)
        {
            gameOver = true;
            switch (winState) {
                case 1:
                    redWins++;
                    break;
                case 2:
                    yellowWins++;
                    break;
                case 0:
                    ties++;
                    break;
            }
            NotifyPlayers(winState);
            return true;
        }
        return false;
    }

    private void NotifyPlayers(int state) {
        switch (state)
        {
            case 1:
                redPlayer.AlertEnd(true);
                yellowPlayer.AlertEnd(false);
                break;
            case 2:
                redPlayer.AlertEnd(false);
                yellowPlayer.AlertEnd(true);
                break;
            case 0:
                redPlayer.AlertEnd(false);
                yellowPlayer.AlertEnd(false);
                break;
        }
    }

    public void ClearBoard()
    {
        gameModel.board.initBoard();
    }

}
