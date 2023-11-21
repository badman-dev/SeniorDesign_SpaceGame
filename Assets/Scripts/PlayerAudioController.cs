using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    public AudioSource radiationSource;
    public AudioClip lowDmgAudio;
    public AudioClip medDmgAudio;
    public AudioClip highDmgAudio;

    private List<int> tierList = new List<int>();
    private int currentTier = 0;

    public void PlayRadiationAudio(int tier) {
        if (tier == currentTier) { return; }

        currentTier = tier;

        switch(tier)
        {
            case 0:
                radiationSource.clip = null;
                radiationSource.Stop();
                return;
            case 1:
                radiationSource.clip = lowDmgAudio;
                break;
            case 2:
                radiationSource.clip = medDmgAudio;
                break;
            case 3:
                radiationSource.clip = highDmgAudio;
                break;
        }
        radiationSource.Play();
    }

    public void StopRadiationAudio() {
        radiationSource.Stop();
    }

    public void AddRadiationIndex(int tier)
    {
        tierList.Add(tier);
        UpdateHighestRadiationIndex();
    }

    public void RemoveRadiationIndex(int tier)
    {
        if (!tierList.Contains(tier)) { return; }
        tierList.Remove(tier);
        UpdateHighestRadiationIndex();
    }

    public void UpdateHighestRadiationIndex()
    {
        int highest = 0;
        foreach(int tier in tierList)
        {
            if (tier > highest) { highest = tier; }
        }

        PlayRadiationAudio(highest);
    }
}
