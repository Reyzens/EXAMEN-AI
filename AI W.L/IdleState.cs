using UnityEngine;


public class FleeState : CharacterState
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

        return m_stateMachine.IsIdle();

    }

    public override bool CanExit()
    {
        return !m_stateMachine.IsIdle();
    }

}
