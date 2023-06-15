using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAudioManager : MonoBehaviour
{
    public bool isNotPlayer;
    public void PlayStep(float volume)
    {
        if (isNotPlayer) return;
        AudioManager.Play("kidStep" + Random.Range(1, 10).ToString()).Volume(volume);
    }

    public void PlayLand(float volume)
    {
        if (isNotPlayer) return;
        AudioManager.Play("kidJumpLanding" + Random.Range(1, 3).ToString()).Volume(volume);
    }

    public void PlayJump(float volume)
    {
        if (isNotPlayer) return;
        AudioManager.Play("kidJumpEfffort" + Random.Range(1, 5).ToString()).Volume(volume);
    }
}
