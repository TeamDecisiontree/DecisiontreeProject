using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Combat : MonoBehaviour{

    public GameObject characterPrefab;
    public GameObject playerPrefab;
    public GameObject healthBarPrefab;
    public GameObject marker;
    Player player;
    List<GameCharacter> enemies = new List<GameCharacter>();

    int turn = 0;
    GameCharacter currentC;

    public void Init(){

        marker = Instantiate(marker);

        player = GameObject.Find("Player").GetComponent<Player>(); //horrible way of doing this
        player.ShowPlayer();
        player.c = this;

    }

    void Awake(){

        Init();

    }

    void Start(){ // TEMP

        //Add a healthbar for the player and put it inside the canvas.
        Vector3 healthBarPosition = Camera.main.WorldToScreenPoint(player.gameObject.transform.position + Vector3.up*2);
        player.healthBar = Instantiate(healthBarPrefab, healthBarPosition, Quaternion.identity, GameObject.Find("Canvas").transform).GetComponent<HealthBar>();
        player.healthBar.Init();
        player.healthBar.gameObject.name = "PlayerHP";
        player.healthBar.UpdateHealthBar(player.HP, player.Vitality);

        for(int i = 0; i < 3; i++){

            enemies.Add(Instantiate(characterPrefab).GetComponent<GameCharacter>());
            enemies[i].Init();
            enemies[i].c = this;
            enemies[i].gameObject.name = "Enemy #" + i;
            enemies[i].transform.position = Vector3.right * (i+1) * 2;

            //Add a healthbar for the enemy and put it inside the canvas.
            Vector3 enemyHealthBarPosition = Camera.main.WorldToScreenPoint(enemies[i].gameObject.transform.position + Vector3.up*2);   //Place healthbar above character.
            enemies[i].healthBar = Instantiate(healthBarPrefab, enemyHealthBarPosition, Quaternion.identity, GameObject.Find("Canvas").transform).GetComponent<HealthBar>();
            enemies[i].healthBar.Init();
            enemies[i].healthBar.gameObject.name = enemies[i].gameObject.name + " HP";
            enemies[i].healthBar.UpdateHealthBar(enemies[i].HP, enemies[i].Vitality);

        }

    }

    public GameCharacter GetCurrentCharacter(){

        GameCharacter current = null;

        if(turn == 0)
            current = player;
        else
            current = enemies[turn - 1];

        // move marker aswell
        marker.transform.position = current.gameObject.transform.position;
        return current;

    }

    public async Task KillCharacter(GameCharacter target){

        SpriteRenderer sr = target.gameObject.GetComponentInChildren<SpriteRenderer>();
        float time = 1;

        if(enemies.Remove(target)){

            while(time > 0){

                sr.color = new Color(time,time,time,time);
                time -= Time.deltaTime;
                await Task.Yield();

            }

            Destroy(target.healthBar.gameObject);
            Destroy(target.gameObject);
            return;
        
        }

        while(time > 0){

            sr.color = new Color(time,time,time,time);
            time -= Time.deltaTime;
            await Task.Yield();

        }

        // GAME OVER (Player died)
        Debug.LogError("Main character died lol");

    }

    public void CharacterClicked(GameCharacter clicked)
    {
        if (currentC == null)
            currentC = GetCurrentCharacter();

        // Get the selected damage from the AbilityClickButton (default 10 if no button clicked)
        int selectedDamage = AbilityClickButton.selectedDamageGlobal;

        // Apply damage to the clicked character
        clicked.TakeDamage(selectedDamage);
        Debug.Log(currentC.gameObject.name + " attacked " + clicked.gameObject.name + " for " + selectedDamage + " damage!");

        // Reset damage back to default (10) after an attack
        AbilityClickButton.selectedDamageGlobal = 10;

        if (clicked == currentC)
        {
            print("it failed :(");
            return;
        }

        // Move to the next turn
        turn = (turn + 1) % (enemies.Count + 1);
        currentC = GetCurrentCharacter();
    }
}
