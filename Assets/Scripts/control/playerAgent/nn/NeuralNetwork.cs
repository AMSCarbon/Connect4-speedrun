using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NeuralNetwork
{
    private List<Layer> layers;
    private int inputSize;
    private int outputSize;
    public float score; 

    public NeuralNetwork(int inputSize, int outputSize) {
        this.inputSize = inputSize;
        this.outputSize = outputSize;
        layers = new List<Layer>();
    }

    public void addLayer(int size) {
        //if it's the first layer, make the input the size of the actual NN input
        int inputDim = layers.Count == 0 ? this.inputSize : layers[layers.Count - 1].outputSize;
        Layer nextLayer = new Layer(inputDim, size);
        layers.Add(nextLayer);
    }

    public float[] Evaluate(float[] input) {
        foreach (Layer layer in layers) {
            input = layer.determineOutput(input);
        }
        return input;
    }

    internal void Evolve(float scale, float rate)
    {
        foreach (Layer layer in layers) {
            layer.Mutate(scale, rate);
        }
    }
    public string dump() {
        string dump = "{";
        dump += "\"score\": " + score + ",\n" +
                "\"layerCount\": " + layers.Count + ",\n" +
                "\"layers\": [";

        for (int i = 0; i < layers.Count; i++) {
            dump += layers[i].toString();
            if (i < layers.Count - 1) dump += ",";
            dump += "\n";
        }
        dump += "]}";
        return dump;
    }


    public NeuralNetwork copy() {
        NeuralNetwork nn = new NeuralNetwork(this.inputSize, this.outputSize);
        nn.score = score;
        foreach (Layer l in layers) {
            nn.layers.Add(l.copy());
        }
        return nn;
    }
}
