using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector3 _inputVector;
    private Rigidbody2D _rigidbody2D;
    private bool disabled;

    private void Awake ()
    {
        disabled = false;
        _rigidbody2D = GetComponent<Rigidbody2D>();

        Guard.OnPlayerSpotted += Guard_OnPlayerSpotted;
    }

    private void Guard_OnPlayerSpotted ()
    {
        disabled = true;
    }

    private void Update ()
    {
        _inputVector = Vector3.zero;

        if (disabled) return;

        _inputVector = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate ()
    {
        Vector3 inputNormal = _inputVector.normalized;
        _rigidbody2D.velocity = moveSpeed * Time.deltaTime * inputNormal;
    }

    private void OnDestroy ()
    {
        Guard.OnPlayerSpotted -= Guard_OnPlayerSpotted;
    }
}
