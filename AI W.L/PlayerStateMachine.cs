using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private Transform m_player;
    [SerializeField] private float m_detectionDistance;
    [SerializeField] public Transform m_ennemy;
    private NavMeshAgent m_agent;

    #region States Variables
    private List<CharacterState> m_possibleStates;
    private CharacterState m_currentState;
    #endregion

    #region Waypoints Variables
    public Transform[] m_waypoints;
    public int m_currentWaypointIndex = 0;
    #endregion

    #region Booleans
    private bool m_isIdle = false;
    #endregion

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    // Initialize possible states for the state machine.
    private void CreatePossibleStates()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new RoamingState());
        m_possibleStates.Add(new FleeState());
        m_possibleStates.Add(new CombatState());
        
    }

    void Start()
    {
        CreatePossibleStates();

        // Initialize each state with a reference to this state machine.
        foreach (CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();

    }

    private void Update()
    {
        m_currentState.OnUpdate();
        TryStateTransition();
    }


    private void FixedUpdate()
    {
        m_currentState.OnFixedUpdate();
    }

    // Attempts to transition from the current state to a different state.
    private void TryStateTransition()
    {
        if (!m_currentState.CanExit())
        {
            return;
        }

        // Loop through all possible states to find a state that can be entered.
        foreach (var state in m_possibleStates)
        {
            if (m_currentState.Equals(state))
            {
                continue;
            }

            if (state.CanEnter(m_currentState))
            {
                // Transition to the new state.
                m_currentState.OnExit();
                m_currentState = state;
                m_currentState.OnEnter();
                return;
            }
        }
    }

    // Returns the current state of the character.
    public CharacterState GetCurrentState()
    {
        return m_currentState;
    }

    // Checks if the player is within detection distance.
    public bool PlayerIsNear()
    {

        RaycastHit hit;

        Vector3 direction = m_player.transform.position - transform.position;

        // Perform a raycast towards the player. If the player is detected within the distance, return true.
        if (Physics.Raycast(transform.position, direction, out hit, m_detectionDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.DrawRay(transform.position, direction, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.red);
                return false;
            }

        }
        return false;
    }

    public bool EnnemyIsNear()
    {

        RaycastHit hit;

        Vector3 direction = m_ennemy.transform.position - transform.position;

        // Perform a raycast towards the player. If the Ennemy is detected within the distance, return true.
        if (Physics.Raycast(transform.position, direction, out hit, m_detectionDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.DrawRay(transform.position, direction, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.red);
                return false;
            }

        }
        return false;
    }

    // Returns the NavMesh agent component.
    public NavMeshAgent GetAgent()
    {
        return m_agent;
    }

    // Returns the player's transform.
    public Transform GetPlayerTransform()
    {
        return m_player;
    }

    // Sets the idle state of the character.
    public void SetIdle(bool isIdle)
    {
        m_isIdle = isIdle;
    }

    public bool IsIdle() { return m_isIdle; }
}



