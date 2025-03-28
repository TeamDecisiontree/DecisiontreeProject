using System.Threading.Tasks;
using UnityEngine;

public class GameCharacter : MonoBehaviour{

    Combat c;

    // STATS
    public int HP = 100;
    int vitality = 100;
    int armor = 5;
    int strength = 10;
    int magic = 1;
    int mana = 100;
    public int Strength{get { return strength; }}
    public int Magic{get { return magic; }}
    public int Mana{get{ return mana; } set{ this.mana = value; }}

    // TODO should be deprecated
    int dmg = 15;
    public int DMG{get{ return dmg; }}

    // SKILLS
    public Skill[] skills = new Skill[8];
    int skillCount = 1;
    int selectedSkill = 0;

    // INVENTORY
    Equipment equipment;
    Item[] inventory = new Item[20];

    public void Init(Combat c){

        this.c = c;
        skills[0] = new Punch(this, 20, 100);
        equipment = gameObject.AddComponent<Equipment>();

    }

    void OnMouseDown(){

        c.CharacterClicked(this);

    }

    public bool UseSkill(GameCharacter target){

        return skills[selectedSkill].Effect(target);

    }

    public float GetEquipmentStrengthMult(){

        // (equipment.head as Head)
        return 0;

    }

    public int GetEquipmentStrengthSum(){

        return 0;

    }

    public void TakeDamage(int dmg){

        if(dmg <= armor)
            return;

        HP -= dmg - armor;
        DamageEffect();

        if(HP <= 0)
            c.KillCharacter(this);

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

}