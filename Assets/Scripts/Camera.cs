using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
    [Header("Citlivost a limity")]
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float minVerticalAngle = -85f; // Limit pohledu dolů
    [SerializeField] private float maxVerticalAngle = 85f;  // Limit pohledu nahoru

    [Header("Odkazy")]
    [SerializeField] private Transform playerBody; // Přetáhni sem objekt Player

    private float verticalRotation = 0f;

    private void Awake()
    {
        // Uzamčení kurzoru uprostřed obrazovky
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Automatické nalezení rodiče (Player), pokud není ručně přiřazen
        if (playerBody == null && transform.parent != null)
        {
            playerBody = transform.parent;
        }
    }

    private void LateUpdate()
    {
        // Kontrola, zda je myš připojená
        if (Mouse.current == null) return;

        // Získání pohybu myši od posledního snímku (BEZ Time.deltaTime!)
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;

        // 1. Pohled NAHORU a DOLŮ (rotujeme pouze samotnou kamerou kolem osy X)
        verticalRotation -= mouseDelta.y;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        // Aplikujeme lokalní rotaci na kameru
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // 2. Pohled DO STRAN (otáčíme celým hráče kolem osy Y)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseDelta.x);
        }
    }
}