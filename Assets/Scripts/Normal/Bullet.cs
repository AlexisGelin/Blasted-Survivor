using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _bulletFace, _bulletFade;

    BulletData _data, _upgradeData;
    Coroutine _disableBullet;
    int _penetrationIndex;

    public BulletData Data { get => _data; }
    public Rigidbody2D Rb { get => _rb; }

    public void Init(BulletData data, BulletData upgradeData, Vector3 direction)
    {
        _data = data;
        _upgradeData = upgradeData;

        _penetrationIndex = _data.Penetration + _upgradeData.Penetration;
        _rb.velocity = (Vector2)direction * (_data.Speed + _upgradeData.Speed) * ((PlayerController.Instance.PlayerVelocity.magnitude / 6)+ 1f);
        Debug.Log(PlayerController.Instance.PlayerVelocity.magnitude);
        _rb.simulated = true;

        transform.localScale = Vector3.one;
        _bulletFade.DOFade(1, 0);
        _bulletFace.DOFade(1, 0);

        _disableBullet = StartCoroutine(DisableBullet());
    }

    IEnumerator DisableBullet()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(ScaleBulletAndDisable());
    }

    IEnumerator ScaleBulletAndDisable()
    {
        transform.DOScale(new Vector3(2, 2, 2), .3f);
        _bulletFace.DOFade(0, .3f);
        _bulletFade.DOFade(0, .3f);

        _rb.simulated = false;

        yield return new WaitForSeconds(.3f);

        gameObject.SetActive(false);
    }

    public void Collision()
    {
        _penetrationIndex--;

        if (_penetrationIndex <= 0)
        {
            StartCoroutine(ScaleBulletAndDisable());
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            EnemyController tempEnemy = collision.gameObject.GetComponent<EnemyController>();

            tempEnemy.Rb.AddForce(_rb.velocity);

            if (tempEnemy.TakeDamage(_data.Damage + _upgradeData.Damage))
            {
                PlayerManager.Instance.IncreaseExp(tempEnemy.Data.ExpOnDestroy);
            }

            Collision();
        }
    }
}
