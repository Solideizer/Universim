using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private int speed;
    private Vector3 moveVector;
    private Vector3 riseVector;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveVector = transform.right * x + transform.forward * z;
        characterController.Move(moveVector * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.C))
        {
            riseVector.y = speed * Time.deltaTime;
            characterController.Move(riseVector);
        }

        if (Input.GetKey(KeyCode.V))
        {
            riseVector.y = speed * Time.deltaTime * -1;
            characterController.Move(riseVector);
        }
    }
}
