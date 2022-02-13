using System;
using System.Collections;
using System.Collections.Generic;
using BulletSystem;
using UnityEngine;
using UnityEngine.VFX;

public class SpellEffect : MonoBehaviour
{

    [SerializeField] private VisualEffect spellVFX;
    // Start is called before the first frame update

    private void Start()
    {
        Spell.instance.onSpellUse += () =>
        {
            gameObject.SetActive(true);
            StartCoroutine(StartEffect());
        };
        
        gameObject.SetActive(false);
    }

    private IEnumerator StartEffect()
    {
        spellVFX.Play();
        yield return new WaitForSeconds(Spell.instance.spellDuration);
        spellVFX.Reinit();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("EnemyBullet")) return;
        var bullet = col.gameObject.GetComponent<Bullet>();
        if (bullet!=null)
        {
            bullet.InvokeBulletDeath();
        }
    }
}