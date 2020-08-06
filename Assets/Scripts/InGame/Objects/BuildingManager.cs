using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Many buildings have global effects, and to avoid all
buildings having their own calls for the same effect,
this script will implement them in one go. 
*/

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager INSTANCE;
    public int initialGoldPerSec = 1;
    int goldPerSec;

    HashSet<Building_GoldMine> goldMines = new HashSet<Building_GoldMine>();
    HashSet<Building> buildings = new HashSet<Building>();
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        goldPerSec = initialGoldPerSec;
        StartProduction();
    }

    void AddGold(int amount)
    {
        GameController.INSTANCE.GainGold(amount);
    }

    public void OnBuyGoldMine(Building_GoldMine mine)
    {
        goldPerSec += mine.goldPerSec;
        goldMines.Add(mine);
    }

    public void StartProduction()
    {
        StopAllCoroutines();
        StartCoroutine(StartGoldProduction());
    }

    public void StopProduction()
    {
        StopAllCoroutines();
    }

    IEnumerator StartGoldProduction()
    {
        float duration = 1.0f;
        while (true) 
        {
            yield return new WaitForSeconds(duration);
            AddGold(goldPerSec);
        }
    }

    public void ClearAllBuildings()
    {
        foreach (Building b in buildings) {
            if (b != null) 
            {
                Destroy(b.gameObject);
            }
        }
        buildings.Clear();
    }
}
