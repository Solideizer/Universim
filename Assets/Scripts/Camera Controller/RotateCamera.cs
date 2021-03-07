using UnityEngine;

public class RotateCamera : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Transform watcher;

    [SerializeField] private float sensivity = 100f;
#pragma warning restore 0649
    private float _mouseY;
    private float _mouseX;
    private float _xRotation = 0f;
    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update ()
    {
        Rotate ();
    }

    private void Rotate ()
    {
        _mouseY = sensivity * Input.GetAxis ("Mouse Y") * Time.deltaTime;
        _mouseX = sensivity * Input.GetAxis ("Mouse X") * Time.deltaTime;

        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp (_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler (_xRotation, 0f, 0f);
        watcher.Rotate (Vector3.up * _mouseX);
    }
}