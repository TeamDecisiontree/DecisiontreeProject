using UnityEngine;
using UnityEngine.UI;

public class AbilityClickButton : MonoBehaviour
{
    public Button yourButton;
    public int selectedDamage = 10;

    public static int selectedDamageGlobal = 10;  // Static variable to hold damage globally

    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(() => SelectDamage(selectedDamage));
    }

    // Set the damage when the button is clicked
    public void SelectDamage(int damage)
    {
        selectedDamageGlobal = damage;
        Debug.Log("Selected Damage: " + selectedDamageGlobal);
    }
}
