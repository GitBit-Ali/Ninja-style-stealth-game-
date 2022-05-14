using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector3 _inputVector;
    private Rigidbody2D _rigidbody2D;

    private void Awake ()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update ()
    {
        _inputVector = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate ()
    {
        Vector3 inputNormal = _inputVector.normalized;
        _rigidbody2D.velocity = moveSpeed * Time.deltaTime * inputNormal;
    }
}
