using UnityEngine;
using UnityEngine.InputSystem; // Nutné pro nový Input System

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{
    [Header("Pohyb")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 0.1f;

    [Header("Skok a Gravitace")]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -19.62f;

    [Header("Animace")]
    [SerializeField] private Animator animator;

    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Automatické dohledání Animatoru, pokud není ručně vložený
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Zamčení a skrytí kurzoru myši
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // 1. Otáčení postavou do stran pomocí myši
        if (Mouse.current != null)
        {
            float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseX);
        }

        // 2. Načtení vstupů klávesnice (WASD)
        float x = 0f;
        float z = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) z += 1f;
            if (Keyboard.current.sKey.isPressed) z -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
        }

        Vector3 moveInput = (transform.right * x + transform.forward * z).normalized;

        // 3. Správná kontrola podlahy a gravitace
        if (controller.isGrounded)
        {
            // Jemnější přitlačení k zemi, aby to nezpůsobovalo odraz
            if (verticalVelocity < 0)
            {
                verticalVelocity = -0.5f; 
            }

            // Skok na Mezerník (Space)
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // 4. Předání stavu do Animatoru
        bool isMoving = moveInput.magnitude > 0.1f;
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }

        // 5. Aplikace pohybu
        Vector3 finalMove = moveInput * moveSpeed + Vector3.up * verticalVelocity;
        controller.Move(finalMove * Time.deltaTime);
    }
}