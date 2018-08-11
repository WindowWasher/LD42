using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerOnSight : MovementBehavior
{
    private GameObject obj;
    private float reach;
    public FollowPlayerOnSight(GameObject obj, float reach)
    {
        this.obj = obj;
        this.reach = reach;
    }

    public override Vector3 getTargetDestination()
    {
        //Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        //var directionFromPlayerToObj = Vector3.Normalize(obj.transform.position - playerPosition);
        //var targetPosition = playerPosition + directionFromPlayerToObj * reach;

        //return targetPosition;
        return GameObject.FindGameObjectWithTag("Player").transform.position;
    }
}
