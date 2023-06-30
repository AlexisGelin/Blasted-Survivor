using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : Enemy
{
    [SerializeField] float _rangeEnemy;
    [SerializeField] bool _canMoove;
    [SerializeField] float _durationBeforeFirstShoot;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] Transform _renderer;

    TankRenderer _tankRenderer;

    float _nextFire = 0.0f;
    BulletEnemy _bullet;
    bool isFirstShoot;

    List<Sequence> _canonSequences = new List<Sequence>();
    List<Vector2> _canonTransforms = new List<Vector2>();
    Coroutine mooveEnemyAndShoot;
    public override void Init(int hpToIncrease)
    {
        base.Init(hpToIncrease);

        isFirstShoot = true;
        _tankRenderer = _data.Renderer;

        if (_tankRenderer != null)
        {
            foreach (Transform TankRenderer in _tankRenderer._canonTransforms)
            {
                DOTween.Kill(TankRenderer);
            }
            //Destroy(_tankRenderer.gameObject);
        }

        var tempRenderer = Instantiate(_data.Renderer.Renderer, _renderer);
        _tankRenderer = tempRenderer.GetComponent<TankRenderer>(); //Here

        _canonTransforms.Clear();

        foreach (var canon in _tankRenderer._canonTransforms)
        {
            _canonTransforms.Add(canon.localPosition);
        }

        mooveEnemyAndShoot = StartCoroutine(MooveEnemyAndShoot());
    }

    IEnumerator MooveEnemyAndShoot()
    {
        while (_health > 0)
        {

            Vector2 direction = PlayerController.Instance.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


            if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) <= _rangeEnemy)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, PlayerController.Instance.transform.position - transform.position, 1000f, _layerMask);
                if (hit.collider.gameObject.layer == 10)
                {
                    _canMoove = false;
                    if (isFirstShoot)
                    {
                        yield return new WaitForSeconds(_durationBeforeFirstShoot);
                        if (hit.collider.gameObject.layer != 10)
                        {
                            isFirstShoot = true;
                        }
                        else
                        {
                            isFirstShoot = false;
                        }
                    }
                    if (!isFirstShoot)
                    {
                        if (Time.time > _nextFire)
                        {
                            _nextFire = Time.time + _data.FireRate;

                            AudioManager.Instance.PlaySound("PlayerShoot");

                            foreach (Sequence seq in _canonSequences)
                            {
                                seq.Kill();
                            }

                            _canonSequences.Clear();

                            for (int i = 0; i < _tankRenderer._canonTransforms.Count; i++)
                            {
                                var currentCannon = _tankRenderer._canonTransforms[i];
                                //print(currentCannon.position);
                                GameObject bullet = PoolManager.Instance.GetPooledObject(1);
                                _bullet = bullet.GetComponent<BulletEnemy>();

                                bullet.transform.position = currentCannon.position;
                                bullet.SetActive(true);

                                _bullet.Init(_data.Bullet, currentCannon.transform.right);

                                currentCannon.transform.localPosition = new Vector3(_canonTransforms[i].x, _canonTransforms[i].y);

                                _tankRenderer.ShootFX[i].Play();

                                var newSeq = DOTween.Sequence()
                                   .Join(currentCannon.transform.DOLocalMoveX(currentCannon.transform.localPosition.x - .05f, .05f).SetEase(Ease.OutSine))
                                   .Append(currentCannon.transform.DOLocalMoveX(currentCannon.transform.localPosition.x + .05f, .1f).SetEase(Ease.OutSine))
                                   .Append(currentCannon.transform.DOLocalMoveX(currentCannon.transform.localPosition.x, .5f).SetEase(Ease.OutSine));

                                _canonSequences.Add(newSeq);
                            }
                        }
                    }
                }
                else
                {
                    _canMoove = true;
                }
            }
            else if (!_canMoove)
                _canMoove = true;

            if (_canMoove)
            {
                AIPath.destination = PlayerController.Instance.transform.position;
/*                direction = PlayerController.Instance.transform.position - transform.position;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/
            }
            else
            {
                AIPath.destination = transform.position;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public override IEnumerator ScaleTankAndDisable()
    {
        //DOTween.KillAll();
/*        foreach (Sequence seq in _canonSequences)
        {
            seq.Kill();
        }

        _canonSequences.Clear();

        StopCoroutine(mooveEnemyAndShoot);*/

        return base.ScaleTankAndDisable();
    }
}
