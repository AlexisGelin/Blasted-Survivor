using DG.Tweening;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
    [SerializeField] protected TankData _data;
    [SerializeField] protected int _health;
    [SerializeField] int _coinGain;

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _canonTransform;

    [SerializeField] List<SpriteRenderer> _sprites;

    Sequence _damageColorTweek;
    List<Sequence> _damageColorTweeks = new List<Sequence>();
    bool _isDead;

    public AIPath AIPath;

    public Rigidbody2D Rb { get => _rb; }
    public TankData Data { get => _data; }
    public int GetBodyDamage { get => _data.BodyDamage; }

    public virtual void Init()
    {
        _isDead = false;

        _health = _data.Health;

        _rb.simulated = true;

        transform.localScale = Vector3.one;
        foreach (var sprite in _sprites) sprite.DOFade(1, 0);
    }

    public bool TakeDamage(int amount)
    {
        if (!_isDead)
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

                _damageColorTweek.Join(sprite.DOColor(Color.white, .1f))
                    .Join(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), .1f))
                    .Append(sprite.DOColor(tempColor, .1f))
                    .Join(transform.DOScale(Vector3.one, .1f));

                _damageColorTweeks.Add(_damageColorTweek);
            }

            if (_health <= 0)
            {
                _isDead = true;
                WaveManager.Instance.EnnemyDie();
                StartCoroutine(ScaleTankAndDisable());
                return true;
            }
        }

        return false;
    }

    IEnumerator ScaleTankAndDisable()
    {
        transform.DOScale(new Vector3(2, 2, 2), .5f);

        _rb.simulated = false;

        foreach (var sprite in _sprites) sprite.DOFade(0, .5f);

        yield return new WaitForSeconds(.3f);
       
        gameObject.SetActive(false);
    }

    public void TakeHeal(int amount)
    {
        throw new System.NotImplementedException();
    }

}
