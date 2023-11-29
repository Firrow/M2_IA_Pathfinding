using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    private float deltaDirection;

    public State(float deltaDirection)
    {
        this.deltaDirection = deltaDirection;
    }

    public Vector3 getDirection(Vector3 positionBoid, Vector3 positionTarget)
    {
        return (positionTarget - positionBoid) * deltaDirection;
    }
}
