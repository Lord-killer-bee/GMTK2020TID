using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : EnemyBaseBehaviour
{ 
    [SerializeField] private float followSpeed = 50f;
    
    private Rigidbody rigidBody;
    
    bool behaviourInitialized = false;
    
    GameObject target;
    
    public override void InitializeBehaviour(GameObject target)
    {
        behaviourInitialized = true;
        this.target = target;
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (target)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        //Interpolating the speed in order to reduce the relative motion from environment movement
    
        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, followSpeed * Time.deltaTime);
        rigidBody.velocity = (target.transform.position - transform.position).normalized * followSpeed;
        transform.LookAt(target.transform);
    
        Debug.DrawLine(transform.position, target.transform.position, Color.red);
    }
    
    private void OnCollisionEnter(Collision collision)
    {

    }

}
