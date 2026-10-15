using UnityEngine;

public class IdleState : State
{
    private Transform target;
    protected override string AnimBoolName => "isIdling";

    public IdleState(Enemy enemy) : base(enemy) { }


    public override void Enter()
        {
        base.Enter();
            rb.linearVelocity = Vector2.zero;

        }



    public override void FixedUpdate()
    {
        //1. Check for Target
        target = senses.GetChaseTarget();

        if (!target)
        {
            enemy.StateMachine.ChangeState(new PatrolState(enemy));
            return;
        }

        enemy.FaceTarget(target);

        //2. Check if we can attack
        if (senses.IsInMeleeRange(target) && combat.CanMeleeAttack())
        {
            enemy.StateMachine.ChangeState(new MeleeAttackState(enemy));
            return;
        }

        //3. Check if target is reached
        float distance = Mathf.Abs(target.position.x - enemy.transform.position.x);
        if (distance <= config.turnThreshold)
        {
            rb.linearVelocity = Vector2.zero;

            return;
        }

        //4. Check for obstacles
        if (senses.IsHittingWall() || senses.IsAtCliff())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        //5. Have target, no obstacles, but too far (continue chasing
        enemy.StateMachine.ChangeState(new ChaseState(enemy));
    }

   
}
