using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCollections : MonoBehaviour
{
    public static List<List<List<List<int>>>> graphCollections = new List<List<List<List<int>>>>{
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
            }
        },
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {3, 4,},
                new List<int> {5, 6, 7, 8, },
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {2},
                new List<int> {3},
                new List<int> {3},
                new List<int> {4},
                new List<int> {4},
            }
        },
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1},
                new List<int> {2},
                new List<int> {3},
                new List<int> {4},
                new List<int> {5},
                new List<int> {6},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {1},
                new List<int> {2},
                new List<int> {3},
                new List<int> {4},
                new List<int> {5},
            }
        }
    };
}
