using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth;
    public MeshRenderer model;
    public MeshRenderer pistolModel;

    Animator anim;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
    }

    public void SetHealth(float _health)
    {
        health = _health;

        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        model.enabled = false;
        pistolModel.enabled = false;
    }

    public void Respawn()
    {
        model.enabled = true;
        pistolModel.enabled = true;
        SetHealth(maxHealth);
    }

    public void PlayWeaponEffects(Vector3 _position)
    {
        transform.position = _position;

        GetComponentInChildren<ParticleSystem>().Play();

        GetComponentInChildren<AudioSource>().Play();

        GetComponentInChildren<Animator>().Play("PistolShot");
    }

}
