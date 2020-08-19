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

    // Copy contructor for deep copy
    public Effect(Effect other)
    {
        effectName = other.effectName;
        duration = other.duration;
        persistence = other.persistence;
        dmgOverTime = other.dmgOverTime;
        speedScale = other.speedScale;
        shieldAmount = other.shieldAmount;
        affected = other.affected;
        stacked = other.stacked;

        countdown = duration; // Start counting down
        Debug.Log("countdown: " + countdown.ToString());
    }

    void Start()
    {
        countdown = duration;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            StopEffect();
            Destroy(gameObject);
        }
    }

    public void SetAffected(IAffectable affectable) => affected = affectable;

    public void StopEffect()
    {
        if (affected == null) 
            Debug.LogWarning("No units are affects by " + effectName);
        affected.RemoveEffect(this);
    }

    public void ResetCountdown() => countdown = duration;
}
