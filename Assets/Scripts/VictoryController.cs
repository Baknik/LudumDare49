using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryController : MonoBehaviour
{
    public TextMeshPro Text;
    public ParticleSystem ConfettiA;
    public ParticleSystem ConfettiB;
    public Switch Switch;

    private void Update()
    {
        if (Switch.TurnedOn)
        {
            Switch.TurnedOn = false;

            Text.text = "Thank you for playing!";
            ConfettiA.Play();
            ConfettiB.Play();
        }
    }
}
