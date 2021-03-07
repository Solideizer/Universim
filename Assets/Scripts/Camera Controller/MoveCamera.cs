using UnityEngine;

public class MoveCamera : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private int _speed = 100;
#pragma warning restore 0649
    private Vector3 _moveVector;
    private Vector3 _riseVector;
    private CharacterController _characterController;

    void Start ()
    {
        _characterController = GetComponent<CharacterController> ();
    }

    void Update ()
    {
        Move ();
    }

    private void Move ()
    {
        float x = Input.GetAxis ("Horizontal");
        float z = Input.GetAxis ("Vertical");

        _moveVector = transform.right * x + transform.forward * z;
        _characterController.Move (_moveVector * _speed * Time.deltaTime);

        if (Input.GetKey (KeyCode.Q))
        {
            _riseVector.y = _speed * Time.deltaTime;
            _characterController.Move (_riseVector);
        }

        if (Input.GetKey (KeyCode.E))
        {
            _riseVector.y = _speed * Time.deltaTime * -1;
            _characterController.Move (_riseVector);
        }
    }
}