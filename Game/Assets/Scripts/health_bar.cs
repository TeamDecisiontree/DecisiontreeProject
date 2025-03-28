using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class health_bar : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject hpText;
    public float sliderValue;
    public int curHealth;
    public int maxHealth;
    float hpBarPercentage;

    public string healthText_testing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get starting health value for the player.
        maxHealth = player.GetComponent<player_stats_test>().health;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the health of the player.
        curHealth = player.GetComponent<player_stats_test>().health;

        //Get how much of a percentage of health is left.
        hpBarPercentage = (float)curHealth/maxHealth;

        //Change the Slider attribute "value".
        this.GetComponent<Slider>().value = hpBarPercentage;

        //Update text inside health bar
        hpText.GetComponent<TextMeshProUGUI>().text = curHealth + " / " + maxHealth;
    }
}
