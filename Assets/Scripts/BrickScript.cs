using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : ShotScript
{
    public float Inertia;
    public float InitialAcceleration;

    private Vector2 velocity;
    private float acceleration;
    private float startTime;

	// Use this for initialization
	void OnEnable ()
    {
        Pool.Instance.ActivateObject("jumpSoundEffect").SetActive(true);

        acceleration = InitialAcceleration;
        startTime = 1.0f;
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        if (startTime >= 0)
        {
            startTime -= Time.deltaTime;
            velocity = Direction.normalized;
            return;
        }

        if (Target == null || !Target.activeSelf)
        {
            Target = EnemyManagerScript.Instance.GetClosestEnemyInRange(transform.position, float.PositiveInfinity, EnemyTags);
            if (Target == null) { 
                BlowUp();
            }
        }


        if ( Target != null) { 
        var direction = Target.transform.position - transform.position;
        var angle = MathHelpers.Angle(direction, transform.up) * Time.deltaTime * Inertia * Mathf.Pow(velocity.sqrMagnitude, 0.5f);

         velocity = velocity.Rotate(angle);
        acceleration *= 0.95f;
        velocity += velocity * acceleration * Time.deltaTime;
        
        transform.position += (Vector3)velocity;
        transform.up = velocity;
        }

    }



    public override void BlowUp()
    {
        base.BlowUp();
    }
}
