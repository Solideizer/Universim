using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject stats;

#pragma warning restore 0649

    private void Update ()
    {
        if (Input.GetKey (KeyCode.Space))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            stats.SetActive (true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            stats.SetActive (false);
        }

    }

}