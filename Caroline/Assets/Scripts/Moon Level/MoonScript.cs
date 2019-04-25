using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScript : MonoBehaviour
{
    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;

    private Explodable _explodable;

    private void Awake()
    {
        _explodable = GetComponent<Explodable>();
        targetPos = transform.position;
    }

    private void Update()
    {
        this.transform.localScale = new Vector2((target.transform.position.y / 60) + 0.5f, (target.transform.position.y / 60) + 0.5f);
        //this.transform.position = new Vector2(girlTrans.position.x, girlTrans.position.y + increaseY);
    }

    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * 5f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

        }
    }

    public void explodeWithForce()
    {
        _explodable.explode();
		ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
		ef.doExplosion(transform.position);
    }



}
