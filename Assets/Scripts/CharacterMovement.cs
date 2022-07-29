using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Vector3 _playerMovementInput;

    private Rigidbody _rb;

    [SerializeField] private float speed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        MovePlayer();
    }

    private void MovePlayer()
    {
        var moveVector = transform.TransformDirection(_playerMovementInput) * speed;
        _rb.velocity = new Vector3(moveVector.x, _rb.velocity.y, moveVector.z);
    }
}
