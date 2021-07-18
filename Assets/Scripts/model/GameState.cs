using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState { 

    public bool playerOneTurn = true;
    public BoardModel board;
    public int pieces = 0; 

    public GameState() {
        board = new BoardModel();
    }

    public void AddPiece(int col) {
        int tokenNumber = playerOneTurn ? 1 : 2;
        board.AddPiece(col, tokenNumber);
        pieces++;
    }

    public void switchTurn() { playerOneTurn = !playerOneTurn; }


    public void empty() {
        board = new BoardModel();
        pieces = 0;
    }
}
