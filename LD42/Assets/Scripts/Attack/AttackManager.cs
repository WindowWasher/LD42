using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackManager : MonoBehaviour
{

    public List<AttackData> attackData;
    private List<Attack> attacks;
    private Attack currentAttack;
    private List<GameObject> targets;
    public float meeleAttackRange;
    private float globalCooldown = 0f;
    public Timer canAttackTimer = new Timer();

    // Use this for initialization
    void Start()
    {

        InitTargets();

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
        if (currentAttack != null && currentAttack.attackLengthTimer.Expired())
        {
            currentAttack = null;
        }

        if (!canAttackTimer.Expired())
        {
            // we aren't ready to attack again yet
            return;
        }

        Attack nextAttack = GetNextAttack();
        GameObject target = findTargetInRange(nextAttack);
        if (target)
        {
            // we found a target in range, so lets attack it
            BeginAttack(nextAttack);
        }
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
    }

    public void HitHealth(Health healthHit, GameObject weaponUsed)
    {
        // check we are doing an attack
        // and the weapon used is part of the current attack's weaponized objects
        // and the health is a valid target
        if (currentAttack == null || !currentAttack.validWeaponObj(weaponUsed.name) || !currentAttack.ValidHealth(healthHit))
            return;

        currentAttack.DoDamage(healthHit);
    }

    GameObject findTargetInRange(Attack attack)
    {
        foreach (GameObject target in targets)
        {
            if (Vector3.Distance(target.transform.position, this.transform.position) <= attack.attackRange)
            {
                return target;
            }
        }
        return null;
    }
}
