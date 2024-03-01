using UnityEngine;


public class IdleState : CharacterState
{
    private float m_idleTimeDuration = 3.0f;
    private float m_currentIdleDuration;

    public override void OnEnter()
    {
        Debug.Log("Enter state: Idle\n");
        m_currentIdleDuration = m_idleTimeDuration;
    }

    public override void OnUpdate()
    {
        // Check if the character is still within their idle duration period.
        if (m_currentIdleDuration > 0)
        {
            m_currentIdleDuration -= Time.deltaTime;
        }
        else
        {
            m_stateMachine.SetIdle(false);
        }
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {
        Debug.Log("Exit idle state");
    }

    public override bool CanEnter(IState currentState)
    {
        // si lennemi est plus fort que notre AI
        return m_stateMachine.IsIdle();

    }

    public override bool CanExit()
    {
        // si lennemi nest plus en porte de notre AI
        return !m_stateMachine.IsIdle();
    }

}
