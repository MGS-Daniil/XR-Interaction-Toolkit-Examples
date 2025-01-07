using System;
using UnityEngine;


[CreateAssetMenu(fileName = "config", menuName = "KartConfig", order = 0)]
public class KartConfig : ScriptableObject
{
    public int mass;
    public float wheelDampingRate;
    public float suspensionDistance;
    public Spring suspensionSpring;
    public Friction forwardFriction;
    public Friction sidewaysFriction;

    [Serializable]
    public class Friction
    {
        public float extremumSlip;
        public float extremumValue;
        public float asymptoteSlip;
        public float asymptoteValue;
        public float stiffness;
    }

    [Serializable]
    public class Spring
    {
        public int spring;
        public int damper;
        public float targetPosition;
    }
}