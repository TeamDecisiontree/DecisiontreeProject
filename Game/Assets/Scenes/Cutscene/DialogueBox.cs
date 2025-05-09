using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour{

    SceneScript sc;
    List<string> queue = new List<string>();

    public TMP_Text talkingName;
    public TMP_Text talkingText;

    public void Init(SceneScript sc){

        this.sc = sc;
        transform.GetChild(0).gameObject.SetActive(false);
        
    }

    public void Enqueue(string dialogue){

        queue.Add(dialogue);

    }

    //Empties all enqueued dialogues.
    public void SkipDialogue(){
        queue.Clear();
        ContinueDialogue();
    }

    public void DisplayNext(){
        
        string who = queue[0].Split('§')[0];
        string dialogue = queue[0].Split('§')[1];

        talkingName.text = who;
        talkingText.text = dialogue;

        transform.GetChild(0).gameObject.SetActive(true);
        
        queue.RemoveAt(0);

    }

    public void ContinueDialogue(){

        sc.waitingForDialogue = true;

        if(queue.Count > 0)
            DisplayNext();

        else {

            transform.GetChild(0).gameObject.SetActive(false);
            sc.waitingForDialogue = false;

        }

    }

    //Used for moving the dialogue box during cutscenes.
    public void MoveDialogueBox(float positionX, float positionY){
        RectTransform rect = gameObject.transform.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(positionX, positionY);
    }

}