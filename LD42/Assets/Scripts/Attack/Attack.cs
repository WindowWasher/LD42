using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{

    /// <summary>
    /// Scriptable object holding attack data
    /// </summary>
    private AttackData data;

    /// <summary>
    /// Timer to track when the attack will be ready again
    /// </summary>
    public Timer attackCooldownTimer = new Timer();

    /// <summary>
    /// Timer to track when the attack has finished animating
    /// </summary>
    public Timer attackLengthTimer = new Timer();

    /// <summary>
    /// Dictionary to keep track of what health objects have been hit
    /// and when they can be hit again.
    /// </summary>
    public Dictionary<Health, Timer> hitTimers;

    /// <summary>
    /// How long this attack lasts in seconds
    /// </summary>
    public float attackLength { get { return data.animationClip.length; } }

    /// <summary>
    /// Range of the attack.
    /// </summary>
    public float attackRange;

    /// <summary>
    /// Init the attack data.  If no range is specified for this attack, use the default attack range
    /// </summary>
    /// <param name="data"></param>
    /// <param name="defaultAttackRange"></param>
    public Attack(AttackData data, float defaultAttackRange)
    {
        this.data = data;
        attackRange = data.attackRange == 0 ? defaultAttackRange : data.attackRange;
        ResetHitTimers();
    }

    /// <summary>
    /// Start the attack animation
    /// </summary>
    /// <param name="animator"></param>
    public void BeginAttack(Animator animator)
    {
        ResetHitTimers();
        attackLengthTimer.Start(attackLength / animator.speed);
        attackCooldownTimer.Start(data.postAttackCooldown);
        // TODO I really need to fix this
        animator.Play(data.animationClip.name.Replace("Clip", "") + "BlendTree");
    }

    /// <summary>
    /// Returns true if the weapon name is part of the attack's weaponized objects.
    /// For instance, if an enemy is punching, the foot won't be weaponized but the hand will be
    /// </summary>
    /// <param name="weaponName"></param>
    /// <returns></returns>
    public bool validWeaponObj(string weaponName)
    {
        return data.weaponizedObjects.Contains(weaponName);
    }

    /// <summary>
    /// Check if the health is hittable
    /// </summary>
    /// <param name="health"></param>
    /// <returns></returns>
    public bool ValidHealth(Health health)
    {
        if (health == null)
            return false;

        // We are valid if we haven't hit this health during the attack
        if (!hitTimers.ContainsKey(health))
            return true;
        // we are invalid if we have hit this health during the attack
        // and the damageInterval is zero (meaning can only hit once per attack)
        else if (data.damageInterval == 0)
            return false;

        // otherwise, return true if the timer for this attack has expired
        return hitTimers[health].Expired();
    }

    /// <summary>
    /// Do damage to the health
    /// </summary>
    /// <param name="health"></param>
    public void DoDamage(Health health)
    {
        health.TakeDamage(data.baseDamage);
        hitTimers[health] = new Timer();
        hitTimers[health].Start(data.damageInterval);

        AudioPlayer audioPlayer = health.gameObject.GetComponent<AudioPlayer>();

        if (audioPlayer)
        {
            audioPlayer.PlaySound();
        }
    }

    /// <summary>
    /// Reset the dictionary tracking health hit during an attack
    /// </summary>
    public void ResetHitTimers()
    {
        hitTimers = new Dictionary<Health, Timer>();
    }

}
