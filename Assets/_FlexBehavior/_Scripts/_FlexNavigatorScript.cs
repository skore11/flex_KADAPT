using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _FlexNavigatorScript : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent flexAgent;
    //the animator is present only on the original game object
    private float angleDiff;
    [HideInInspector]
    public Quaternion desiredOrientation { get; set; }

    public Animator orig_Animator = null;

    private int m_SpeedId = 0;
    private int m_AgularSpeedId = 0;
    private int m_DirectionId = 0;

    public float m_SpeedDampTime = 0.1f;
    public float m_AnguarSpeedDampTime = 0.25f;
    public float m_DirectionResponseTime = 0.2f;

    void Start()
    {
        this.Initialize();
    }

    public void Initialize()
    {
        flexAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        desiredOrientation = transform.rotation;
        m_SpeedId = Animator.StringToHash("Speed");
        m_AgularSpeedId = Animator.StringToHash("AngularSpeed");
        m_DirectionId = Animator.StringToHash("Direction");
    }

    protected void FlexAgentLocomotion()
    {
        if (AgentDone())
        {
            flexAgent.ResetPath();

            animCheck(0, angleDiff);
        }
        else
        {
            float speed = flexAgent.desiredVelocity.magnitude;

            Vector3 velocity = Quaternion.Inverse(transform.rotation) * flexAgent.desiredVelocity;

            float angle = Mathf.Atan2(velocity.x, velocity.z) * 180.0f / 3.14159f;

            animCheck(speed, angle);
        }
    }

    public bool AgentDone()
    {
        return !flexAgent.pathPending && AgentStopping();
    }

    protected bool AgentStopping()
    {
        return flexAgent.remainingDistance <= flexAgent.stoppingDistance;
    }

    void OnAnimatorMove()
    {
        flexAgent.velocity = orig_Animator.deltaPosition / Time.deltaTime;
        transform.rotation = orig_Animator.rootRotation;
        //get forward vector for each rotation
        var forwardA = transform.rotation * Vector3.forward;
        var forwardB = desiredOrientation * Vector3.forward;
        //get numeric angle for each vector, on the X-Z plane (relative to world forward)
        var angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
        var angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;
        //get signed difference in these angles
        angleDiff = Mathf.DeltaAngle(angleA, angleB);
    }

    public void animCheck(float speed, float direction)
    {
        AnimatorStateInfo state = orig_Animator.GetCurrentAnimatorStateInfo(0);

        bool inTransition = orig_Animator.IsInTransition(0);
        bool inIdle = state.IsName("Locomotion.Idle");
        bool inTurn = state.IsName("Locomotion.TurnOnSpot") || state.IsName("Locomotion.PlantNTurnLeft") || state.IsName("Locomotion.PlantNTurnRight");
        bool inWalkRun = state.IsName("Locomotion.WalkRun");

        float speedDampTime = inIdle ? 0 : m_SpeedDampTime;
        float angularSpeedDampTime = inWalkRun || inTransition ? m_AnguarSpeedDampTime : 0;
        float directionDampTime = inTurn || inTransition ? 1000000 : 0;

        float angularSpeed = direction / m_DirectionResponseTime;

        orig_Animator.SetFloat(m_SpeedId, speed, speedDampTime, Time.deltaTime);
        orig_Animator.SetFloat(m_AgularSpeedId, angularSpeed, angularSpeedDampTime, Time.deltaTime);
        orig_Animator.SetFloat(m_DirectionId, direction, directionDampTime, Time.deltaTime);
    }

    private void Update()
    {
        FlexAgentLocomotion();
    }
}
