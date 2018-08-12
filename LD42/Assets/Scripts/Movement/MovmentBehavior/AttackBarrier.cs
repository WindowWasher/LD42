using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBarrier : MovementBehavior
{
    private GameObject obj;
    private float reach;
    public GameObject target;
    public AttackBarrier(GameObject obj, float reach, GameObject target)
    {
        this.obj = obj;
        this.reach = reach;
        this.target = target;
    }

    public override Vector3 getTargetDestination()
    {
        //Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        //var directionFromPlayerToObj = Vector3.Normalize(obj.transform.position - playerPosition);
        //var targetPosition = playerPosition + directionFromPlayerToObj * reach;

        //return targetPosition;
        if (!target)
            return Vector3.zero;
        return target.transform.position;
    }
}
