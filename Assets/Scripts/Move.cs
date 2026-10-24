using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{
    [Header("Pohyb")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Skok a Gravitace")]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -19.62f;

    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // zamknout a skrýt kurzor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // při pauze (otevřený inventář) se nehýbat
        if (Time.timeScale == 0f) return;

        // vstup z klávesnice (WASD)
        float x = 0f;
        float z = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) z += 1f;
            if (Keyboard.current.sKey.isPressed) z -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
        }

        // směr pohybu vzhledem k hráči
        Vector3 moveInput = (transform.right * x + transform.forward * z).normalized;

        // země, gravitace a skok
        if (controller.isGrounded)
        {
            // lehce přitlačit k zemi
            if (verticalVelocity < 0)
                verticalVelocity = -0.5f;

            // skok na mezerník
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else
        {
            // ve vzduchu padá
            verticalVelocity += gravity * Time.deltaTime;
        }

        // aplikace pohybu (chůze + gravitace dohromady)
        Vector3 finalMove = moveInput * moveSpeed + Vector3.up * verticalVelocity;
        controller.Move(finalMove * Time.deltaTime);
    }
}