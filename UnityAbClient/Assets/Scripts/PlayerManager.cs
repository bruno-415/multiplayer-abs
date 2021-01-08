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
    public GameObject muzzleFlash;

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
    }

    public void Respawn()
    {
        model.enabled = true;
        SetHealth(maxHealth);
    }

    public void CreateMuzzleFlash(Vector3 _position)
    {
        transform.position = _position;

        ParticleSystem _muzzleFlash = GetComponentInChildren<ParticleSystem>();
        _muzzleFlash.Play();
        //GameObject _muzzleFlash = Instantiate(muzzleFlash, transform.position, Quaternion.identity);
        //StartCoroutine(DestroyAfterTime(_muzzleFlash));

    }

    private IEnumerator DestroyAfterTime(GameObject _object)
    {
        yield return new WaitForSeconds(1f);
        Destroy(_object);
    }

}
