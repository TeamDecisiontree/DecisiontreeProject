using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameCharacter : MonoBehaviour{

    public Combat c;

    // STATS
    string cName;
    int hp;
    int vitality;
    int armor;
    int strength;
    int magic;
    int mana;
    int maxMana;
    public Bar healthBar;
    public Bar manaBar;

    public string CName{get { return cName; } set {this.cName = value; }}
    public int HP{get{ return hp; } set{ this.hp = value; }}
    public int Vitality{ get { return Mathf.RoundToInt((vitality + GetEquipmentVitalitySum()) * GetEquipmentVitalityMult()); } set{ this.vitality = value; }}
    public int Armor{ get { return Mathf.RoundToInt((armor + GetEquipmentArmorSum()) * GetEquipmentArmorMult()); } set { this.armor = value; }}
    public int Strength{get { return Mathf.RoundToInt((strength + GetEquipmentStrengthSum()) * GetEquipmentStrengthMult()); } set{ this.strength = value; }}
    public int Magic{get { return Mathf.RoundToInt((magic + GetEquipmentMagicSum()) * GetEquipmentMagicMult()); } set{ this.magic = value; }}
    public int Mana{get{ return Mathf.RoundToInt((mana + GetEquipmentManaSum()) * GetEquipmentManaMult()); } set{ this.mana = value; }}
    public int MaxMana{get{ return maxMana; } set {this.maxMana = value; }}

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
    public List<Sprite> sprites;
    private readonly float CHARACTER_SCALE = 2.4f;

    public GameCharacter(string cName, int vitality, int armor, int strength, int magic, int mana, int maxSkill, int inventorySize){

        this.cName = cName;
        c = null;
        hp = vitality;
        this.vitality = vitality;
        this.armor = armor;
        this.strength = strength;
        this.magic = magic;
        this.mana = mana;
        this.maxMana = mana;
        skills = new Skill[maxSkill];
        skillCount = 0;
        equipment = null;
        inventory = new Item[inventorySize];

    }

    public virtual void Init(){
        equipment = gameObject.GetComponent<Equipment>();
    }

    public void SetSprite(string type) {

        spriteManager = GetComponentInChildren<SpriteManager>();
        if(spriteManager == null) {
            Debug.Log("spriteManager Not found");
            return;
        }
        spriteManager.SetCharacter(this);
        moveCharacterSprite = gameObject.transform.GetChild(0);
        moveCharacterSprite.localScale = new Vector3(CHARACTER_SCALE,CHARACTER_SCALE,CHARACTER_SCALE);

    }

    void OnMouseDown(){

        if(c != null)
            c.CharacterClicked(this);

    }

    void OnMouseEnter(){
        
        if(c != null)
            c.CharacterHover(this);

    }

    public bool SelectSkill(int index){

        if(index < 0)
            return false;

        if(index > skillCount - 1)
            return false;

        if(skills[index].Cooldown > 0)
            return false;

        selectedSkill = index;
        return true;

    }

    public bool UseSkill(GameCharacter target){

        bool skill = target != null && skills[selectedSkill].Effect(target);
        healthBar.UpdateBar(HP, Vitality);

        Vector3 posOfTarget = target.transform.GetChild(0).position;
        if (spriteManager != null && skill) {
            Debug.Log(gameObject.name);
            spriteManager.AttackAnimation(gameObject.name, this);
            spriteManager.AbilityAnimation(posOfTarget, this, selectedSkill, 5);
        }

        return skill;

    }
    
    public void TakeDamage(int dmg){

        print($"{cName} is attacked with {dmg} damage and has {Armor} armor");

        if(dmg <= Armor)
            return;

        hp -= dmg - Armor;

        healthBar.UpdateBar(hp, Vitality);

        if(hp <= 0)
            c.KillCharacter(this);
        else
            DamageEffect();

    }

    async Task DamageEffect(){

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        float time = 0.5f;
        
        while(time > 0){

            sr.color = new Color(1,1-time,1-time);
            time -= Time.deltaTime;
            await Task.Yield();

        }
            
        sr.color = new Color(1,1,1);

    }

    public void AddSkill(Skill newSkill) {
        if (skillCount == skills.Length) {
            Debug.Log("Not enough slots!!");
            return;
        }

        skills[skillCount] = newSkill;
        skillCount++;
    }

    public float GetEquipmentVitalityMult(){

        float factor = 1;
        if(equipment.head != null)
            factor *= equipment.head.VitalityMult;
        if(equipment.torso != null)
            factor *= equipment.torso.VitalityMult;
        if(equipment.boots != null)
            factor *= equipment.boots.VitalityMult;
        if(equipment.weaponLeft != null)
            factor *= equipment.weaponLeft.VitalityMult;
        return factor;

    }

    public int GetEquipmentVitalitySum(){

        int sum = 0;
        if(equipment.head != null)
            sum += equipment.head.VitalityAdd;
        if(equipment.torso != null)
            sum += equipment.torso.VitalityAdd;
        if(equipment.boots != null)
            sum += equipment.boots.VitalityAdd;
        if(equipment.weaponLeft != null)
            sum += equipment.weaponLeft.VitalityAdd;
        return sum;

    }

    public float GetEquipmentArmorMult(){

        float factor = 1;
        if(equipment.head != null)
            factor *= equipment.head.ArmorMult;
        if(equipment.torso != null)
            factor *= equipment.torso.ArmorMult;
        if(equipment.boots != null)
            factor *= equipment.boots.ArmorMult;
        if(equipment.weaponLeft != null)
            factor *= equipment.weaponLeft.ArmorMult;
        return factor;

    }

    public int GetEquipmentArmorSum(){

        int sum = 0;
        if(equipment.head != null)
            sum += equipment.head.ArmorAdd;
        if(equipment.torso != null)
            sum += equipment.torso.ArmorAdd;
        if(equipment.boots != null)
            sum += equipment.boots.ArmorAdd;
        if(equipment.weaponLeft != null)
            sum += equipment.weaponLeft.ArmorAdd;
        return sum;

    }

    public float GetEquipmentStrengthMult(){

        float factor = 1;
        if(equipment.head != null)
            factor *= equipment.head.StrengthMult;
        if(equipment.torso != null)
            factor *= equipment.torso.StrengthMult;
        if(equipment.boots != null)
            factor *= equipment.boots.StrengthMult;
        if(equipment.weaponLeft != null)
            factor *= equipment.weaponLeft.StrengthMult;
        return factor;

    }

    public int GetEquipmentStrengthSum(){

        int sum = 0;
        if(equipment.head != null)
            sum += equipment.head.StrengthAdd;
        if(equipment.torso != null)
            sum += equipment.torso.StrengthAdd;
        if(equipment.boots != null)
            sum += equipment.boots.StrengthAdd;
        if(equipment.weaponLeft != null)
            sum += equipment.weaponLeft.StrengthAdd;
        return sum;

    }

    public float GetEquipmentMagicMult(){

        float factor = 1;
        if(equipment.head != null)
            factor *= equipment.head.MagicMult;
        if(equipment.torso != null)
            factor *= equipment.torso.MagicMult;
        if(equipment.boots != null)
            factor *= equipment.boots.MagicMult;
        if(equipment.weaponLeft != null)
            factor *= equipment.weaponLeft.MagicMult;
        return factor;

    }

    public int GetEquipmentMagicSum(){

        int sum = 0;
        if(equipment.head != null)
            sum += equipment.head.MagicAdd;
        if(equipment.torso != null)
            sum += equipment.torso.MagicAdd;
        if(equipment.boots != null)
            sum += equipment.boots.MagicAdd;
        if(equipment.weaponLeft != null)
            sum += equipment.weaponLeft.MagicAdd;
        return sum;

    }

    public float GetEquipmentManaMult(){

        float factor = 1;
        if(equipment.head != null)
            factor *= equipment.head.ManaMult;
        if(equipment.torso != null)
            factor *= equipment.torso.ManaMult;
        if(equipment.boots != null)
            factor *= equipment.boots.ManaMult;
        if(equipment.weaponLeft != null)
            factor *= equipment.weaponLeft.ManaMult;
        return factor;

    }

    public int GetEquipmentManaSum(){

        int sum = 0;
        if(equipment.head != null)
            sum += equipment.head.ManaAdd;
        if(equipment.torso != null)
            sum += equipment.torso.ManaAdd;
        if(equipment.boots != null)
            sum += equipment.boots.ManaAdd;
        if(equipment.weaponLeft != null)
            sum += equipment.weaponLeft.ManaAdd;
        return sum;

    }
}