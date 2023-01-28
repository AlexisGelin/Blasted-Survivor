using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] BulletData _data;
    [SerializeField] Rigidbody2D _rb;

    [SerializeField] SpriteRenderer _bulletFace, _bulletFade;

    Coroutine _disableBullet;

    public void Init(Vector3 direction)
    {
        _rb.velocity = direction * _data.Speed;

        transform.localScale = Vector3.one;
        _bulletFade.DOFade(1, 0);
        _bulletFace.DOFade(1, 0);

        _disableBullet = StartCoroutine(DisableBullet());
    }

    IEnumerator DisableBullet()
    {
        yield return new WaitForSeconds(1);
        transform.DOScale(new Vector3(2, 2, 2), .3f);
        _bulletFace.DOFade(0, .3f);
        _bulletFade.DOFade(0, .3f);
        yield return new WaitForSeconds(.3f);

        gameObject.SetActive(false);
    }
}
