using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AttackManager : MonoBehaviour
{

    public List<AttackData> attackData;
    private List<Attack> attacks;
    public Attack currentAttack;
    private List<GameObject> targets;
    public float meeleAttackRange;
    private float globalCooldown = 0f;
    public Timer canAttackTimer = new Timer();

    public NavMeshAgent navMeshAgent;

    PlayerMovementController playerController;

    private GameObject currentTarget;
    // Use this for initialization
    void Start()
    {

        InitTargets();

        navMeshAgent = GetComponent<NavMeshAgent>();

        playerController = GameObject.Find("Player").GetComponent<PlayerMovementController>();

        // create attacks from the attack data
        attacks = new List<Attack>();
        foreach (var data in attackData)
        {
            attacks.Add(new Attack(data, meeleAttackRange));
        }
    }

    void InitTargets()
    {
        targets = new List<GameObject>();
        targets.Add(GameObject.Find("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAttack != null)
        {
            if (currentAttack.attackLengthTimer.Expired())
            {
                if(currentTarget != null && currentTarget.layer != LayerMask.NameToLayer("PlayerLayer"))
                {
                    currentAttack.DoDamage(currentTarget.GetComponent<Health>());
                }


                currentAttack = null;
                navMeshAgent.updatePosition = true;
                navMeshAgent.updateRotation = true;
            }
            else
            {
                // Still animating attack
                return;
            }
        }

        if (!canAttackTimer.Expired())
        {
            // we aren't ready to attack again yet
            return;
        }

        UnFreezeIfHolding();

        Attack nextAttack = GetNextAttack();
        GameObject target = findTargetInRange(nextAttack);
        if (target)
        {
            currentTarget = target;
            // we found a target in range, so lets attack it
            BeginAttack(nextAttack);
        }
    }

    public void addTarget(GameObject obj)
    {
        targets.Add(obj);
    }

    Attack GetNextAttack()
    {
        // only select ones that aren't on cooldown
        var readyAttacks = attacks.Where(a => a.attackCooldownTimer.Expired()).ToList();
        if (readyAttacks.Count == 0)
            return null;

        // Let's just select randomly for now. Will probably change this to a percentage later.
        return RandomUtil.choice(readyAttacks);
    }

    void BeginAttack(Attack attack)
    {
        attack.BeginAttack(GetComponent<Animator>());
        canAttackTimer.Start(attack.attackLength + globalCooldown);
        currentAttack = attack;

        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
    }

    public void HitHealth(Health healthHit, GameObject weaponUsed)
    {
        // check we are doing an attack
        // and the weapon used is part of the current attack's weaponized objects
        // and the health is a valid target
        if (currentAttack == null || !currentAttack.validWeaponObj(weaponUsed.name) || !currentAttack.ValidHealth(healthHit))
            return;

        if(healthHit.transform.root.tag == "Player")
        {
            if (weaponUsed.name == "HeadBone")
            {
                UnFreezeIfHolding();
            }
            else
            {
                healthHit.GetComponent<PlayerMovementController>().Freeze(this.transform.gameObject);
                return;
            }
        }


        currentAttack.DoDamage(healthHit);
    }

    public void UnFreezeIfHolding()
    {
        if (playerController.enemyHoldingPlayer == this.transform.gameObject)
        {
            playerController.UnFreeze();
        }
    }

    public GameObject findTargetInRange(Attack attack)
    {
        foreach (GameObject target in targets)
        {
            if (target!=null && Vector3.Distance(target.transform.position, this.transform.position) <= attack.attackRange)
            {
                return target;
            }
        }
        return null;
    }
}
