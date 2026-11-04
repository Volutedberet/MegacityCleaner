using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayer : MonoBehaviour{
    public List<AudioSource> audios = new List<AudioSource>();

    public void PlaySound(int ID){
        audios[ID].Play();
    }

    public void StopSound(int ID){
        audios[ID].Stop();
    }
}