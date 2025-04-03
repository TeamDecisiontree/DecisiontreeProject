using System.Threading.Tasks;
using UnityEngine;

public class GameCharacter : MonoBehaviour
{

    public Combat c;

    // STATS
    int hp;
    int vitality;
    int armor;
    int strength;
    int magic;
    int mana;
    int maxMana;
    public HealthBar healthBar;

    public int HP { get { return hp; } set { this.hp = value; } }
    public int Vitality { get { return Mathf.RoundToInt((vitality + GetEquipmentVitalitySum()) * GetEquipmentVitalityMult()); } set { this.vitality = value; } }
    public int Armor { get { return Mathf.RoundToInt((armor + GetEquipmentArmorSum()) * GetEquipmentArmorMult()); } }
    public int Strength { get { return Mathf.RoundToInt((strength + GetEquipmentStrengthSum()) * GetEquipmentStrengthMult()); } set { this.strength = value; } }
    public int Magic { get { return Mathf.RoundToInt((magic + GetEquipmentMagicSum()) * GetEquipmentMagicMult()); } set { this.magic = value; } }
    public int Mana { get { return Mathf.RoundToInt((mana + GetEquipmentManaSum()) * GetEquipmentManaMult()); } set { this.mana = value; } }

    // SKILLS
    public Skill[] skills;
    int skillCount;
    int selectedSkill = 0;

    // INVENTORY
    public Equipment equipment;
    public Item[] inventory;

    // to change sprite
    SpriteManager spriteManager;
    Transform moveCharacterSprite;

    public GameCharacter()
    {

        c = null;
        hp = 100;
        vitality = 100;
        armor = 5;
        strength = 10;
        magic = 0;
        mana = 0;
        maxMana = 0;
        skills = new Skill[8];
        skillCount = 1;
        equipment = null;
        inventory = new Item[20];

    }

    public virtual void Init()
    {

        equipment = gameObject.GetComponent<Equipment>();
        skills[0] = new Punch(this);
        // skills[1] = new TestSkill(this);

    }

    public void SetSprite(string type)
    {

        spriteManager = GetComponentInChildren<SpriteManager>();
        if (spriteManager == null) return;
        spriteManager.SetCharacter(type);
        moveCharacterSprite = gameObject.transform.GetChild(0);
        moveCharacterSprite.localScale = new Vector3(3, 3, 3);

    }

    void OnMouseDown()
    {

        if (c != null)
            c.CharacterClicked(this);

    }

    void OnMouseEnter()
    {

        if (c != null)
            c.CharacterHover(this);

    }

    public bool UseSkill(GameCharacter target)
    {

        print(gameObject.name + " is using " + skills[selectedSkill].Name + " on " + target.gameObject.name);

        bool skill = skills[selectedSkill].Effect(target);

        if (spriteManager != null && skill)
            spriteManager.AttackAnimation();

        return skill;
    }

    public void SetSelectedSkill(int skillIndex)
    {
        if (skills[skillIndex] != null)
        {
            selectedSkill = skillIndex;
            Debug.Log($"GameCharacter selected skill: {skills[selectedSkill].Name}");
        }
    }
    public void TakeDamage(int dmg)
    {

        print($"Character armor: {Armor}");

        if (dmg <= Armor)
            return;

        hp -= dmg - Armor;
        print(gameObject.name + " took: " + dmg + " damage!");

        healthBar.UpdateHealthBar(hp, Vitality);

        if (hp <= 0)
            c.KillCharacter(this);
        else
            DamageEffect();

    }

    async Task DamageEffect()
    {

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        float time = 0.5f;

        while (time > 0)
        {

            sr.color = new Color(1, 1 - time, 1 - time);
            time -= Time.deltaTime;
            await Task.Yield();

        }

        sr.color = new Color(1, 1, 1);

    }

    public float GetEquipmentVitalityMult()
    {

        float factor = 1;
        if (equipment.head != null)
            factor *= equipment.head.VitalityMult;
        if (equipment.torso != null)
            factor *= equipment.torso.VitalityMult;
        if (equipment.boots != null)
            factor *= equipment.boots.VitalityMult;
        if (equipment.weaponLeft != null)
            factor *= equipment.weaponLeft.VitalityMult;
        return factor;

    }

    public int GetEquipmentVitalitySum()
    {

        int sum = 0;
        if (equipment.head != null)
            sum += equipment.head.VitalityAdd;
        if (equipment.torso != null)
            sum += equipment.torso.VitalityAdd;
        if (equipment.boots != null)
            sum += equipment.boots.VitalityAdd;
        if (equipment.weaponLeft != null)
            sum += equipment.weaponLeft.VitalityAdd;
        return sum;

    }

    public float GetEquipmentArmorMult()
    {

        float factor = 1;
        if (equipment.head != null)
            factor *= equipment.head.ArmorMult;
        if (equipment.torso != null)
            factor *= equipment.torso.ArmorMult;
        if (equipment.boots != null)
            factor *= equipment.boots.ArmorMult;
        if (equipment.weaponLeft != null)
            factor *= equipment.weaponLeft.ArmorMult;
        return factor;

    }

    public int GetEquipmentArmorSum()
    {

        int sum = 0;
        if (equipment.head != null)
            sum += equipment.head.ArmorAdd;
        if (equipment.torso != null)
            sum += equipment.torso.ArmorAdd;
        if (equipment.boots != null)
            sum += equipment.boots.ArmorAdd;
        if (equipment.weaponLeft != null)
            sum += equipment.weaponLeft.ArmorAdd;
        return sum;

    }

    public float GetEquipmentStrengthMult()
    {

        float factor = 1;
        if (equipment.head != null)
            factor *= equipment.head.StrengthMult;
        if (equipment.torso != null)
            factor *= equipment.torso.StrengthMult;
        if (equipment.boots != null)
            factor *= equipment.boots.StrengthMult;
        if (equipment.weaponLeft != null)
            factor *= equipment.weaponLeft.StrengthMult;
        return factor;

    }

    public int GetEquipmentStrengthSum()
    {

        int sum = 0;
        if (equipment.head != null)
            sum += equipment.head.StrengthAdd;
        if (equipment.torso != null)
            sum += equipment.torso.StrengthAdd;
        if (equipment.boots != null)
            sum += equipment.boots.StrengthAdd;
        if (equipment.weaponLeft != null)
            sum += equipment.weaponLeft.StrengthAdd;
        return sum;

    }

    public float GetEquipmentMagicMult()
    {

        float factor = 1;
        if (equipment.head != null)
            factor *= equipment.head.MagicMult;
        if (equipment.torso != null)
            factor *= equipment.torso.MagicMult;
        if (equipment.boots != null)
            factor *= equipment.boots.MagicMult;
        if (equipment.weaponLeft != null)
            factor *= equipment.weaponLeft.MagicMult;
        return factor;

    }

    public int GetEquipmentMagicSum()
    {

        int sum = 0;
        if (equipment.head != null)
            sum += equipment.head.MagicAdd;
        if (equipment.torso != null)
            sum += equipment.torso.MagicAdd;
        if (equipment.boots != null)
            sum += equipment.boots.MagicAdd;
        if (equipment.weaponLeft != null)
            sum += equipment.weaponLeft.MagicAdd;
        return sum;

    }

    public float GetEquipmentManaMult()
    {

        float factor = 1;
        if (equipment.head != null)
            factor *= equipment.head.ManaMult;
        if (equipment.torso != null)
            factor *= equipment.torso.ManaMult;
        if (equipment.boots != null)
            factor *= equipment.boots.ManaMult;
        if (equipment.weaponLeft != null)
            factor *= equipment.weaponLeft.ManaMult;
        return factor;

    }

    public int GetEquipmentManaSum()
    {

        int sum = 0;
        if (equipment.head != null)
            sum += equipment.head.ManaAdd;
        if (equipment.torso != null)
            sum += equipment.torso.ManaAdd;
        if (equipment.boots != null)
            sum += equipment.boots.ManaAdd;
        if (equipment.weaponLeft != null)
            sum += equipment.weaponLeft.ManaAdd;
        return sum;

    }

}