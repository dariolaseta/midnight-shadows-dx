using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private RectTransform slotsContainer;
    [SerializeField] private float scrollSpeed = 25f;
    [SerializeField] private float scaleMultiplier = 1.5f;
    [SerializeField] private float scaleTransitionSpeed = 5f;
    
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
    }

    private void OnEnable()
    {
        slotsContainer.localPosition = initialContainerPosition;
        targetHorizontalPosition = initialContainerPosition.x;
        
        moveLeftAction.action.Enable();
        moveRightAction.action.Enable();
        moveLeftAction.action.performed += OnMoveLeft;
        moveRightAction.action.performed += OnMoveRight;

        InitializeSlots();
        
        initialContainerPosition = slotsContainer.localPosition;
        
        currentCenterIndex = Mathf.Clamp(_lastSelectedIndex, 0, Mathf.Max(0, inventory.Count - 1));
        targetHorizontalPosition = -currentCenterIndex * GetSlotWidth();
        slotsContainer.localPosition = new Vector3(targetHorizontalPosition, initialContainerPosition.y, initialContainerPosition.z);
    }
    
    private void OnDisable()
    {
        _lastSelectedIndex = currentCenterIndex;
        
        moveLeftAction.action.performed -= OnMoveLeft;
        moveRightAction.action.performed -= OnMoveRight;
        moveLeftAction.action.Disable();
        moveRightAction.action.Disable();
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

    public void ListItems()
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

    public void CleanContentItems()
    {
        foreach (Transform item in slotsContainer)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void AddItem(Items item)
    {
        inventory.Add(item);
        
        ObtainItemUI.Instance.ShowObtainingItemUI(item.ItemImage, item.ItemDescription);
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
}