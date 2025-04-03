using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemButton : MonoBehaviour
{
    Shop a;

    //
    private TextMeshProUGUI detailPanel;
    // item information
    public ShopItem currentItem;
    private TextMeshProUGUI priceText;
    // private Image iconImage;
    // private string nameText;
    // private int price;
    // private string statsText;
    // private string storyText;

    void Awake()
    {
        detailPanel = GameObject.Find("ItemDetail")?.GetComponent<TextMeshProUGUI>();
        priceText = transform.Find("PriceText")?.GetComponent<TextMeshProUGUI>();
        if (priceText == null) Debug.LogWarning("PriceText not found in ItemButton prefab!");
    }

    public void Init(ShopItem item, Shop a)
    {
        // för att kunna använda funktion från Shop
        this.a = a;

        currentItem = item;
        priceText.text = item.price.ToString();

        /* price = item.price;
        nameText = item.itemName;
        statsText = item.stats;
        storyText = item.story; */

        // iconImage.sprite = ...

        GetComponent<Button>().onClick.AddListener(OnItemClicked);
    }

    void DisplayItemDetail()
    {
        detailPanel.text = currentItem.itemName + "\n" + currentItem.stats + "\n" + currentItem.story;
    }

    public void ButtonClose()
    {
        this.GetComponent<Button>().interactable = false; 
    }
    private void OnItemClicked()
    {
        DisplayItemDetail();
        a.ShowDetailPanel(true,this);
    } 

}
