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

    public void DisplayNext(){
        
        string who = queue[0].Split('§')[0];
        string dialogue = queue[0].Split('§')[1];

        talkingName.text = who;
        talkingText.text = dialogue;

        transform.GetChild(0).gameObject.SetActive(true);
        
        queue.RemoveAt(0);

    }

    public void ContinueDialogue(){

        if(queue.Count > 0)
            DisplayNext();

        else {

            transform.GetChild(0).gameObject.SetActive(false);
            sc.waitingForDialogue = false;

        }

    }

}