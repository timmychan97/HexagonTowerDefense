using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public int id;
    public GameObject src { get; set; }
    public string effectName;
    public float duration;
    public int persistence; // for effects that disappear after receiving certain number of attacks
    public float dmgOverTime;
    public float speedScale;
    public float shieldAmount;
    private IAffectable affected;
    public float countdown;
    public bool stacked;

    // copy contructor for deep copy
    public Effect(Effect other)
    {
        this.effectName = other.effectName;
        this.duration = other.duration;
        this.persistence = other.persistence;
        this.dmgOverTime = other.dmgOverTime;
        this.speedScale = other.speedScale;
        this.shieldAmount = other.shieldAmount;
        this.affected = other.affected;
        this.stacked = other.stacked;

        this.countdown = duration; // start counting down
        Debug.Log("countdown: " + countdown.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        countdown = duration;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            StopEffect();
            Destroy(gameObject);
        }
    }

    public void SetAffected(IAffectable affectable)
    {
        affected = affectable;
    }

    public void StopEffect()
    {
        if (affected == null) 
        {
            Debug.LogWarning("No units are affects by " + effectName);
        }
        affected.RemoveEffect(this);
    }

    public void ResetCountdown()
    {
        countdown = duration;
    }
}
