using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    [Header("UI")]
    public GameObject itemButtonPrefabs;
    //public Transform itemsParent;
    public GameObject detailPanel;
    public TextMeshProUGUI GoldText;
    public Button buyButton;
    public Transform content;
    ItemButton itemButton;

    // Data
    int playerGold = 2000;
    private List<ShopItem> allItems = new List<ShopItem>();
    private ShopItem selectedItem;

    void Start()
    {
        Init();
    }

    void Init()
    {
        TestItems();
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyButtonClick);
        for (int i = 0; i < allItems.Count; i++)
        {
            itemButton = Instantiate(itemButtonPrefabs, content).GetComponent<ItemButton>();
            itemButton.Init(allItems[i], this);
        }
        ShowDetailPanel(false, null);
        UpdateGoldText();
    }

    void TestItems()
    {
        allItems.Add(new ShopItem(
            "Sword",
            450,
            "Attack: +15",
            "This is a sword."
        ));

        allItems.Add(new ShopItem(
            "Armor",
            300,
            "Armor: +20",
            "This is armor"
        ));

    }

    void TryBuyItem()
    {
        Debug.Log("购买按钮被点击！");
        int price = itemButton.currentItem.price;
        if(price == null) return;
        
        if(playerGold >= price)
        {
            playerGold -= price;
            itemButton.ButtonClose();
            UpdateGoldText();
        }
    }

    void UpdateGoldText()
    {
        GoldText.text = "Gold: " + playerGold;
    }

    public void ShowDetailPanel(bool show, ItemButton btn)
    {
        detailPanel.SetActive(show);
        itemButton = btn;
    }

    public void OnBuyButtonClick()
    {
        TryBuyItem();
        UpdateGoldText();
    }

}
public class ShopItem
{
    public string itemName;
    public int price;
    public string stats;
    public string story;

    public ShopItem(string name, int price, string stats, string story)
    {
        this.itemName = name;
        this.price = price;
        this.stats = stats;
        this.story = story;
    }
}