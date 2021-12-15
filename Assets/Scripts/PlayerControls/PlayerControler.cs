using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerControler : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Rigidbody _rb;

    private bool _canJump = true;

    Vector2 _move;
    Vector2 _look;
    float _jumpValue;


    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    Vector3 nextPosition;
    [SerializeField]
    Quaternion nextRotation;

    [SerializeField]
    float rotationPower = 0.2f;
    [SerializeField]
    float rotationLerp = 0.1f;
    [SerializeField]
    float jumpForce = 20;

    [SerializeField]
    float speed = 1f;
    [SerializeField]
    Camera camera;

    [SerializeField]
    GameObject followTransform;

    private void Awake()
    {
        _agent = transform.GetComponent<NavMeshAgent>();
        _rb = transform.GetComponent<Rigidbody>();    
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _jumpValue = context.ReadValue<float>();    
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }


    
    private void Update()
    {
        followTransform.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

        followTransform.transform.rotation *= Quaternion.AngleAxis(-_look.y * rotationPower, Vector3.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1.1f, Color.red);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit,  1.1f, layerMask))
        {
            if(hit.collider.gameObject.tag == "Ground")
            {
                _canJump = true;
            }
        }

        var angle = followTransform.transform.localEulerAngles.x;

        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        if (_jumpValue == 1 && _canJump)
        {
            _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            _canJump = false;
        }

        float moveSpeed = speed / 100f;

        Vector3 position = (transform.forward * _move.y * moveSpeed) + (transform.right * _move.x * moveSpeed);

        transform.position += position;

        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }
}
