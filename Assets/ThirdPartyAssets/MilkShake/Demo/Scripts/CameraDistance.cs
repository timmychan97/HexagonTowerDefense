using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilkShake.Demo
{
    public class CameraDistance : MonoBehaviour
    {
        [SerializeField]
        private Transform startPoint;
        [SerializeField]
        private Transform endPoint;

        private void Awake()
        {
            UISetDistance(0.5f);
        }

        public void UISetDistance(float value)
        {
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, value);
        }
    }
}

