using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Dictionary<int, Bullet> bullets = new Dictionary<int, Bullet>();
    private static int nextBulletId = 1;

    public int id;
    public Rigidbody rigidBody;
    public int shotByPlayer;
    public Vector3 initialForce;
    public float damage = 50f;

    private void Start()
    {
        id = nextBulletId;
        nextBulletId++;
        bullets.Add(id, this);

        ServerSend.SpawnBullet(this, shotByPlayer);

        rigidBody.AddForce(initialForce);
        StartCoroutine(RemoveAfterTime());
    }

    private void FixedUpdate()
    {
        ServerSend.BulletPosition(this);
    }

    public void Initialize(Vector3 _initialMovementDirection, float _initialForceStrength, int _shotByPlayer)
    {
        initialForce = _initialMovementDirection * _initialForceStrength;
        shotByPlayer = _shotByPlayer;
    }

    private void OnTriggerEnter(Collider _collider)
    {
        ServerSend.BulletHit(this);

        Player player = _collider.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator RemoveAfterTime()
    {
        yield return new WaitForSeconds(10f);

        ServerSend.RemoveBullet(this);
        Destroy(gameObject);
    }
}
