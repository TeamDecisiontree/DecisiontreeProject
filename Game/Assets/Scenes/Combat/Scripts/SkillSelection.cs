using UnityEngine;
using UnityEngine.UI;

public class SkillSelection : MonoBehaviour
{
    public Button[] skillButtons = new Button[8]; // Array for 8 skill buttons
    private Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        for (int i = 0; i < skillButtons.Length; i++)
        {
            int skillIndex = i; // Capture index for lambda
            skillButtons[i].onClick.AddListener(() => SelectSkill(skillIndex));
        }

        UpdateSkillButtons();
    }

    public void UpdateSkillButtons()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].interactable = player.skills.Length > i && player.skills[i] != null;
        }
    }

    public void SelectSkill(int skillIndex)
    {
        if (player.skills == null || player.skills.Length <= skillIndex || player.skills[skillIndex] == null)
        {
            Debug.LogWarning($"Skill selection failed: Index {skillIndex} is out of bounds or skill is null.");
            return;
        }

        player.SetSelectedSkill(skillIndex);
        Debug.Log($"Selected skill: {player.skills[skillIndex]?.Name}");
    }
}


/*using UnityEngine;
using UnityEngine.UI;

public class SkillSelection : MonoBehaviour
{
    public Button skill1Button;
    public Button skill2Button;

    private Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        skill1Button.onClick.AddListener(() => SelectSkill(0)); // Default skill
        skill2Button.onClick.AddListener(() => SelectSkill(1)); // Additional skill

        Debug.Log($"Skill 1: {player.skills[0]?.Name}, Skill 2: {(player.skills.Length > 1 ? player.skills[1]?.Name : "None")}");

        UpdateSkillButtons();
    }

    public void UpdateSkillButtons()
    {
        skill1Button.interactable = player.skills[0] != null;
        skill2Button.interactable = player.skills.Length > 1 && player.skills[1] != null;
    }

    public void SelectSkill(int skillIndex)
    {
        if (player.skills == null || player.skills.Length <= skillIndex || player.skills[skillIndex] == null)
        {
            Debug.LogWarning($"Skill selection failed: Index {skillIndex} is out of bounds or skill is null.");
            return;
        }

        player.SetSelectedSkill(skillIndex);  // This updates the selectedSkill in GameCharacter
        Debug.Log($"Selected skill: {player.skills[skillIndex]?.Name}");
    }
}*/