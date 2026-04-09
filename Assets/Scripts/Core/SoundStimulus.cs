using System.Collections.Generic;
using UnityEngine;

public class SoundStimulus : Stimulus
{
    [SerializeField] private List<AudioClip> _audioClips;

    protected override void OnFire()
    {
        if (_audioClips.Count == 0)
            return;

        int idx = Random.Range(0, _audioClips.Count);
        AudioClip _audioClip = _audioClips[idx];
        AudioManager.PlaySound(_audioClip, transform.position, detectableDistance);
    }
}
