using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MilkShake.Shaker))]
public class CameraShaker : MonoBehaviour
{
    public MilkShake.ShakePreset explosionPreset;
    public static CameraShaker INSTANCE;

    private void Awake() => INSTANCE = this;

    /// <summary>
    /// Shake the camera from a specific position
    /// </summary>
    /// <param name="position"></param>
    public static void ExplosionShake(Vector3 position, float maxDistance = 30f)
    {
        MilkShake.Shaker.ShakeAllFromPoint(position, maxDistance, INSTANCE.explosionPreset);
    }




}
