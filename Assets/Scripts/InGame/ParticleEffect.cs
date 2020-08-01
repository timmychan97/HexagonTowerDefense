using System.Collections;
using UnityEngine;

/// <summary>
/// Instantiates a GameObject with a Particle System.
/// 
/// A start delay can be set before instantiating the object, as the
/// object may contain multiple particle systems.
/// </summary>
public class ParticleEffect : MonoBehaviour
{
    public ParticleSystem particleSystemPf;
    public float delayStartSeconds = 0;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(delayStartSeconds);
        Instantiate(particleSystemPf.gameObject, transform);

        // Auto destroy after the main particle system module is done
        Destroy(gameObject, particleSystemPf.main.duration);
    }
}
