using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningAgent : PlayerAgent
{
    public float thinkTime = 1.0f;
    public int gamesPerEpisode = 5;
    public int gamesSoFar = 0;
    public float randomRate = 0.01f;
    public int academyIndex = 0;

    public bool loadNetwork = false;
    public string nnPath;

    public override void startTurn()
    {
        Invoke("PlaceToken", thinkTime);
        base.startTurn();
    }

    public NeuralNetwork nn;


    public void Start()
    {
        if (loadNetwork && System.IO.File.Exists(nnPath)) {
            nn = JsonUtility.FromJson<NeuralNetwork>(System.IO.File.ReadAllText(nnPath));
        }
        else { 
            nn = new NeuralNetwork(42,7);
            nn.addLayer(42);
            nn.addLayer(42);
            nn.addLayer(42);
            nn.addLayer(21);
            nn.addLayer(7);
        }
        base.Start();
    }

    public void PlaceToken()
    {
        float[] board = new float[7*6];
        for (int i = 0; i < 7; i++) {
            for (int j = 0; j < 6; j++)
            {
                board[i*6+j] = (float)gm.gameModel.board.board[i, j];
            }
        }
        float[] prefs = nn.Evaluate(board);
        int column = oneHot(prefs);
        // force the first turn to be random to stop it from doing the same game every time.
        if (Random.Range(0.0f, 1.0f) < randomRate) {
            column = Random.Range(0, 6);
            while (!gm.CanAddAtColumn(column))
            {
                column = Random.Range(0, 6);
            }
        }
        GameObject columnMarker = GameObject.Find(column.ToString());
        token.transform.position = columnMarker.transform.position;
        FindObjectOfType<GameManager>().AddPiece(column);
        token.ReleaseToBoard();
        gm.SwitchTurn();
    }

    public int oneHot(float[] vec) {
        int maxIndex = 0;
        while (!gm.CanAddAtColumn(maxIndex))
        {
            maxIndex++;
        }
        for (int i = 0; i < vec.GetLength(0); i++) {
            if (vec[i] > vec[maxIndex] && gm.CanAddAtColumn(i)) maxIndex = i;
        }
        return maxIndex;
    }

    private float[,] multiplyMatrix(float[,] a, float[,] b) {
        int m = a.GetLength(0);
        int q = b.GetLength(1);
        int n = b.GetLength(0);

        float[,] c = new float[m, q];
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < q; j++)
            {
                c[i, j] = 0;
                for (int k = 0; k < n; k++)
                {
                    c[i, j] += a[i, k] * b[k, j];
                }
            }
        }
        return c;
    }


    public override void AlertEnd(bool won)
    {
        // Reward winners for winning quickly and losers for losing slowly.  
        if (won)
        {
            score += 1.0f - (float)gm.gameModel.pieces / 42.0f;
        }
        else {
            score -= 1.0f - (float)gm.gameModel.pieces / 42.0f;
        }
        score += won ? 1 : -1;
        gamesSoFar++;
        if (gamesSoFar > gamesPerEpisode) Evolve();
    }

    public void Evolve() {
        nn.Evolve(0.1f, 0.1f);
    }

    public string printLayer(float[,] layer) {
        string returnString = "";
        for (int i = 0; i < layer.GetLength(0); i++)
        {
            returnString += "{";
            for (int j = 0; j < layer.GetLength(1); j++)
            {
                returnString += layer[i, j].ToString() + ", ";
            }
            returnString += "}\n";
        }
        return returnString;
    }

    public float[,] Mutate(float[,] layer, float rate, float scale) {

        for (int i = 0; i < layer.GetLength(0); i++) {
            for (int j = 0; j < layer.GetLength(1); j++) {
                if (Random.Range(0.0f, 1.0f) < rate) {
                    layer[i, j] += Random.Range(-scale, scale);
                }
            }

        }
        return layer;
    }


    public float[,] Normalise(float[,] layer) {
        float sum = 0;
        for (int i = 0; i < layer.GetLength(0); i++)
        {
            for (int j = 0; j < layer.GetLength(1); j++)
            {
                sum += Mathf.Abs(layer[i, j]);
            }
        }
        for (int i = 0; i < layer.GetLength(0); i++)
        {
            for (int j = 0; j < layer.GetLength(1); j++)
            {
                layer[i, j] = layer[i,j]/sum;
            }
        }
        return layer;
    }
}
