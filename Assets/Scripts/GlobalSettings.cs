using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public enum Difficulty {Easy, Normal, Hard};
    static int numGamesPlayed = 0;
    static int numGameWon = 0;
    static int numGameLost = 0;
    static int numEnemiesKilled = 0;
    static int numTowersBought = 0;
    static int numMinsPlayed = 0;
    static bool Level1Finished = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
