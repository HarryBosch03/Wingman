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

    public GameObject Shooter { get; set; }

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
            Health health = hit.transform.GetComponentInParent<Health>();
            if (health)
            {
                health.Damage(new DamageArgs(Shooter, damage));
            }

            Instantiate(hitPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(gameObject);
        }
    }
}
