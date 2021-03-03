using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private Transform watcher;

    [SerializeField] private float sensivity = 100F;
    private float mouseY;
    private float mouseX;
    private float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        mouseY = sensivity * Input.GetAxis("Mouse Y") * Time.deltaTime;
        mouseX = sensivity * Input.GetAxis("Mouse X") * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        watcher.Rotate(Vector3.up * mouseX);
    }
}
