using UnityEngine;

public class Player_RunningState : EntityState
{
    public Player_RunningState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.anim.SetBool(animBoolName, true);
    }
    public override void Execute()
    {
        base.Execute();

        player.SetVelocity(
            player.moveInput.x * player.moveSpeed,
            player.rb.linearVelocity.y,
            player.moveInput.z * player.moveSpeed
        );

        if (player.moveInput.sqrMagnitude < 0.01f)
            stateMachine.ChangeState(player.idleState);
    }
}
