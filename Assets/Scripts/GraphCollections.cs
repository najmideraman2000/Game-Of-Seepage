using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCollections : MonoBehaviour
{
    public static List<List<List<List<int>>>> graphCollections = new List<List<List<List<int>>>>{
        // 0
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {3, 4, 5}
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {2},
                new List<int> {2}
            }
        },
        // 1
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {3, 4, 5}
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1, 2},
                new List<int> {2}
            }
        },
        // 2
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3},
                new List<int> {4, 5, 6, 7},
                new List<int> {8, 9, 10, 11, 12},
                new List<int> {13, 14, 15, 16}
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1, 2},
                new List<int> {2, 3},
                new List<int> {3},
                new List<int> {4},
                new List<int> {4, 5},
                new List<int> {7},
                new List<int> {6, 7},
                new List<int> {7},
                new List<int> {8},
                new List<int> {9, 10},
                new List<int> {10, 11},
                new List<int> {12},
            }
        },
        // 3
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3},
                new List<int> {4, 5, 6, 7, 8},
                new List<int> {9, 10, 11, 12, 13},
                new List<int> {14, 15, 16,  17, 18}
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1},
                new List<int> {2, 3},
                new List<int> {3},
                new List<int> {3},
                new List<int> {4},
                new List<int> {4, 5},
                new List<int> {5},
                new List<int> {6, 7},
                new List<int> {6, 7, 8},
                new List<int> {9, 10},
                new List<int> {9, 10, 11},
                new List<int> {9, 10, 11, 12},
                new List<int> {11, 12, 13},
                new List<int> {12, 13},
            }
        },
        // 4
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {3, 4, 5},
                new List<int> {6, 7, 8, 9},
                new List<int> {10, 11, 12, 13, 14}
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {1, 2},
                new List<int> {1, 2},
                new List<int> {3},
                new List<int> {3, 4},
                new List<int> {3, 4, 5},
                new List<int> {5},
                new List<int> {6, 7},
                new List<int> {6, 7, 8},
                new List<int> {7, 8},
                new List<int> {7, 8, 9},
                new List<int> {8, 9},
            }
        },
        // 5
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3},
                new List<int> {4, 5, 6, 7, 8},
                new List<int> {9, 10, 11, 12},
                new List<int> {13, 14, 15, 16, 17, 18},
                new List<int> {19, 20, 21, 22, 23, 24},
                new List<int> {25, 26, 27, 28, 29, 30, 31}
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1, 2},
                new List<int> {2},
                new List<int> {2, 3},
                new List<int> {3},
                new List<int> {4, 5},
                new List<int> {5, 6},
                new List<int> {6, 7},
                new List<int> {7, 8},
                new List<int> {9},
                new List<int> {9, 10},
                new List<int> {9, 10, 11},
                new List<int> {10, 11, 12},
                new List<int> {11, 12},
                new List<int> {12},
                new List<int> {13, 14, 15, 16},
                new List<int> {13, 14, 15, 17},
                new List<int> {13, 14, 15, 18},
                new List<int> {13, 16, 17, 18},
                new List<int> {14, 16, 17, 18},
                new List<int> {15, 16, 17, 18},
                new List<int> {19, 20, 21},
                new List<int> {19, 20, 21},
                new List<int> {19, 20, 21, 22, 23, 24},
                new List<int> {19, 20, 21, 22, 23, 24},
                new List<int> {19, 20, 21, 22, 23, 24},
                new List<int> {22, 23, 24},
                new List<int> {22, 23, 24},
            }
        },
        // 6
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3},
                new List<int> {4, 5, 6, 7, 8, 9},
                new List<int> {10, 11, 12, 13, 14, 15},
                new List<int> {16, 17, 18, 19, 20, 21, 22, 23, 24, 25},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1, 2},
                new List<int> {1, 2, 3},
                new List<int> {1, 2, 3},
                new List<int> {2, 3},
                new List<int> {3},
                new List<int> {4, 5},
                new List<int> {4, 5},
                new List<int> {6, 7},
                new List<int> {6, 7},
                new List<int> {8, 9},
                new List<int> {8, 9},
                new List<int> {10},
                new List<int> {11},
                new List<int> {11},
                new List<int> {12},
                new List<int> {12},
                new List<int> {13},
                new List<int> {13},
                new List<int> {14},
                new List<int> {14},
                new List<int> {15},
            }
        },
        // 7
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3, 4},
                new List<int> {5, 6, 7, 8, 9},
                new List<int> {10, 11, 12, 13, 14, 15, 16, 17},
                new List<int> {18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {1, 2},
                new List<int> {3, 4},
                new List<int> {3, 4},
                new List<int> {3, 4},
                new List<int> {5},
                new List<int> {5, 6},
                new List<int> {6},
                new List<int> {7},
                new List<int> {7, 8},
                new List<int> {7, 8, 9},
                new List<int> {8, 9},
                new List<int> {9},
                new List<int> {10},
                new List<int> {10, 11},
                new List<int> {10, 11, 12},
                new List<int> {11, 12},
                new List<int> {12},
                new List<int> {13},
                new List<int> {13, 14},
                new List<int> {14, 15},
                new List<int> {15, 16},
                new List<int> {16, 17},
                new List<int> {17},
            }
        },
        // 8
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {3, 4, 5, 6, 7},
                new List<int> {8, 9, 10, 11, 12, 13, 14},
                new List<int> {15, 16, 17, 18, 19, 20, 21},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1},
                new List<int> {2},
                new List<int> {2},
                new List<int> {2},
                new List<int> {3},
                new List<int> {3},
                new List<int> {4},
                new List<int> {4},
                new List<int> {5, 6, 7},
                new List<int> {5, 6, 7},
                new List<int> {5, 6, 7},
                new List<int> {8, 9, 10, 11},
                new List<int> {8, 9, 10, 11},
                new List<int> {8, 9, 10, 11},
                new List<int> {12},
                new List<int> {12, 13},
                new List<int> {13, 14},
                new List<int> {14},
            }
        },
        // 9
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3},
                new List<int> {4, 5, 6, 7, 8},
                new List<int> {9, 10, 11, 12, 13, 14, 15},
                new List<int> {16, 17, 18, 19, 20, 21, 22, 23, 24},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1, 2},
                new List<int> {1, 2, 3},
                new List<int> {2, 3},
                new List<int> {3},
                new List<int> {4},
                new List<int> {4, 5},
                new List<int> {5, 6},
                new List<int> {5, 6, 7},
                new List<int> {6, 7},
                new List<int> {7, 8},
                new List<int> {8},
                new List<int> {9},
                new List<int> {9, 10},
                new List<int> {10, 11},
                new List<int> {11, 12},
                new List<int> {11, 12, 13},
                new List<int> {12, 13},
                new List<int> {13, 14},
                new List<int> {14, 15},
                new List<int> {15},
            }
        },
        // 10
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {3, 4, 5},
                new List<int> {6, 7, 8, 9},
                new List<int> {10, 11, 12, 13, 14, 15, 16, 17},
                new List<int> {18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {1, 2},
                new List<int> {1, 2},
                new List<int> {3, 5},
                new List<int> {4},
                new List<int> {4},
                new List<int> {3, 5},
                new List<int> {6},
                new List<int> {6},
                new List<int> {7},
                new List<int> {7},
                new List<int> {8},
                new List<int> {8},
                new List<int> {9},
                new List<int> {9},
                new List<int> {10, 11},
                new List<int> {10, 11},
                new List<int> {10, 11},
                new List<int> {12, 13},
                new List<int> {12, 13},
                new List<int> {12, 13},
                new List<int> {14, 15},
                new List<int> {14, 15},
                new List<int> {14, 15},
                new List<int> {16, 17},
                new List<int> {16, 17},
                new List<int> {16, 17},
            }
        },
        // 11
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3},
                new List<int> {4, 5, 6, 7, 8, 9, 10},
                new List<int> {11, 12, 13, 14, 15, 16, 17, 18, 19},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1},
                new List<int> {2},
                new List<int> {2},
                new List<int> {3},
                new List<int> {3},
                new List<int> {3},
                new List<int> {4},
                new List<int> {4},
                new List<int> {5},
                new List<int> {5, 6},
                new List<int> {6},
                new List<int> {7},
                new List<int> {7},
                new List<int> {8, 9, 10},
                new List<int> {8, 9, 10},
            }
        },
        // 12
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {3, 4, 5, 6, 7, 8},
                new List<int> {9, 10, 11, 12},
                new List<int> {13, 14, 15, 16, 17},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {2},
                new List<int> {2},
                new List<int> {2},
                new List<int> {1},
                new List<int> {1},
                new List<int> {1},
                new List<int> {3, 5, 6, 7, 8},
                new List<int> {4, 6, 7, 8},
                new List<int> {3, 5, 6, 7, 8},
                new List<int> {6, 7, 8},
                new List<int> {9, 10, 11, 12},
                new List<int> {9, 10, 11, 12},
                new List<int> {9, 10, 11, 12},
                new List<int> {9, 10, 11, 12},
                new List<int> {9, 10, 11, 12},
            }
        },
        // 13
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {3, 4, 5},
                new List<int> {6, 7, 8, 9},
                new List<int> {10, 11, 12, 13, 14},
                new List<int> {15, 16, 17, 18, 19, 20},
                new List<int> {21, 22, 23, 24, 25, 26, 27},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {1, 2},
                new List<int> {1, 2},
                new List<int> {3, 4, 5},
                new List<int> {3, 4, 5},
                new List<int> {3, 4, 5},
                new List<int> {3, 4, 5},
                new List<int> {6, 7, 8, 9},
                new List<int> {6, 7, 8, 9},
                new List<int> {6, 7, 8, 9},
                new List<int> {6, 7, 8, 9},
                new List<int> {6, 7, 8, 9},
                new List<int> {10, 11, 12, 13, 14},
                new List<int> {10, 11, 12, 13, 14},
                new List<int> {10, 11, 12, 13, 14},
                new List<int> {10, 11, 12, 13, 14},
                new List<int> {10, 11, 12, 13, 14},
                new List<int> {10, 11, 12, 13, 14},
                new List<int> {15, 20},
                new List<int> {15, 16},
                new List<int> {15, 16, 17},
                new List<int> {16, 17, 18},
                new List<int> {17, 18, 19},
                new List<int> {18, 19, 20},
                new List<int> {19, 20},
            }
        },
        // 14
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3, 4, 5},
                new List<int> {6, 7, 8, 9, 10, 11, 12, 13, 14, 15},
                new List<int> {16, 17, 18, 19, 20, 21},
                new List<int> {22, 23, 24, 25, 26},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1},
                new List<int> {2},
                new List<int> {2},
                new List<int> {3},
                new List<int> {3},
                new List<int> {4},
                new List<int> {4},
                new List<int> {5},
                new List<int> {5},
                new List<int> {6, 7, 8, 9},
                new List<int> {6, 7, 8, 9},
                new List<int> {10, 11, 12, 13, 14, 15},
                new List<int> {10, 11, 12, 13, 14, 15},
                new List<int> {10, 11, 12, 13, 14, 15},
                new List<int> {10, 11, 12, 13, 14, 15},
                new List<int> {16, 17, 18, 21},
                new List<int> {16, 17, 18, 19},
                new List<int> {16, 17, 18, 19, 20},
                new List<int> {16, 17, 19, 20, 21},
                new List<int> {16, 17, 20, 21},
            }
        },
        // 15
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3},
                new List<int> {4, 5, 6, 7, 8},
                new List<int> {9, 10, 11, 12, 13},
                new List<int> {14, 15, 16, 17, 18},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1, 2},
                new List<int> {1, 2, 3},
                new List<int> {2, 3},
                new List<int> {3},
                new List<int> {4, 6, 7},
                new List<int> {4, 5, 8},
                new List<int> {5, 6, 7},
                new List<int> {4, 7, 8},
                new List<int> {5, 6, 8},
                new List<int> {9, 10, 12},
                new List<int> {9, 10, 11},
                new List<int> {9, 11, 13},
                new List<int> {11, 12, 13},
                new List<int> {10, 12, 13},
            }
        },
        // 16
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3},
                new List<int> {4, 5, 6},
                new List<int> {7, 8, 9, 10},
                new List<int> {11, 12, 13, 14, 15},
                new List<int> {16, 17, 18, 19, 20, 21, 22},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1, 2, 3},
                new List<int> {1, 2, 3},
                new List<int> {1, 2, 3},
                new List<int> {4, 5},
                new List<int> {4, 5, 6},
                new List<int> {4, 5, 6},
                new List<int> {6},
                new List<int> {7},
                new List<int> {7, 8},
                new List<int> {7, 8, 9, 10},
                new List<int> {8, 9, 10},
                new List<int> {9, 10},
                new List<int> {11},
                new List<int> {11, 12},
                new List<int> {11, 12, 13},
                new List<int> {12, 13, 14},
                new List<int> {13, 14, 15},
                new List<int> {14, 15},
                new List<int> {15},
            }
        },
        // 17
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2},
                new List<int> {3, 4, 5, 6, 7},
                new List<int> {8, 9, 10, 11, 12, 13, 14, 15},
                new List<int> {16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1},
                new List<int> {2},
                new List<int> {2},
                new List<int> {2},
                new List<int> {3, 4},
                new List<int> {3, 4},
                new List<int> {3, 4},
                new List<int> {5},
                new List<int> {5, 6},
                new List<int> {6},
                new List<int> {6, 7},
                new List<int> {7},
                new List<int> {8},
                new List<int> {8, 9, 10},
                new List<int> {8, 9, 10},
                new List<int> {8, 9, 10},
                new List<int> {8, 11},
                new List<int> {8, 11, 12},
                new List<int> {10, 12, 13},
                new List<int> {13},
                new List<int> {13, 14},
                new List<int> {14, 15},
                new List<int> {15},
            }
        },
        // 18
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3},
                new List<int> {4, 5, 6, 7, 8},
                new List<int> {9, 10, 11, 12, 13, 14, 15},
                new List<int> {16, 17, 18, 19, 20, 21, 22},
                new List<int> {23, 24, 25, 26, 27, 28, 29},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1, 2},
                new List<int> {1, 2, 3},
                new List<int> {2, 3},
                new List<int> {3},
                new List<int> {4},
                new List<int> {4, 5},
                new List<int> {5, 6},
                new List<int> {5, 6, 7},
                new List<int> {6, 7},
                new List<int> {7, 8},
                new List<int> {8},
                new List<int> {9, 10},
                new List<int> {10, 11},
                new List<int> {11, 12},
                new List<int> {11, 12, 13},
                new List<int> {12, 13},
                new List<int> {13, 14},
                new List<int> {14, 15},
                new List<int> {16, 21, 22},
                new List<int> {16, 17, 22},
                new List<int> {16, 17, 18},
                new List<int> {17, 18, 19},
                new List<int> {18, 19, 20},
                new List<int> {19, 20, 21},
                new List<int> {20, 21, 22},
            }
        },
        // 19
        new List<List<List<int>>>{
            new List<List<int>>{
                new List<int> {0},
                new List<int> {1, 2, 3, 4},
                new List<int> {5, 6, 7, 8, 9, 10, 11, 12},
                new List<int> {13, 14, 15, 16, 17, 18, 19, 20},
                new List<int> {21, 22, 23, 24, 25, 26, 27, 28},
            },
            new List<List<int>>{
                new List<int> {},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {0},
                new List<int> {1},
                new List<int> {1, 2},
                new List<int> {1, 2},
                new List<int> {1, 2, 3},
                new List<int> {2, 3, 4},
                new List<int> {3, 4},
                new List<int> {3, 4},
                new List<int> {4},
                new List<int> {5},
                new List<int> {5, 6},
                new List<int> {5, 6, 7, 8, 9, 10},
                new List<int> {5, 6, 7, 8, 9, 10, 11},
                new List<int> {6, 7, 8, 9, 10, 11, 12},
                new List<int> {7, 8, 9, 10, 11, 12},
                new List<int> {11, 12},
                new List<int> {12},
                new List<int> {13},
                new List<int> {13, 14},
                new List<int> {13, 14, 15, 16, 17, 18},
                new List<int> {13, 14, 15, 16, 17, 18, 19},
                new List<int> {14, 15, 16, 17, 18, 19, 20},
                new List<int> {15, 16, 17, 18, 19, 20},
                new List<int> {19, 20},
                new List<int> {20},
            }
        },
    };
}
