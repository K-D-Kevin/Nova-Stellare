using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLaser : LasersAttacks
{
    // Characteristics
    [SerializeField]
    private float BaseSpeed = 5;
    private float CurrentSpeed = 0;
    [SerializeField]
    private float Acceleration = 0;
    private float CurrentTime = 0;
    [SerializeField]
    private float ObjectLifespan = 5;

    // Components
    private Transform MyT;

    private void Start()
    {
        Destroy(gameObject, ObjectLifespan);
        CurrentSpeed = BaseSpeed;
        MyT = transform;
    }
    private void FixedUpdate()
    {
        if (Acceleration > 0)
        {
            CurrentTime += Time.fixedDeltaTime;
            CurrentSpeed = BaseSpeed + Acceleration * CurrentTime;
        }

        MyT.position += MyT.right * CurrentSpeed;
        //MyT.Translate(MyT.right * CurrentSpeed);
    }

    // Collisions
    public override void AttackOnCollisionEnter(Collision2D Col)
    {
    }

    public override void AttackOnCollisionExit(Collision2D Col)
    {
    }

    public override void AttackOnCollisionStay(Collision2D Col)
    {
    }

    public override void AttackOnTriggerEnter(Collider2D Col)
    {
    }

    public override void AttackOnTriggerExit(Collider2D Col)
    {
    }

    public override void AttackOnTriggerStay(Collider2D Col)
    {
    }
}
