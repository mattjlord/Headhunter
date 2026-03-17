using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : AMovement
{
    public override void StopMovement()
    {
        dir = Vector2.zero;
    }

    protected override void Move(Vector2 value)
    {
        dir = value;
    }

    protected override void UpdateMove(float speed)
    {
        organism.Position += dir * speed * Time.deltaTime;
    }
}
