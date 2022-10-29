using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public sealed class Projectile : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float muzzleSpeed;

    [Space]
    [SerializeField] float collisionSize;
    [SerializeField] LayerMask collisionMask;

    [Space]
    [SerializeField] GameObject hitPrefab;

    new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = transform.forward * muzzleSpeed;
    }

    private void FixedUpdate()
    {
        float speed = rigidbody.velocity.magnitude;
        Ray ray = new Ray(rigidbody.position, rigidbody.velocity);
        if (Physics.SphereCast(ray, collisionSize, out RaycastHit hit, speed * Time.deltaTime + 0.01f, collisionMask))
        {
            Instantiate(hitPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(gameObject);
        }
    }
}
