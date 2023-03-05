using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] TankData _data;
    [SerializeField] int _health;

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _canonTransform;

    [SerializeField] List<SpriteRenderer> _sprites;

    Sequence _damageColorTweek;
    List<Sequence> _damageColorTweeks = new List<Sequence>();

    public Rigidbody2D Rb { get => _rb; }
    public TankData Data { get => _data; }

    public void Init()
    {
        _health = _data.Health;

        _rb.simulated = true;

        transform.localScale = Vector3.one;
        foreach (var sprite in _sprites) sprite.DOFade(1, 0);

    }

    public bool TakeDamage(int amount)
    {
        _health -= amount;

        if (_damageColorTweeks != null)
        {
            foreach (var tween in _damageColorTweeks)
            {
                tween.Complete();
            }
        }


        foreach (var sprite in _sprites)
        {
            Color tempColor = sprite.color;

            _damageColorTweek = DOTween.Sequence();

            _damageColorTweek.Join(sprite.DOColor(Color.white, .05f))
                .Join(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), .05f))
                .Append(sprite.DOColor(tempColor, .05f))
                .Join(transform.DOScale(Vector3.one, .05f));

            _damageColorTweeks.Add(_damageColorTweek);
        }

        if (_health < 0)
        {
            StartCoroutine(ScaleBulletAndDisable());
            return true;
        }

        return false;
    }

    IEnumerator ScaleBulletAndDisable()
    {
        transform.DOScale(new Vector3(2, 2, 2), .3f);

        _rb.simulated = false;

        foreach (var sprite in _sprites) sprite.DOFade(0, .3f);

        yield return new WaitForSeconds(.3f);
       
        gameObject.SetActive(false);
    }

}
