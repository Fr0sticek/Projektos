using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
    [Header("Citlivost a limity")]
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float minVerticalAngle = -85f; // limit pohledu dolů
    [SerializeField] private float maxVerticalAngle = 85f;  // limit pohledu nahoru

    [Header("Odkazy")]
    [SerializeField] private Transform playerBody; // objekt Player

    private float verticalRotation = 0f;

    private void Awake()
    {
        // zamknout a skrýt kurzor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // když chybí hráč, vezmi rodiče
        if (playerBody == null && transform.parent != null)
        {
            playerBody = transform.parent;
        }
    }

    private void LateUpdate()
    {
        if (Mouse.current == null) return;

        // pohyb myši * citlivost
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;

        // nahoru/dolů: otáčí se kamera
        verticalRotation -= mouseDelta.y;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // doleva/doprava: otáčí se hráč
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseDelta.x);
        }
    }
}