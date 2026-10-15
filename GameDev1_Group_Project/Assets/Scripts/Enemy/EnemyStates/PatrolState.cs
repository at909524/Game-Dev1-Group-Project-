using UnityEngine;

public class PatrolState : State
{

    protected override string AnimBoolName => "isWalking";

    public PatrolState(Enemy enemy) : base(enemy) { }


    public override void FixedUpdate()
    {
        if(senses.GetChaseTarget())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if(senses.IsHittingWall() || senses.IsAtCliff())
        {
            enemy.Flip();
            return;
        }
        rb.linearVelocity = new Vector2(config.patrolSpeed * enemy.FacingDirection, rb.linearVelocity.y);
    }
}
