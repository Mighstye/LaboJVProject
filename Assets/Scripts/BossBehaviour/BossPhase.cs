using System;
using System.Collections;
using System.Collections.Generic;
using BossBehaviour;
using BulletSystem;
using Logic_System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BossPhase : StateMachineBehaviour
{
    public enum PhaseEndType
    {
        DealDamage,
        Endure
    }
    public enum PhaseType
    {
        SpellPhase,
        NonSpellPhase
    }
    [SerializeField] private int phaseFsmIndexNumber=0;
    private PhaseIndex phaseIndex;
    private Animator phaseBehaviors;
    [SerializeField] private string phaseName;
     private PhaseTimer phaseTimer;
     private BossController bossController;
     private BattleOutcome battleOutcome;
     [SerializeField] public PhaseEndType phaseEndType;
    [SerializeField] public PhaseType phaseType;
    [SerializeField] private float phaseDuration;
    [SerializeField] private int phaseHp;
    private static readonly int PhaseEnd = Animator.StringToHash("phaseEnd");
    private Action phaseEnd;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        phaseIndex = animator.GetComponent<PhaseIndex>();
        phaseBehaviors = phaseIndex.phaseStateMachines[phaseFsmIndexNumber];
        phaseBehaviors.gameObject.SetActive(true);
        phaseEnd = () => { SetPhaseEndVar(animator, true);};
        SetPhaseEndVar(animator,false);
        InitializeTimer(animator);
        InitializeBossController(animator);
        OnPhaseStartCustom(animator, stateInfo,layerIndex);
        if (phaseType is PhaseType.NonSpellPhase) return;
        battleOutcome = LogicSystemAPI.instance.battleOutcome;
        battleOutcome.RecordNewPhase(phaseName);
    }

    protected virtual void OnPhaseStartCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){}

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ActiveBulletManager.instance.Wipe();
        phaseBehaviors.gameObject.SetActive(false);
        phaseTimer.onPhaseTimeoutReached -= phaseEnd;
        if (bossController is null) return;
        bossController.onHpDepleted -= phaseEnd;
        battleOutcome.RegisterCurrentPhase();

    }

    private static void SetPhaseEndVar(Animator animator, bool state)
    {
        animator.SetBool(PhaseEnd,state);
    }

    private void InitializeTimer(Animator animator)
    {
        phaseTimer = PhaseTimer.instance;
        phaseTimer.SetUpTimer(phaseDuration,phaseName);
        phaseTimer.onPhaseTimeoutReached += phaseEnd;
    }

    private void InitializeBossController(Animator animator)
    {
        
        if (phaseEndType is not PhaseEndType.DealDamage) return;
        bossController = BossBehaviourSystemProxy.instance.bossController;
        bossController.SetUpHp(phaseHp);
        bossController.onHpDepleted += phaseEnd;
    }
    
}
