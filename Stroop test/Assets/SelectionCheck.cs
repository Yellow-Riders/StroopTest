using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionCheck : MonoBehaviour
{
    public AudioSource audioPlayer;
    public AudioClip swishSound;
    private bool played;
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (!played)
            {
                audioPlayer.PlayOneShot(swishSound);
                played = true;
            }
        }
        else
        {
            played = false;
        }
    }
}
