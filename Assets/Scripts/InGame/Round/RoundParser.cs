using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class RoundParser : MonoBehaviour
{
    public Transform pfBrute;
    public Transform pfSprinter;
    private int hp;
    private int gold;
    private int numRounds;
    float waveCd = 5.0f;
    private List<Round> rounds;
    // Start is called before the first frame update
    void Start()
    {
        hp = 0;
        gold = 0;;
        numRounds = 0;
        waveCd = 20.0f;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Round> GetRounds() { return rounds; }
    public int GetHp() { return hp; }
    public int GetGold() { return gold; }
    public int GetNumRounds() { return numRounds; }
    public float GetWaveCd() { return waveCd; }


    ///////////////////////////////////////////////////
    //
    //         Parse functions
    //
    //////////////////////////////////////////////////
    public void ParseFileRounds(string filename) 
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
            else if (words[head] == "rounds") 
            {
                rounds = ParseRounds(words, ref head);
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
            else if (words[head] == "numRounds") 
            {
                numRounds = int.Parse(words[++head]);
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
    //              parse rounds
    /////////////////////////////////////////////////

    List<Round> ParseRounds(string[] words, ref int head) 
    {
        int numRounds;
        head++;
        if (!Int32.TryParse(words[head], out numRounds)) 
        {
            Debug.LogWarning($"Error parsing file (head = {head}): There must be an integer after \"rounds\" specifying the number of rounds");
        }
        List<Round> rounds = new List<Round>(new Round[numRounds]);
        head++;
        while (words[head] != "end")
        {
            if (words[head] == "round") 
            {
                // NOTE: indices in txt file starts from 1
                int idx;
                head++;
                if (!Int32.TryParse(words[head], out idx)) 
                {
                    Debug.LogWarning($"Error parsing file (head = {head}): There must be an integer after \"round\" specifying the index of the round");
                }
                if (idx < 1 || numRounds < idx) {
                    Debug.LogWarning($"Error parsing file (head = {head}): the index of the round is invalid");
                }
                rounds[idx - 1] = ParseRound(words, ref head);
                // Debug.Log($"Got round {idx}");
            } 
            else 
            {
                Debug.LogWarning($"Error parsing file (head = {head}): Unidentified token: {words[head]}");
            }
            head++;
        }
        return rounds;
    }
    Round ParseRound(string[] words, ref int head) 
    {
        Round round = new Round();
        head++;
        while (words[head] != "end") 
        {
            if (words[head] == "sprinter")
            {
                int cnt = int.Parse(words[++head]);
                round.AddUnit(pfSprinter, cnt);
            }
            else if (words[head] == "brute")
            {
                int cnt = int.Parse(words[++head]);
                round.AddUnit(pfBrute, cnt);
            }
            else 
            {
                Debug.Log($"Error parsing round (head = {head}): Unidentified token: {words[head]}");
            }
            head++;
        }
        return round;
    }
}
