using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("Reference")]
    public Inventory inventory;          // inventář hráče
    public GameObject panel;             // panel inventáře (zapíná se/vypíná)
    public Transform slotsParent;        // objekt s Grid Layout Group
    public InventorySlotUI slotPrefab;   // prefab jednoho slotu

    [Header("Nastavení")]
    public bool pauseGameWhenOpen = true;  // zastavit hru při otevření
    public bool unlockCursor = true;       // odemknout kurzor při otevření

    private InventorySlotUI[] slotUIs;
    private bool isOpen;

    void Start()
    {
        // vytvoř UI sloty podle inventáře
        slotUIs = new InventorySlotUI[inventory.slots.Count];
        for (int i = 0; i < slotUIs.Length; i++)
            slotUIs[i] = Instantiate(slotPrefab, slotsParent);

        // překresli UI při každé změně inventáře
        inventory.OnChanged += Refresh;
        Refresh();
        SetOpen(false);
    }

    void OnDestroy()
    {
        // odhlášení odběru události
        if (inventory != null) inventory.OnChanged -= Refresh;
    }

    void Update()
    {
        // klávesa I otevře/zavře inventář
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
            SetOpen(!isOpen);
    }

    // Otevře nebo zavře inventář
    void SetOpen(bool open)
    {
        isOpen = open;
        panel.SetActive(open);

        // pauza hry
        if (pauseGameWhenOpen)
            Time.timeScale = open ? 0f : 1f;

        // kurzor: odemknout při otevření, zamknout při zavření
        if (unlockCursor)
        {
            Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = open;
        }

        if (open) Refresh();
    }

    // Překreslí všechny sloty
    void Refresh()
    {
        for (int i = 0; i < slotUIs.Length; i++)
            slotUIs[i].SetSlot(inventory.slots[i]);
    }
}