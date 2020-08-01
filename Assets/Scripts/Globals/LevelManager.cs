using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager INSTANCE;
    public List<Level> levels;

    public List<Level> GetLevels() { return levels; }

    void Start()
    {
        INSTANCE = this;
    }

}
