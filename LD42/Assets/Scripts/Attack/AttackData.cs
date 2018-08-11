using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "AttackData")]
public class AttackData : ScriptableObject
{
    /// <summary>
    /// Animation clip to play during this attack
    /// </summary>
    public AnimationClip animationClip;

    /// <summary>
    /// Name of game objects that will do damage if
    /// they collide with a game object with health
    /// </summary>
    public List<string> weaponizedObjects;

    /// <summary>
    /// Range of the attack.  If left at zero, will use default attack range.
    /// </summary>
    public float attackRange;

    /// <summary>
    /// Base damage applied on hit
    /// </summary>
    public int baseDamage;

    /// <summary>
    /// How often during an attack, hitting the same object
    /// can do damage.  If set to zero, an object can only be
    /// hit once per attack.
    /// </summary>
    public float damageInterval;

    /// <summary>
    /// How long until the attack is ready again.
    /// </summary>
    public float postAttackCooldown;
}
