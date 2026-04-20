using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected Rigidbody rb;
    protected Player player;

    public EntityState(StateMachine stateMachine, string animBoolName, Player player)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        this.player = player;
    }

    public virtual void Enter()
    {
        rb = player.rb;
    }

    public virtual void Execute()
    {
    }

    public virtual void Exit()
    {
    }
}
