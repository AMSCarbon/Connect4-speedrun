using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Academy : MonoBehaviour
{
    public LearningAgent redPlayer;
    public LearningAgent yellowPlayer;

    public int populationSize = 100;
    public List<NeuralNetwork> playerPopulation;

    public int currentPairingGames = 0;
    public int gamesPerPairing = 20;
    public int redPlayerIndex;
    public int yellowPlayerIndex;
    public int killNumber;
    public int totalGames = 0;
    public int generation = 0;
    public bool useAcademy = false;
    public GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        playerPopulation = new List<NeuralNetwork>();
        for (int i = 0; i < populationSize; i++) {
            NeuralNetwork nn = MakeTrainingNetwork();
            playerPopulation.Add(nn);
        }
    }

    private NeuralNetwork MakeTrainingNetwork() {
        NeuralNetwork nn = new NeuralNetwork(42, 7);
        nn.addLayer(42);
        nn.addLayer(42);
        nn.addLayer(42);
        nn.addLayer(21);
        nn.addLayer(7);
        return nn;
    }

    public void Setup() {

        if (!useAcademy) return;
        //set the first agents
        gm.academyWorking = true;
        redPlayerIndex = 0;
        yellowPlayerIndex = 1;
        setPlayers();
        gm.academyWorking = false;
    }

    private void setPlayers() {

        if (!useAcademy) return;
        redPlayer.nn = playerPopulation[redPlayerIndex];
        redPlayer.score = 0;
        yellowPlayer.nn = playerPopulation[yellowPlayerIndex];
        yellowPlayer.score = 0;
    }

    public void GameEnd() {

        if (!useAcademy) return;
        totalGames++;
        currentPairingGames++;
        if (currentPairingGames == gamesPerPairing) {
            gm.academyWorking = true;
            // update the agents scores. 
            playerPopulation[redPlayerIndex].score += redPlayer.score;
            playerPopulation[yellowPlayerIndex].score += yellowPlayer.score;   
            currentPairingGames = 0;
            yellowPlayerIndex++;
            if (yellowPlayerIndex == populationSize) {
                redPlayerIndex++;
                yellowPlayerIndex = redPlayerIndex + 1;
                if (redPlayerIndex == populationSize-1) {
                    EvolvePopulation();
                    redPlayerIndex = 0;
                    yellowPlayerIndex = 1;
                }
            }
            setPlayers();
            gm.academyWorking = false;
        }
    }

    public void EvolvePopulation() {
        generation++;
        playerPopulation.Sort((p1, p2) => p1.score.CompareTo(p2.score));
        writeBestAgent();
        //networks below a certain threshold are replaced by the top performer 
        for (int i = 1; i < killNumber; i++)
        {
            // go through and copy down the list, but don't go too far.
            int index = populationSize - 1 - i > 0 ? populationSize - 1 - i : populationSize - 1;
            playerPopulation[i] = playerPopulation[index].copy();
        }

        // the very worst is replaced by a new one. prevents the entire population
        // from totally stagnating getting used to playing just copies of itself.
        playerPopulation[0] =  MakeTrainingNetwork();
        // mutate the population 
        foreach (NeuralNetwork agent in playerPopulation) {
            float rate = ((float) gamesPerPairing - agent.score) / ((float)gamesPerPairing * 2.0f);
            float scale = rate / 10.0f;
            agent.Evolve(rate, scale);
        }
       
        foreach (NeuralNetwork agent in playerPopulation)
        {
            agent.score = 0;
        }
    }

    public void writeBestAgent() {
        string path = Application.dataPath + "/nn_" + generation + ".json";
        Debug.Log(path);
        if (!File.Exists(path)) {
            File.WriteAllText(path, JsonUtility.ToJson(playerPopulation[populationSize - 1]));
        }
    }
}
