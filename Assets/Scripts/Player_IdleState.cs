using UnityEngine;

public class Player_IdleState : EntityState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.anim.SetBool(animBoolName, false);
        player.SetVelocity(0, player.rb.linearVelocity.y, 0);
    }
    public override void Execute()
    {
        base.Execute();
        if (player.moveInput.sqrMagnitude > 0.01f)
            stateMachine.ChangeState(player.runningState);
    }
}
