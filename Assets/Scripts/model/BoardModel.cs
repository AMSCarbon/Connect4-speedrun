using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardModel
{
    // 0: empty tile, 1: player 1, 2: player 2. 
    // 0  is the bottom of the board, goes up
    public int[,] board;

    public BoardModel() {
        initBoard();
    }

    public void initBoard() {
        //The default values of numeric array elements are set to zero, and reference elements are set to null.
        board = new int[7, 6]; // 7 columns and 6 rows. x,y; 
    }

    public bool canPlaceInColumn(int col) {
        return SpaceEmpty(col, 5);
    }

    public bool SpaceEmpty(int col, int row) {
        return board[col, row] == 0;
    }

    public int FindFirstEmptyRow(int col) {
        for (int i = 0; i < 6; i++) {
            if (SpaceEmpty(col, i)) return i;
        }
        return -1; // if you're trying to add to a full column, then fuck you lol.
    }

    public void AddPiece(int col, int player) {
        if (!canPlaceInColumn(col)) return;
        int row = FindFirstEmptyRow(col);
        board[col, row] = player;
    }

    public void PrintBoard() {
        string boardString = "";
        for (int r  = 5; r >= 0; r-- ) {
            for (int c = 0; c < 7; c++) {
                boardString += board[c, r];
                if (c < 6) boardString += ",";
            }
            boardString += "\n";
        }
        Debug.Log(boardString);
    }
    
    public bool ValidIndex(int col, int row) {
        return col >= 0 && col < 7 && row >= 0 && row < 6;
    }

    // 0: no win, 1: player 1 wins, 2: player 2 wins
    public int TestWin()
    {
        for (int c = 0; c < 7; c++) {
            for (int r = 0; r < 6; r++)
            {
                int horizontalWin = OffsetWin(c, r,1,0);
                if (horizontalWin != 0) return horizontalWin;
                int verticalWin = OffsetWin(c, r, 0, 1);
                if (verticalWin != 0) return verticalWin;
                int DiagonalRightWin = OffsetWin(c, r, 1, 1);
                if (DiagonalRightWin != 0) return DiagonalRightWin;
                int DiagonalLeftWin = OffsetWin(c, r, -1, 1);
                if (DiagonalLeftWin != 0) return DiagonalLeftWin;
            }
        }
        if (BoardFull()) return -1; 
        return 0; // no one won uwu
    }

    private bool BoardFull()
    {
        for (int r = 0; r < 6; r++) {
            for (int c = 0; c < 7; c++) {
                if (board[c, r] == 0) return false;
            }
        }
        return true;
    }

    private int OffsetWin(int col, int row, int x_dir, int y_dir) {
        if (board[col, row] == 0) return 0; //if it's empty then obvs not a win.
        for (int i = 0; i < 4; i++)
        {
            // over the edge of the board, can't be a win. 
            // if it's a different piece, it's not a win
            if (!ValidIndex(col + i*x_dir , row + i*y_dir) ||
                board[col, row] != board[col + i * x_dir, row + i * y_dir]) return 0;
        }
        return board[col, row];
    }


    public int ContiguousPieces(int col, int row, int x_dir, int y_dir, int tokenId)
    {
        if (board[col, row] == 0) return 0; 
        for (int i = 0; i < 4; i++)
        {
            // over the edge of the board, can't be a win. 
            if (!ValidIndex(col + i * x_dir, row + i * y_dir) ||
                board[col + i * x_dir, row + i * y_dir] != tokenId) return i;
        }
        return 0;
    }

    public bool isEmpty() {
        for (int r = 0; r < 6; r++)
        {
            for (int c = 0; c < 7; c++)
            {
                if (board[c, r] != 0) return false;
            }
        }
        return true;
    }
}
