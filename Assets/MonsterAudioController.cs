using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudioController : MonoBehaviour
{
    public void BreakSound()
    {
        AudioManager.Play("monsterHit1").Volume(0.3f);
    }

    public void BreathSound()
    {
        StartCoroutine(soundBreath(Random.Range(4,8)));
    }

    IEnumerator soundBreath(float time)
    {
        yield return new WaitForSeconds(time);
        AudioManager.Play("monsterBreathing" + Random.Range(1, 5).ToString()).Volume(0.3f);
        StartCoroutine(soundBreath(Random.Range(4, 8)));

    }

    public void DieSound()
    {
        AudioManager.Play("monsterGrumble").Volume(0.3f);
    }

    public void CancelBreath()
    {
        StopAllCoroutines();
    }

    public void Spell()
    {
        AudioManager.Play("spellActivation1").Volume(0.3f);
    }
}
