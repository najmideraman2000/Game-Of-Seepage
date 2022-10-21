using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    [SerializeField]
    private int key;

    private int layer;
    private List<int> parentNodes;

    public Node(int key, int layer, List<int> parentNodes)
    {
        this.key = key;
        this.layer = layer;
        this.parentNodes = parentNodes;
    }

    void Start() {

    }
}
