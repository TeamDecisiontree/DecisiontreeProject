using System;
using System.Collections;
using UnityEngine;

public class Intro : SceneScript {

    public override IEnumerator RunAnimation() {

        //"TheMiracle" image is set.
        bg.SetBG(0);

        db.MoveDialogueBox(-350, 150);

        db.Enqueue(@"
        Narrator§It all started that day. The 12th of March, 2078. Some call it “The Miracle”, while others call it “The Disaster”.
        ");
        db.Enqueue(@"
        Narrator§Suddenly, every human on Earth became able to use magic. Some could grow plants quicker, some could light candles from a distance; Some could turn a town into a crater the size of Manhattan.
        ");
        db.Enqueue(@"
        Narrator§The most powerful among them became eager to use their own powers. As a result, the biggest revolution in human history began. It was complete anarchy. 
        ");

        db.ContinueDialogue();
        yield return WaitForDialogue();
        yield return WaitForSecs(1);

        
        //"TheNuke" image is set.
        bg.SetBG(1);
        yield return WaitForSecs(0.5f);

        db.Enqueue(@"
        Narrator§All the countries in the world tried to stop it. Air raids were called, giant battlefields were erected, and many lives were lost. After the entirety of France was taken over, the first nuke was launched. Then came another. And another.
        But it didn't matter.
        ");
        db.Enqueue(@"
        Narrator§In today's society, 1000 years after that day, some order has managed to take root in the world. The “Law of the Jungle“ is still very much in effect, but instead of allowing just anyone to go and kill the leader of a country, regular “Arena“ battles are held. 
        ");
        db.Enqueue(@"
        Narrator§Anyone can enter the Arena, and the final victor is deemed the “strongest“, and thus is granted the right to lead everyone else in power.
        ");


        db.ContinueDialogue();
        yield return WaitForDialogue();
        yield return WaitForSecs(1);

        //"raidSlums" image is set.
        bg.SetBG(2);
        db.MoveDialogueBox(0, 0);
        yield return WaitForSecs(0.5f);

        db.Enqueue(@"
        Narrator§On one fateful day, Ynnos and his family were having dinner in their home at “The Slums”, inside the capital of “Kirderf”.
        ");
        db.Enqueue(@"
        Narrator§Suddenly, a raid began, where the highest brass of the city swarmed through The Slums to gather more (involuntary) workers for their own secret operations.
        ");
        db.Enqueue(@"
        Narrator§Among them, Ynnos’ parents were both taken, leaving him and his younger sister to fend for themselves.
        ");

        db.ContinueDialogue();

        yield return WaitForDialogue();
        yield return WaitForSecs(1);

        //"[Not Decided]" image is set.
        bg.SetBG(3);
        yield return WaitForSecs(0.5f);

        db.Enqueue(@"
        Narrator§Now, 10 years later, Ynnos had become a ‘Fighter’; One who fights in the Arena not for prestige or for power, but simply for the money. 
        ");
        db.Enqueue(@"
        Narrator§After all, when spectators bet on who wins a fight, the fighter who wins gets a cut of the profits.
        ");
        db.Enqueue(@"
        Narrator§Thus began the adventure of “Ynnos”, the one who would eventually become the “Ekamer”.....
        ");
        db.ContinueDialogue();
        yield return WaitForDialogue();
        yield return WaitForSecs(1);
    }

}