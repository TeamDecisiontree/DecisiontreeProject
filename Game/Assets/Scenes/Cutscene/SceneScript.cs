using System.Collections;
using UnityEngine;

public abstract class SceneScript : MonoBehaviour {

    [HideInInspector] public DialogueBox db;
    [HideInInspector] public Backgrounds bg;
    [HideInInspector] public bool waitingForDialogue;
    [HideInInspector] public bool skipping = false;

    public void LoadCutscene(DialogueBox db, Backgrounds bg){

        this.db = db;
        this.bg = bg;

    }

    public void SkipCutscene(){
        if (!skipping) skipping = true;
    }

    public IEnumerator WaitForSecs(float seconds){
        yield return new WaitForSeconds(skipping ? 0 : seconds);
    }

    public abstract IEnumerator RunAnimation();

    public IEnumerator WaitForDialogue(){
        while(waitingForDialogue && !skipping){
            yield return null;
        }
    }

}