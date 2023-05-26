using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _bulletFace, _bulletFade;
    [SerializeField] TrailRenderer _trailRenderer;
    [SerializeField] GameObject _onHitFX, _circleFX;

    BulletData _data, _upgradeData;
    Coroutine _disableBullet;
    int _penetrationIndex;

    Tweener _trailRendererBoing;
    Vector3 _direction;


    public BulletData Data { get => _data; }
    public Rigidbody2D Rb { get => _rb; }

    public void Init(BulletData data, BulletData upgradeData, Vector3 direction)
    {
        _data = data;
        _upgradeData = upgradeData;

        _direction = direction;

        _penetrationIndex = _data.Penetration + _upgradeData.Penetration;
        _rb.velocity = _direction * (_data.Speed + _upgradeData.Speed) + (PlayerController.Instance.PlayerVelocity / 10);
        _rb.simulated = true;

        transform.localScale = new Vector3(data.BulletSize, data.BulletSize, data.BulletSize);

        _trailRenderer.Clear();
        _trailRendererBoing.Kill();

        _trailRendererBoing = DOVirtual.Float(1, .4f, .1f, x => _trailRenderer.widthMultiplier = x).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);


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
        var circleFX = Instantiate(_circleFX, transform.position, Quaternion.identity);
        var circle = circleFX.GetComponent<SpriteRenderer>();



        if (collision.gameObject.layer == 20)
        {
            EnemyController tempEnemy = collision.gameObject.GetComponent<EnemyController>();

            tempEnemy.Rb.AddForce(_rb.velocity);

            if (tempEnemy.TakeDamage(_data.Damage + _upgradeData.Damage))
            {
                PlayerManager.Instance.IncreaseExp(tempEnemy.Data.ExpOnDestroy);
            }

            DOTween.Sequence()
                .Join(circle.transform.DOPunchScale(new Vector3(.2f, .2f, .2f), .1f))
                .Append(circle.DOFade(0, .2f));

            Destroy(circleFX, .4f);

            var hitFX = Instantiate(_onHitFX, transform);
            hitFX.transform.LookAt(transform.position - _direction);
            Destroy(hitFX, hitFX.GetComponent<ParticleSystem>().main.duration);

            CameraManager.Instance.ShakeCamera(1);
            TimeManager.Instance.DoLagTime();

            Collision();
        }

        if (collision.gameObject.layer == 30 || collision.gameObject.layer == 31)
        {
            DOTween.Sequence()
                .Join(circle.transform.DOPunchScale(new Vector3(.1f, .1f, .1f), .1f))
                .Append(circle.DOFade(0, .1f));

            Destroy(circleFX, .4f);

            var hitFX = Instantiate(_onHitFX, transform);
            hitFX.transform.LookAt(transform.position - _direction);
            Destroy(hitFX, hitFX.GetComponent<ParticleSystem>().main.duration);

            StartCoroutine(ScaleBulletAndDisable());
        }
    }
}
