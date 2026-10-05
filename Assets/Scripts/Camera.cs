using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
    [Header("Citlivost a limity")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float minVerticalAngle = -80f; // Limit pohledu dolů
    [SerializeField] private float maxVerticalAngle = 80f;  // Limit pohledu nahoru

    [Header("Odkazy")]
    [SerializeField] private Transform playerBody; // Přetáhni sem objekt postavy (Player)

    private float verticalRotation = 0f;

    private void Awake()
    {
        // Zamčení a skrytí kurzoru
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Pokud není přiřazen playerBody v Inspectoru, zkusíme najít rodiče
        if (playerBody == null && transform.parent != null)
        {
            playerBody = transform.parent;
        }
    }

    private void LateUpdate()
    {
        if (Mouse.current == null) return;

        // Načtení pohybu myši
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;

        // 1. Vertikální rotace (rozhlížení nahoru a dolů)
        verticalRotation -= mouseDelta.y;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // 2. Horizontální rotace (otáčení celé postavy do stran)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseDelta.x);
        }
    }
}