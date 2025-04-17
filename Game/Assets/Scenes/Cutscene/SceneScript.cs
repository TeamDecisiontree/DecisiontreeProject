using System.Collections;
using UnityEngine;

public abstract class SceneScript : MonoBehaviour {

    [HideInInspector] public DialogueBox db;
    [HideInInspector] public GameObject dialogueBoxGO;
    [HideInInspector] public bool waitingForDialogue;

    public abstract void LoadCutscene(GameObject dbgo);
    public abstract IEnumerator RunAnimation();

}