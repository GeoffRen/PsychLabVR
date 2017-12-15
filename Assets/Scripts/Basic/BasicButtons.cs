using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Holds some button clicks for the Basic scene.
public class BasicButtons : MonoBehaviour {

    public GameObject instructions;
    public GameObject barrier;
    public GameObject guide;

    // The InstructionContinue click, disables the barrier and guide and displays further instructions for 30 more seconds.
    public void InstructionContinue()
    {
        instructions.transform.Find("InstructionText").GetComponent<Text>().text = @"You are at work and have wanted to get to know Mark, one of your coworkers. 
He’s over there right now (on the left in the green shirt) - why don’t you walk up to him?";
        Invoke("KillInstruction", 30f);
        instructions.transform.Find("Continue").gameObject.SetActive(false);
        barrier.SetActive(false);
        guide.SetActive(false);
    }

    // Disables the instructions.
    private void KillInstruction()
    {
        instructions.SetActive(false);
    }


}
