using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour{

    public GameObject dialogueBoxGO;
    public SceneScript[] sceneScripts;

    public void SwitchScene(int from, int to, int cutscene){

        AsyncOperation departingOp = SceneManager.UnloadSceneAsync(from);
        StartCoroutine(DoCutscene(departingOp, to, cutscene));

    }

    IEnumerator DoCutscene(AsyncOperation from, int to, int cutscene){

        while(!from.isDone)
            yield return null;

        AsyncOperation arrivingSceneOp = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        arrivingSceneOp.allowSceneActivation = false;

        if(cutscene >= 0 && cutscene < sceneScripts.Length) {

            sceneScripts[cutscene].LoadCutscene(dialogueBoxGO);
            yield return sceneScripts[cutscene].RunAnimation();

        }

        Destroy(GameObject.Find("Canvas"));
        arrivingSceneOp.allowSceneActivation = true;

        while(!arrivingSceneOp.isDone)
            yield return null;

        Scene arrivingScene = SceneManager.GetSceneAt(1);
        SceneManager.SetActiveScene(arrivingScene);
        SceneManager.UnloadSceneAsync(10);

    }

    // TODO create save using AsyncOperation

}
