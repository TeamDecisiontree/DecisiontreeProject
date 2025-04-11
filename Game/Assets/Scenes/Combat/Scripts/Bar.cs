using TMPro;     
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Transform target;
    public float yOffset = 2f;

    public Slider slider;               
    public TMP_Text valueText;         

    public void Init()
    {
        if (target == null)
        {
            Debug.LogError($"{GetType().Name}: No target assigned.");
            enabled = false;
            return; 
        }

        slider = GetComponent<Slider>();               
        valueText = GetComponentInChildren<TMP_Text>();
    }

    public void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + Vector3.up * yOffset);
            transform.position = screenPos;
        }
    }

    public void UpdateBar(int current, int max)
    {
        if (slider == null || valueText == null || max == 0) return;

        current = Mathf.Clamp(current, 0, max); // To keep current between 0 and the max value

        float percentage = (float)current / max;
        slider.value = percentage;
        valueText.text = $"{current} / {max}";
    }
}