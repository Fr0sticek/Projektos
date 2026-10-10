using UnityEngine;

// Dej na Canvas (nebo jakýkoli aktivní objekt, NE na samotný panel!)
public class InventoryUI : MonoBehaviour
{
    [Header("Reference")]
    public Inventory inventory;          // inventář hráče
    public GameObject panel;             // panel inventáře (bude se zapínat/vypínat)
    public Transform slotsParent;        // objekt s Grid Layout Group
    public InventorySlotUI slotPrefab;   // prefab jednoho slotu

    [Header("Nastavení")]
    public KeyCode toggleKey = KeyCode.I;
    public bool pauseGameWhenOpen = true;
    public bool unlockCursor = true;

    private InventorySlotUI[] slotUIs;
    private bool isOpen;

    void Start()
    {
        // vytvoř sloty
        slotUIs = new InventorySlotUI[inventory.slots.Count];
        for (int i = 0; i < slotUIs.Length; i++)
        {
            slotUIs[i] = Instantiate(slotPrefab, slotsParent);
            slotUIs[i].Init(this, i);
        }

        inventory.OnChanged += Refresh;
        Refresh();
        SetOpen(false);
    }

    void OnDestroy()
    {
        if (inventory != null) inventory.OnChanged -= Refresh;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            SetOpen(!isOpen);
    }

    void SetOpen(bool open)
    {
        isOpen = open;
        panel.SetActive(open);

        if (pauseGameWhenOpen)
            Time.timeScale = open ? 0f : 1f;

        if (unlockCursor)
        {
            Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = open;
        }

        if (open) Refresh();
    }

    void Refresh()
    {
        for (int i = 0; i < slotUIs.Length; i++)
            slotUIs[i].SetSlot(inventory.slots[i]);
    }

    // Kliknutí na slot – zatím odebere 1 kus (sem později přidej "použít" / "zahodit")
    public void OnSlotClicked(int index)
    {
        inventory.RemoveFromSlot(index, 1);
    }
}
