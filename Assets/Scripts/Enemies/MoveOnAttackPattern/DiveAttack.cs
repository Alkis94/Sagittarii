using UnityEngine;
using System.Collections;

public class DiveAttack : MoveOnAttackPattern
{

    private Rigidbody2D rigidbody2d;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public override void MoveOnAttack(float MoveForce)
    {
        rigidbody2d.AddForce(new Vector2(0, MoveForce), ForceMode2D.Impulse);
    }

}
