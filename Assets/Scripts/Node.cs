using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    // state
    // 0: untouched
    // 1: defended
    // 2: attacked
    [SerializeField]
    private int key;

    [SerializeField]
    private int state;

    private int player;

    private int layer;
    private List<int> parentNodes;

    void OnMouseDown() {
        if (player == 1)
        {
            if (state == 0)
            {

            }
            else if (state == 1)
            {
                
            }
            else if (state == 3)
            {

            }
        }
        if (player == 2)
        {
            if (state == 0)
            {

            }
            else if (state == 1)
            {
                
            }
            else if (state == 3)
            {
                
            }
        }
    }
}
