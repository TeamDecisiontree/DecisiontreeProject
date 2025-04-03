using UnityEngine;

public class Heal : Skill {
    GameCharacter gc;

    public Heal(GameCharacter gc) : base(gc, "Heal", 1, 0, 0){
        this.gc = gc;
    }

    public override bool Effect(GameCharacter target){
        if (target != gc) {
            return false;
        }

        if(target.Mana < manaCost)
            return false;

        target.Mana -= manaCost;

        target.HP = target.HP + Mathf.FloorToInt(gc.Magic * power);

        if (target.HP > target.Vitality){
            target.HP = target.Vitality;
        }

        return true;
    }

}
