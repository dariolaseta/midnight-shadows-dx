using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private RectTransform slotsContainer;
    [SerializeField] private float scrollSpeed = 25f;
    [SerializeField] private float scaleMultiplier = 1.5f;
    [SerializeField] private float scaleTransitionSpeed = 5f;
    [SerializeField] private GameObject inventoryUI;
    
    [Header("Text")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    
    [Header("Inventory")]
    [SerializeField] private List<Items> inventory = new List<Items>();
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] int poolSize = 20;
    
    [Header("Input")]
    [SerializeField] private InputActionReference moveLeftAction;
    [SerializeField] private InputActionReference moveRightAction;
    [SerializeField] private InputActionReference selectAction;
    [SerializeField] private InputActionReference openInventoryAction;

    [Header("Audio")] 
    [SerializeField] private AudioClip scrollSound;
    [SerializeField] private AudioClip openInventorySound;
    [SerializeField] private AudioClip closeInventorySound;

    private Items pendingItemCheck;
    private Action<bool> itemCheckCallback;
    
    private bool isInventoryOpen = false;

    private List<RectTransform> slots = new List<RectTransform>();
    private float targetHorizontalPosition;
    private int currentCenterIndex;
    private Vector3 initialContainerPosition;
    
    private List<GameObject> itemPool = new List<GameObject>();

    private static int _lastSelectedIndex = 0;
    
    private void Awake()
    {
        GenerateItemPool();
        
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        
        openInventoryAction.action.Enable();
        openInventoryAction.action.performed += OpenInventory;
    }

    private void OnDestroy()
    {
        openInventoryAction.action.performed -= OpenInventory;
        openInventoryAction.action.Disable();
        UnsubscribeFromEvents();
    }

    private void OnEnableInventory()
    {
        inventoryUI.SetActive(true);
        
        SubscribeToEvents();
        
        slotsContainer.localPosition = initialContainerPosition;
        targetHorizontalPosition = initialContainerPosition.x;

        InitializeSlots();
        
        initialContainerPosition = slotsContainer.localPosition;
        
        currentCenterIndex = Mathf.Clamp(_lastSelectedIndex, 0, Mathf.Max(0, inventory.Count - 1));
        targetHorizontalPosition = -currentCenterIndex * GetSlotWidth();
        slotsContainer.localPosition = new Vector3(targetHorizontalPosition, initialContainerPosition.y, initialContainerPosition.z);
        
        ListItems();
    }

    private void OpenInventory(InputAction.CallbackContext obj)
    {
        if (!Flags.Instance.IsFlagTrue("hasBackpack")) return;

        if (ShouldOpenInventory())
        {
            OpenInventoryInternal();
        } else if (ShouldCloseInventory())
        {
            CloseInventoryInternal();
        }
    }

    private bool ShouldOpenInventory()
    {
        return !isInventoryOpen &&
               GameController.Instance.State == GameState.FREEROAM;
    }

    private bool ShouldCloseInventory()
    {
        return isInventoryOpen &&
               GameController.Instance.State == GameState.INVENTORY;
    }

    private void OpenInventoryInternal()
    {
        AudioManager.Instance.PlaySfx(openInventorySound);
        GameController.Instance.ChangeState(GameState.INVENTORY);
        isInventoryOpen = true;
        OnEnableInventory();
    }

    private void CloseInventoryInternal()
    {
        AudioManager.Instance.PlaySfx(closeInventorySound);
        isInventoryOpen = false;
        OnDisableInventory();
        GameController.Instance.GoToPrevState();
    }

    private void OnSelect(InputAction.CallbackContext obj)
    {
        if (GameController.Instance.State != GameState.USE_ITEM)
        {
            GameController.Instance.GoToPrevState();
            
            DialogueSystem.Instance.SetDialogue("Non mi serve ora");
            StartCoroutine(DialogueSystem.Instance.ShowDialogue());

            CloseInventoryInternal();
            
            return;
        }

        if (inventory.Count == 0)
        {
            DialogueSystem.Instance.SetDialogue("Non ho oggetti nello zaino");
            StartCoroutine(DialogueSystem.Instance.ShowDialogue());
            return;
        }
        
        Items selectedItem = inventory[currentCenterIndex];
        bool isCorrectItem = selectedItem == pendingItemCheck;

        if (isCorrectItem)
        {
            RemoveItem(selectedItem);
            ListItems();
            UpdateNameAndDescriptionText(currentCenterIndex);
        }
        
        itemCheckCallback?.Invoke(isCorrectItem);
        ResetItemCheck();
    }

    private void ResetItemCheck()
    {
        pendingItemCheck = null;
        itemCheckCallback = null;
        GameController.Instance.GoToPrevState();
        CloseInventoryInternal();
    }

    private void SubscribeToEvents()
    {
        moveLeftAction.action.Enable();
        moveRightAction.action.Enable();
        selectAction.action.Enable();
        moveLeftAction.action.performed += OnMoveLeft;
        moveRightAction.action.performed += OnMoveRight;
        selectAction.action.performed += OnSelect;
    }

    private void UnsubscribeFromEvents()
    {
        moveLeftAction.action.performed -= OnMoveLeft;
        moveRightAction.action.performed -= OnMoveRight;
        selectAction.action.performed -= OnSelect;
        moveLeftAction.action.Disable();
        moveRightAction.action.Disable();
        selectAction.action.Disable();
    }

    private void OnDisableInventory()
    {
        inventoryUI.SetActive(false);
        
        UnsubscribeFromEvents();
        
        _lastSelectedIndex = currentCenterIndex;
        
        CleanContentItems();
    }

    private void InitializeSlots()
    {
        slots.Clear();
        foreach (Transform child in slotsContainer)
        {
            slots.Add(child.GetComponent<RectTransform>());
            //child.localScale = Vector3.one;
        }
        
        UpdateNameAndDescriptionText(currentCenterIndex);
    }
    
    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        Scroll(-1);
    }

    private void OnMoveRight(InputAction.CallbackContext context)
    {
        Scroll(1);
    }

    private void Update()
    {
        SmoothScroll();
        UpdateSlotsScale();
    }

    private void Scroll(int direction)
    {
        if ((direction > 0 && currentCenterIndex != inventory.Count - 1) || (direction < 0 && currentCenterIndex != 0))
        {
            AudioManager.Instance.PlaySfx(scrollSound);
        }
        
        currentCenterIndex = Mathf.Clamp(currentCenterIndex + direction, 0, inventory.Count - 1);
        targetHorizontalPosition = -currentCenterIndex * GetSlotWidth();
        
        UpdateNameAndDescriptionText(currentCenterIndex);
    }

    private void UpdateNameAndDescriptionText(int currentItemIndex)
    {
        itemNameText.text = inventory[currentItemIndex].ItemName;
        itemDescriptionText.text = inventory[currentItemIndex].ItemDescription;
    }

    private float GetSlotWidth()
    {
        return slots[0].rect.width + slotsContainer.GetComponent<HorizontalLayoutGroup>().spacing;
    }

    private void SmoothScroll()
    {
        Vector3 currentPos = slotsContainer.localPosition;
        float newX = Mathf.Lerp(currentPos.x, targetHorizontalPosition, Time.deltaTime * scrollSpeed);
        slotsContainer.localPosition = new Vector3(newX, initialContainerPosition.y, initialContainerPosition.z);
    }

    private void UpdateSlotsScale()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            float distance = Mathf.Abs(i - currentCenterIndex);
            float targetScale = Mathf.Clamp(scaleMultiplier - distance * 0.3f, 0.5f, scaleMultiplier);
            
            slots[i].localScale = Vector3.Lerp(
                slots[i].localScale,
                Vector3.one * targetScale,
                Time.deltaTime * scaleTransitionSpeed
            );
        }
    }

    private void GenerateItemPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(slotPrefab, slotsContainer);
            obj.SetActive(false);
            itemPool.Add(obj);
        }
    }

    private void ListItems()
    {
        foreach (GameObject obj in itemPool)
        {
            obj.SetActive(false);
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            GameObject obj;

            if (i < itemPool.Count)
            {
                obj = itemPool[i];
            }
            else
            {
                obj = Instantiate(slotPrefab, slotsContainer);
                itemPool.Add(obj);
            }
            
            obj.SetActive(true);
            
            var itemSprite = obj.transform.Find("ItemSprite").GetComponent<Image>();
            itemSprite.sprite = inventory[i].ItemImage;
        }
    }

    private void CleanContentItems()
    {
        foreach (Transform item in slotsContainer)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void AddItem(Items item, bool showObtainUI = true)
    {
        inventory.Add(item);

        if (showObtainUI)
        {
            ObtainItemUI.Instance.ShowObtainingItemUI(item.ItemImage, item.ItemDescription);
        }
    }

    public void RemoveItem(Items item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
        }
        else
        {
            Debug.Log("Oggetto non presente nell'inventario."); //TODO REMOVE
        }
    }

    public void StartUseItem(Items requiredItem, Action<bool> callback)
    {
        pendingItemCheck = requiredItem;
        itemCheckCallback = callback;
        GameController.Instance.ChangeState(GameState.USE_ITEM);
        OnEnableInventory();
    }
}