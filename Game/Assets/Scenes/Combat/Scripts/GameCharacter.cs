using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameCharacter : MonoBehaviour{

    public Combat c;
    public RectTransform bars;

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
    
    public int Vitality{ get {
        return 
            Mathf.RoundToInt(
                Mathf.RoundToInt( // VITALITY CALCULATED
                    (vitality + GetEquipmentSum(0)) * GetEquipmentMult(0)
                ) // EFFECTS APPLIED
                - GetEffectSum(0) * GetEffectFactor(0)
            );
    } set{ this.vitality = value; }}

    public int Armor{ get {
         return 
            Mathf.RoundToInt(
                Mathf.RoundToInt( // ARMOR CALCULATED
                    (armor + GetEquipmentSum(1)) * GetEquipmentMult(1)
                ) // EFFECTS APPLIED
                - GetEffectSum(1) * GetEffectFactor(1)
            );
    } set { this.armor = value; }}

    public int Strength{get {
        return 
            Mathf.RoundToInt(
                Mathf.RoundToInt( // STRENGTH CALCULATED
                    (strength + GetEquipmentSum(2)) * GetEquipmentMult(2)
                ) // EFFECTS APPLIED
                - GetEffectSum(2) * GetEffectFactor(2)
            );
    } set{ this.strength = value; }}

    public int Magic{get {
         return 
            Mathf.RoundToInt(
                Mathf.RoundToInt( // MAGIC CALCULATED
                    (magic + GetEquipmentSum(3)) * GetEquipmentMult(3)
                ) // EFFECTS APPLIED
                - GetEffectSum(3) * GetEffectFactor(3)
            );
    } set{ this.magic = value; }}

    public int Mana{get{ return mana; } set{ this.mana = value; }}
    
    public int MaxMana{get{
         return 
            Mathf.RoundToInt(
                Mathf.RoundToInt( // MANA CALCULATED
                    (maxMana + GetEquipmentSum(4)) * GetEquipmentMult(4)
                ) // EFFECTS APPLIED
                - GetEffectSum(4) * GetEffectFactor(4)
            );
    } set {this.maxMana = value; }}

    // SKILLS
    public Skill[] skills;
    public List<Skill> unlockedSkills = new List<Skill>();
    int skillCount;
    public int SkillCount{ get{ return skillCount; }}
    int selectedSkill = 0;

    // INVENTORY
    public Equipment equipment;
    public Item[] inventory;

    // STATUS EFFECT
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    // to change sprite
    SpriteManager spriteManager;
    public SpriteManager SM{ get { return spriteManager; }}
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
        StartCoroutine(FixBars());

    }

    public virtual bool IsPlayer()
    {
        return false; 
    }
    
    public IEnumerator FixBars(){
        
        Moved();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FixBars());

    }

    public void SetSprite() {

        spriteManager = GetComponentInChildren<SpriteManager>();
        if(spriteManager == null) {
            Debug.Log("spriteManager Not found");
            return;
        }
        spriteManager.SetCharacter(this);
        moveCharacterSprite = gameObject.transform.GetChild(0);
        moveCharacterSprite.localScale = new Vector3(CHARACTER_SCALE,CHARACTER_SCALE,CHARACTER_SCALE);
    }

    public void Moved(){

        if(bars == null)
            return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        RectTransform canvasRect = GameObject.FindGameObjectWithTag("Canvas")?.GetComponent<RectTransform>();
        Vector2 localPos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out localPos)) {

            bars.localPosition = localPos;

        }

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

        if(skills[index].cooldownCount > 0)
            return false;

        selectedSkill = index;
        return true;

    }

    public bool UseSkill(GameCharacter target){

        bool skill = skills[selectedSkill].TrySkill(target);
        healthBar.UpdateBar(HP, Vitality, 0);
        manaBar.UpdateBar(mana, MaxMana, 1);

        Vector3 posOfTarget = target.transform.GetChild(0).position;
        if (spriteManager != null && skill) {
            foreach(Skill s in skills){
                if(s is Shield){
                    s.SkillAnimation(posOfTarget, this, spriteManager);
                }
            }
            
            if(!(skills[selectedSkill] is Shield)){
                skills[selectedSkill].SkillAnimation(posOfTarget, this, spriteManager);
            }   
        }
        this.transform.GetChild(0).GetChild(6).transform.localPosition = Vector3.zero;

        return skill;

    }
    
    public void TakeDamage(int dmg){

        if(dmg <= Armor)
            return;

        hp -= dmg - Armor;

        healthBar.UpdateBar(hp, Vitality, 0);

        if(hp <= 0){
            c.PlayDeathSound(this);
            c.KillCharacter(this);
        } else {
            DamageEffect();
        }

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

    public void RemoveSkillAt(int index)
    {
        if (index < 0 || index >= skillCount)
            return;

        for (int i = index; i < skillCount - 1; i++)
        {
            skills[i] = skills[i + 1];
        }

        unlockedSkills.RemoveAt(index);
        skills[skillCount - 1] = null;
        skillCount--;
    }

    //Update the player stats (permanently).
    public void UpdateStats(int vitDelta, int strDelta, int magDelta){
        vitality += vitDelta;
        strength += strDelta;
        magic += magDelta;
    }

    public int[] GetBaseStats(){

        int[] temp = {
            vitality,
            armor,
            strength,
            magic,
            maxMana
        };

        return temp;
    }

    int GetEffectSum(int type){

        int sum = 0;

        for(int i = 0; i < statusEffects.Count; i++){

            if(statusEffects[i].EffectType == type)
                sum += statusEffects[i].Delta;

        }

        return sum;

    }

    float GetEffectFactor(int type){

        float factor = 1;

        for(int i = 0; i < statusEffects.Count; i++){

            if(statusEffects[i].EffectType == type)
                factor *= statusEffects[i].DeltaF;

        }

        return factor;

    }

    public float GetEquipmentMult(int type){

        float factor = 1;

        if(equipment.head != null){
            switch(type){
                case 0:
                    factor *= equipment.head.VitalityMult;
                    break;
                case 1:
                    factor *= equipment.head.ArmorMult;
                    break;
                case 2:
                    factor *= equipment.head.StrengthMult;
                    break;
                case 3:
                    factor *= equipment.head.MagicMult;
                    break;
                case 4:
                    factor *= equipment.head.ManaMult;
                    break;
            }
        }

        if(equipment.torso != null){
            switch(type){
                case 0:
                    factor *= equipment.torso.VitalityMult;
                    break;
                case 1:
                    factor *= equipment.torso.ArmorMult;
                    break;
                case 2:
                    factor *= equipment.torso.StrengthMult;
                    break;
                case 3:
                    factor *= equipment.torso.MagicMult;
                    break;
                case 4:
                    factor *= equipment.torso.ManaMult;
                    break;
            }
        }

        if(equipment.boots != null){
            switch(type){
                case 0:
                    factor *= equipment.boots.VitalityMult;
                    break;
                case 1:
                    factor *= equipment.boots.ArmorMult;
                    break;
                case 2:
                    factor *= equipment.boots.StrengthMult;
                    break;
                case 3:
                    factor *= equipment.boots.MagicMult;
                    break;
                case 4:
                    factor *= equipment.boots.ManaMult;
                    break;
            }
        }

        if(equipment.weaponLeft != null){
            switch(type){
                case 0:
                    factor *= equipment.weaponLeft.VitalityMult;
                    break;
                case 1:
                    factor *= equipment.weaponLeft.ArmorMult;
                    break;
                case 2:
                    factor *= equipment.weaponLeft.StrengthMult;
                    break;
                case 3:
                    factor *= equipment.weaponLeft.MagicMult;
                    break;
                case 4:
                    factor *= equipment.weaponLeft.ManaMult;
                    break;
            }
        }

        if(equipment.weaponRight != null){
                switch(type){
                case 0:
                    factor *= equipment.head.VitalityMult;
                    break;
                case 1:
                    factor *= equipment.head.ArmorMult;
                    break;
                case 2:
                    factor *= equipment.head.StrengthMult;
                    break;
                case 3:
                    factor *= equipment.head.MagicMult;
                    break;
                case 4:
                    factor *= equipment.head.ManaMult;
                    break;
            }
        }

        return factor;

    }

    public int GetEquipmentSum(int type){

        int sum = 1;

        if(equipment.head != null){
            switch(type){
                case 0:
                    sum += equipment.head.VitalityAdd;
                    break;
                case 1:
                    sum += equipment.head.ArmorAdd;
                    break;
                case 2:
                    sum += equipment.head.StrengthAdd;
                    break;
                case 3:
                    sum += equipment.head.MagicAdd;
                    break;
                case 4:
                    sum += equipment.head.ManaAdd;
                    break;
            }
        }

        if(equipment.torso != null){
            switch(type){
                case 0:
                    sum += equipment.torso.VitalityAdd;
                    break;
                case 1:
                    sum += equipment.torso.ArmorAdd;
                    break;
                case 2:
                    sum += equipment.torso.StrengthAdd;
                    break;
                case 3:
                    sum += equipment.torso.MagicAdd;
                    break;
                case 4:
                    sum += equipment.torso.ManaAdd;
                    break;
            }
        }

        if(equipment.boots != null){
            switch(type){
                case 0:
                    sum += equipment.boots.VitalityAdd;
                    break;
                case 1:
                    sum += equipment.boots.ArmorAdd;
                    break;
                case 2:
                    sum += equipment.boots.StrengthAdd;
                    break;
                case 3:
                    sum += equipment.boots.MagicAdd;
                    break;
                case 4:
                    sum += equipment.boots.ManaAdd;
                    break;
            }
        }

        if(equipment.weaponLeft != null){
            switch(type){
                case 0:
                    sum += equipment.weaponLeft.VitalityAdd;
                    break;
                case 1:
                    sum += equipment.weaponLeft.ArmorAdd;
                    break;
                case 2:
                    sum += equipment.weaponLeft.StrengthAdd;
                    break;
                case 3:
                    sum += equipment.weaponLeft.MagicAdd;
                    break;
                case 4:
                    sum += equipment.weaponLeft.ManaAdd;
                    break;
            }
        }

        if(equipment.weaponRight != null){
                switch(type){
                case 0:
                    sum += equipment.head.VitalityAdd;
                    break;
                case 1:
                    sum += equipment.head.ArmorAdd;
                    break;
                case 2:
                    sum += equipment.head.StrengthAdd;
                    break;
                case 3:
                    sum += equipment.head.MagicAdd;
                    break;
                case 4:
                    sum += equipment.head.ManaAdd;
                    break;
            }
        }

        return sum;

    }

}