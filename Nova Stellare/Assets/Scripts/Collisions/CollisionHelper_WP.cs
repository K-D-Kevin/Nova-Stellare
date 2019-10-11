using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHelper_WP : MonoBehaviour
{
    /// <summary>
    /// This clas is a collision helper for weapon attacks
    /// 
    /// A colision helper helps other classes activate on triggers or on collisions that are do not directly have a collider or trigger
    /// </summary>

    [SerializeField]
    private ShipAttacks CorrespondingAttack;

    // Move the collision event to be handled elsewhere
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CorrespondingAttack.AttackOnCollisionEnter(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CorrespondingAttack.AttackOnCollisionStay(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CorrespondingAttack.AttackOnCollisionExit(collision);
    }

    // Move the trigger event to be handled elsewhere
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CorrespondingAttack.AttackOnTriggerEnter(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CorrespondingAttack.AttackOnTriggerStay(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        CorrespondingAttack.AttackOnTriggerExit(collision);
    }
}
