using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class WaveParser : MonoBehaviour
{
    public Transform pfBrute;
    public Transform pfBrute2;
    public Transform pfTanker;
    public Transform pfSprinter;
    public Transform pfSprinter2;
    private int hp;
    private int gold;
    private int numWaves;
    float waveCd = 5.0f;
    private List<Wave> waves;
    // Start is called before the first frame update
    void Start()
    {
        hp = 0;
        gold = 0;;
        numWaves = 0;
        waveCd = 20.0f;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Wave> GetWaves() { return waves; }
    public int GetHp() { return hp; }
    public int GetGold() { return gold; }
    public int GetNumWaves() { return numWaves; }
    public float GetWaveCd() { return waveCd; }


    ///////////////////////////////////////////////////
    //
    //         Parse functions
    //
    //////////////////////////////////////////////////
    public void ParseFileWaves(string filename) 
    {
        string text = System.IO.File.ReadAllText(filename);
        string[] words = System.Text.RegularExpressions.Regex.Split(text, @"\s+");;
        int head = 0;
        while (head < words.Length) 
        {
            if (words[head] == "init") 
            {
                ParseInitData(words, ref head);
            } 
            else if (words[head] == "waves") 
            {
                waves = ParseWaves(words, ref head);
            }
            head++;
        }
    }

    void ParseInitData(string[] words, ref int head)
    {
        head++;
        while (words[head] != "end") 
        {
            if (words[head] == "gold") 
            {
                gold = int.Parse(words[++head]);
            } 
            else if (words[head] == "numWaves") 
            {
                numWaves = int.Parse(words[++head]);
            } 
            else if (words[head] == "hp") 
            {
                hp = int.Parse(words[++head]);
            } 
            else if (words[head] == "waveCd")
            {
                waveCd = float.Parse(words[++head]);
            }
            else 
            {
                Debug.LogWarning($"Error parsing init data (head = {head}): Unidentified token: {words[head]}");
            }
            head++;
        }
    }


    /////////////////////////////////////////////////
    //              parse waves
    /////////////////////////////////////////////////

    List<Wave> ParseWaves(string[] words, ref int head) 
    {
        // int numWaves;
        // head++;
        // if (!Int32.TryParse(words[head], out numWaves)) 
        // {
        //     Debug.LogWarning($"Error parsing file (head = {head}): There must be an integer after \"waves\" specifying the number of waves");
        // }
        // List<Wave> waves = new List<Wave>(new Wave[numWaves]);
        // Debug.Log(waves);
        List<Wave> waves = new List<Wave>();
        head++;
        while (words[head] != "end")
        {
            if (words[head] == "wave") 
            {
                // NOTE: indices in txt file starts from 1
                int idx;
                head++;
                if (!Int32.TryParse(words[head], out idx)) 
                {
                    Debug.LogWarning($"Error parsing file (head = {head}): There must be an integer after \"wave\" specifying the index of the wave");
                }
                // if (idx < 1 || numWaves < idx) {
                //     Debug.LogWarning($"Error parsing file (head = {head}): the index of the wave is invalid");
                // }
                // waves[idx - 1] = ParseWave(words, ref head);
                waves.Add(ParseWave(words, ref head));
                // Debug.Log($"Got wave {idx}");
            } 
            else 
            {
                Debug.LogWarning($"Error parsing file (head = {head}): Unidentified token: {words[head]}");
            }
            head++;
        }
        Debug.Log(waves);
        return waves;
    }
    Wave ParseWave(string[] words, ref int head) 
    {
        Wave wave = new Wave();
        head++;
        while (words[head] != "end") 
        {
            SpawnAction spawnAction = ParseSpawnAction(words, ref head);
            wave.AddSpawnAction(spawnAction);
            head++;
        }
        return wave;
    }

    SpawnAction ParseSpawnAction(string[] words, ref int head) 
    {
        SpawnAction sa = new SpawnAction();
        if (words[head] == "sprinter" || words[head] == "sprinter1")
        {
            int cnt = int.Parse(words[++head]);
            sa.SetSpawnUnit(pfSprinter, cnt);
        }
        else if (words[head] == "brute" || words[head] == "brute1")
        {
            int cnt = int.Parse(words[++head]);
            sa.SetSpawnUnit(pfBrute, cnt);
        }
        else if (words[head] == "brute2")
        {
            int cnt = int.Parse(words[++head]);
            sa.SetSpawnUnit(pfBrute2, cnt);
        }
        else if (words[head] == "tanker" || words[head] == "tanker1")
        {
            int cnt = int.Parse(words[++head]);
            sa.SetSpawnUnit(pfTanker, cnt);
        }
        else 
        {
            Debug.Log($"Error parsing wave (head = {head}): Unidentified token: {words[head]}");
        }
        return sa;
    }
}
