using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_GoldMine : MonoBehaviour
{
    public int goldPerSec = 1;

    bool stopMining = true;

    void Start()
    {
        StartMining();
    }

    void AddGold(int amount)
    {
        GameController.INSTANCE.GainReward(amount);
    }

    IEnumerator Mine()
    {
        float waitTime = 1.0f;
        while (!stopMining) 
        {
            yield return new WaitForSeconds(waitTime);
            AddGold(goldPerSec);
        }
    }
    
    void StopMining()
    {
        stopMining = true;
    }

    void StartMining()
    {
        stopMining = false;
        StartCoroutine(Mine());
    }
}
