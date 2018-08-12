using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Arrow : MonoBehaviour {

    private float speed = 50f;

    private Vector3 targetPosition;
    private Vector3 direction;

    private Timer deathTimer = null;

    private float force = 300000f;
    public int damage = 10;

	// Use this for initialization
	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {
        //this.transform.LookAt(targetPosition);
        //this.transform.Translate(this.transform.forward * Time.deltaTime * speed);
        //float step = speed * Time.deltaTime;

        // Move our position a step closer to the target.
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        //this.transform.Translate(this.transform.forward * Time.deltaTime * speed);

        //this.transform.Translate(direction * Time.deltaTime * speed);

        if(deathTimer != null && deathTimer.Expired())
        {
            Destroy(this.gameObject);
        }
    }

    public void setTargetPosition(Vector3 startPosition, Vector3 targetPosition)
    {
        //this.gameObject.transform.LookAt(targetPosition);

        this.targetPosition = targetPosition;

        //this.transform.position = Vector3.zero;
        //this.transform.localPosition = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.position = startPosition;


        this.transform.LookAt(targetPosition);

        //this.transform.position = startPosition;
        //bullet.transform.LookAt(targetPosition);

        var rb = this.GetComponent<Rigidbody>();
        rb.AddForce(this.transform.forward * force);


        direction = Vector3.Normalize(targetPosition - transform.position);

        //Debug.DrawRay(this.transform.position, direction, Color.blue);
        //Debug.Log("Direction: " + direction.ToString());

        //this.transform.forward = direction;

        //Vector3 targetDir = Vector3.Normalize(targetPosition - transform.position);

        //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 100f, 0.0f);
        //transform.rotation = Quaternion.LookRotation(newDir);


        //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = targetPosition;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            return;
        }

        Health otherHealth = other.gameObject.GetComponentInParent<Health>();

        if (otherHealth != null)
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            EnemyBodyPart enemyPart = other.GetComponent<EnemyBodyPart>();
            if (enemy == null || enemyPart == null)
                return;
            //if(enemy != null && enemyPart != null)
            //{
                enemy.HitWithArrow(enemyPart, this);
            //}
            //else {
            //    otherHealth.TakeDamage(damage);
            //}
            
        }
        else
        {
            deathTimer = new Timer();
            deathTimer.Start(60f);
        }

        // do this to avoid scaling issues
        GameObject newObj = new GameObject();
        this.transform.parent = newObj.transform;
        newObj.transform.parent = other.transform;
        //newObj.transform.parent = other.GetComponentInParent<Enemy>().GetComponentInChildren<EnemyBodyPart>().gameObject.transform;

        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        //this.transform.parent = other.transform;
        this.GetComponent<CapsuleCollider>().enabled = false;



    }
}
