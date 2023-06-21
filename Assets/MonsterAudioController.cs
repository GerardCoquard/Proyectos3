using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudioController : MonoBehaviour
{
    private AudioSourceHandler laserSound;
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
        StartCoroutine(LaserEvent());
    }

    IEnumerator LaserEvent()
    {
        laserSound = AudioManager.Play("spellActivation1").Volume(0.3f);
        yield return new WaitForSeconds(3);
        laserSound.Stop();
        laserSound = AudioManager.Play("fireLaser").Volume(0.3f);
    }
}
