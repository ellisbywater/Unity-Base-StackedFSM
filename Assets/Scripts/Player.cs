using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 movementVector;
    private Animator _animator;
    private float speed;
    private Rigidbody _rigidbody;
    private Health _health;
    
    // Start is called before the first frame update
    void Start()
    {
        _health = GetComponent<Health>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        speed = 1.5f;
        _health.SetHealth(20);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (movementVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementVector), 0.25f );
        }
        _animator.SetBool("Walking", movementVector != Vector3.zero);
    }

    void CalculateMovement()
    {
        movementVector = new Vector3(Input.GetAxis("Horizontal"), _rigidbody.velocity.y, Input.GetAxis("Vertical"));
        _rigidbody.velocity = new Vector3(movementVector.x * speed, movementVector.y, movementVector.z * speed);
    }

    public void Hurt(int amount, float delay = 0.5f)
    {
        StartCoroutine(_health.TakeDamageDelayed(amount, delay));
    }
}
