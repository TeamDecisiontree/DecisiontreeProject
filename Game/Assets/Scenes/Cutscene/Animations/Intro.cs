using System.Collections;
using UnityEngine;

public class Intro : SceneScript {

    public override void LoadCutscene(GameObject dbgo){
        
        db = Instantiate(dbgo, GameObject.Find("Canvas").transform).GetComponent<DialogueBox>();
        db.Init(this);

    }

    public override IEnumerator RunAnimation() {

        int frame = 300;

        while(frame-- > 0)
            yield return new WaitForSeconds(1/60);

        db.Enqueue(@"
        Narrator§It all started that day. The 12th of March, 2078. Some call it “The Miracle”, while others call it “The Disaster”.
        ");
        db.Enqueue(@"
        Narrator§Suddenly, every human on Earth became able to use magic. Some could grow plants quicker, some could light candles from a distance; Some could turn a town into a crater the size of Manhattan.
        ");
        db.Enqueue(@"
        Narrator§The most powerful among them became eager to use their own powers. As a result, the biggest revolution in human history began. It was complete anarchy. 
        ");
        db.Enqueue(@"
        Narrator§All the countries in the world tried to stop it. Air raids were called, giant battlefields were erected, and many lives were lost. After the entirety of France was taken over, the first nuke was launched. Then came another. And another.
But it didn't matter.
        ");
        db.Enqueue(@"
        Narrator§The “Law of the Jungle“ is still very much in effect, but instead of allowing just anyone to go and remove the leader of a country, regular “Arena“ battles are held. 
        ");
        db.Enqueue(@"
        Narrator§Anyone can enter the Arena, and the final victor is deemed the “strongest“, and thus is granted the right to lead everyone else in power.
        ");

        db.ContinueDialogue();
        waitingForDialogue = true;

        while(waitingForDialogue)
            yield return null;

        frame = 300;

        while(frame-- > 0)
            yield return new WaitForSeconds(1/60);

    }

}