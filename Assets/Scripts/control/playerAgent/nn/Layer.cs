using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Layer
{
    float[,] weights;
    float[,] bias;
    public int inputSize;
    public int outputSize;
    public Layer(int previous_level, int next_level) {
        inputSize = previous_level;
        outputSize = next_level;
        weights = initLayer(inputSize, outputSize);
        bias = initLayer(inputSize, outputSize);
    }

    private float[,] initLayer(int input, int output)
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        float[,] layer = new float[input, output];
        for (int i = 0; i < input; i++) {
            for ( int j = 0; j < output; j++) {
                layer[i, j] = UnityEngine.Random.Range(-1.0f, 1.0f);
            }
        }
        return layer;

    }

    public float[] determineOutput(float[] input) {
        float[] output = new float[outputSize];
        for (int i = 0; i < outputSize; i++) {
            for (int j = 0; j < input.Length; j++) {
                output[i] += input[j]* weights[j,i]+bias[j,i];
            }
            output[i] = activationFunction(output[i]);
        }
        return output;
    }

    internal void Mutate(float scale, float rate)
    {
        for (int i = 0; i < weights.GetLength(0); i++)
        {
            for (int j = 0; j < weights.GetLength(1); j++)
            {
                if (UnityEngine.Random.Range(0.0f, 1.0f) < rate)
                {
                    weights[i, j] += UnityEngine.Random.Range(-scale, scale);
                    bias[i, j] += UnityEngine.Random.Range(-scale, scale);
                }
            }
        }
    }

    //Currently ReLu
    private float activationFunction(float value) {
        return Mathf.Max(0.0f, value); 
    }

    public string toString() {
        return "{\n" + "\"inputSize\": " + this.inputSize + ",\n"
            + "\"outputSize\": " + this.outputSize + ",\n" + 
            "\"weights\": " +  printMatrix(weights) + ",\n" + 
            "\"bias\": " + printMatrix(bias) +  "\n}";
    }

    public string printMatrix(float[,] layer)
    {
        string returnString = "";

        returnString += "[";
        for (int i = 0; i < layer.GetLength(0); i++)
        {
            for (int j = 0; j < layer.GetLength(1); j++)
            {
                returnString += layer[i, j].ToString();
              
                if (!(i == layer.GetLength(0)-1 && j == layer.GetLength(1)-1)) returnString += ", ";
            }
        }
        returnString += "]";
        return returnString;
    }

    public Layer copy() {
        Layer l = new Layer(this.inputSize , this.outputSize);
        for (int i = 0; i < weights.GetLength(0); i++)
        {
            for (int j = 0; j < weights.GetLength(1); j++)
            {
                l.weights[i, j] = weights[i, j];
                l.bias[i, j] = bias[i, j];
            }
        }
        return l;
    }
}
