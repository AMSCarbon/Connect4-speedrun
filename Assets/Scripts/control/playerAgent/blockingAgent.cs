using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Tries to place pieces to minimise 
//the number of contiguous pieces the enemy has.
public class blockingAgent : PlayerAgent
{

    //wait for x seconds to make it look like it's thinking. less jaring.
    public float thinkTime = 0.1f;
    public int playerInt;
    public int enemeyInt;
    public int targetColumn;
    public bool waitingToDropToken =false;

    public int[] scores;
    public override void startTurn()
    {
        Invoke("PlaceToken", thinkTime);
        base.startTurn();
    }

    public void Update()
    {
        if (!currentTurn) return;

        if (waitingToDropToken && !token.updatingPosition) {
            waitingToDropToken = false;
            FindObjectOfType<GameManager>().AddPiece(targetColumn);
            token.ReleaseToBoard();
            gm.SwitchTurn();
        }
    }


    public void PlaceToken()
    {
        int[] rating = new int[7];

        for (int i = 0; i < 7; i++)
        {
            if (gm.CanAddAtColumn(i)) {
               int row = gm.gameModel.board.FindFirstEmptyRow(i);
                // if I put a token here next turn, would I win? 
                gm.gameModel.board.board[i, row] = playerInt;
                if (gm.gameModel.board.TestWin() == playerInt) rating[i] += 5000;
                // if we make the move, would the enemy be able to win? 
                for (int j = 0; j < 7; j++ ) {
                    if (gm.CanAddAtColumn(j)) {
                        int enemey_row = gm.gameModel.board.FindFirstEmptyRow(j);
                        gm.gameModel.board.board[j, enemey_row] = enemeyInt;
                        if (gm.gameModel.board.TestWin() == enemeyInt) rating[j] -= 10;
                        gm.gameModel.board.board[j, enemey_row] = 0;
                    }
                }
                // if the enemy put a token here next turn, would they win? 
                gm.gameModel.board.board[i, row] = enemeyInt;
                if (gm.gameModel.board.TestWin() == enemeyInt) rating[i] += 200;
                // make the position empty again >.>
                gm.gameModel.board.board[i, row] = 0;
                //is there a token next to me? (directly up not applicable here)
                //left, right, down, diagonal left down, diagonal right down, diagonal left up, diagonal right up
                rating[i] += (int)System.Math.Pow(DetermineDirectionalPoints(i, row, -1, 0, enemeyInt), 2);
                rating[i] += (int)System.Math.Pow(DetermineDirectionalPoints(i, row, 1, 0, enemeyInt), 2);
                rating[i] += (int)System.Math.Pow(DetermineDirectionalPoints(i, row, 0, -1, enemeyInt), 2);
                rating[i] += (int)System.Math.Pow(DetermineDirectionalPoints(i, row, -1, -1, enemeyInt), 2);
                rating[i] += (int)System.Math.Pow(DetermineDirectionalPoints(i, row, 1, -1, enemeyInt), 2);
                rating[i] += (int)System.Math.Pow(DetermineDirectionalPoints(i, row, -1, 1, enemeyInt), 2);
                rating[i] += (int)System.Math.Pow(DetermineDirectionalPoints(i, row, 1, 1, enemeyInt), 2);

                rating[i] += DetermineDirectionalPoints(i, row, -1, 0, playerInt);
                rating[i] += DetermineDirectionalPoints(i, row, 1, 0, playerInt);
                rating[i] += DetermineDirectionalPoints(i, row, 0, -1, playerInt);
                rating[i] += DetermineDirectionalPoints(i, row, -1, -1, playerInt);
                rating[i] += DetermineDirectionalPoints(i, row, 1, -1, playerInt);
                rating[i] += DetermineDirectionalPoints(i, row, -1, 1, playerInt);
                rating[i] += DetermineDirectionalPoints(i, row, 1, 1, playerInt);

            }
            else
            {
                //can't place it anywhere. default value is 0 so nothing will be less than that.
                rating[i] = int.MinValue;
            }
        }
        int column = 0;
        for (int i = 0; i < 7; i++) {
            if (rating[i] > rating[column]) column = i;
            //if there's a column that has the same score, there should be some chance of flipping to the next one.
            if (rating[i] == rating[column] && Random.Range(0.0f, 1.0f) < 0.5f) column = i;
        }
        GameObject columnMarker = GameObject.Find(column.ToString());
        token.UpdateTargetPosition(columnMarker.transform.position);
        targetColumn = column;
        waitingToDropToken = true;
        scores = rating;
    }

    //we'd rather be in a spot that has more contiguous pieces. 
    public int DetermineDirectionalPoints(int col, int row, int offset_x, int offset_y, int tokenID) {

        if (gm.gameModel.board.ValidIndex(col+offset_x, row + offset_y))
        {
            int contiguous_pieces = gm.gameModel.board.ContiguousPieces(col + offset_x, row + offset_y, offset_x, offset_y, tokenID);
            return contiguous_pieces;
        }
        else return 0;


    }
}
