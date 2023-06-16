using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] SpriteRenderer _preview;
    [SerializeField] GameObject _tankPreviewTransform;
    [SerializeField] TMP_Text _price;
    [SerializeField] CanvasGroup _priceCG;

    GameObject _rewardGO;
    TankData _tankData;
    ChestData _data;
    bool _isPay, _canEquip;
    int _chestPrice;

    public void Init(ChestData data)
    {
        _isPay = false;
        _canEquip = false;

        _data = data;

        _preview.sprite = _data.Preview;
        _price.text = _data.Price.ToString();
        _chestPrice = _data.Price;

        _tankData = GetRandomTank();
    }

    TankData GetRandomTank()
    {
        int maxWeight = 0;
        int tmpWeight = 0;

        foreach (var tank in _data.TankPool)
        {
            maxWeight += tank.Probabilities;
        }

        int choice = Random.Range(0, maxWeight);

        foreach (var tank in _data.TankPool)
        {
            if (choice >= tmpWeight && choice < tank.Probabilities + tmpWeight)
            {
                return tank.TankData;
            }

            tmpWeight += tank.Probabilities;
        }


        return _data.TankPool.Last().TankData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if (!_isPay)
            {
                ShowPrice();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if (!_isPay)
            {
                HidePrice();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayerController.Instance.IsInteract && _isPay && _canEquip)
        {
            _canEquip = false;

            PlayerController.Instance.Evolve(_tankData.Name);

            HideReward();
        }

        if (PlayerController.Instance.IsInteract && !_isPay && PlayerManager.Instance.Coin >= _chestPrice)
        {
            PlayerManager.Instance.UpdateCoins(-_chestPrice);
            _isPay = true;

            AudioManager.Instance.PlaySound("OpenChest");

            StartCoroutine(CanEquip());

            ShowReward();
            HidePrice();
        }
    }

    IEnumerator CanEquip()
    {
        yield return new WaitForSeconds(1f);
        _canEquip = true;
    }

    void ShowReward()
    {

        Sequence rotateSeq = DOTween.Sequence();
        _rewardGO = Instantiate(_tankData.Renderer.Renderer, _tankPreviewTransform.transform);
        rotateSeq
            .Join(_rewardGO.transform.DORotate(new Vector3(0, 0, 180), 2).SetEase(Ease.Linear))
            .Append(_rewardGO.transform.DORotate(new Vector3(0, 0, 360), 2).SetEase(Ease.Linear));

        rotateSeq.SetLoops(-1);

        rotateSeq.Play();
    }
    void HideReward()
    {
        DOTween.Kill(_rewardGO.transform);
        _rewardGO.SetActive(false);
    }

    void ShowPrice()
    {
        _priceCG.gameObject.SetActive(true);
        _priceCG.DOFade(1, .2f);
    }

    void HidePrice()
    {
        _priceCG.DOFade(0, .2f).OnComplete(() => _priceCG.gameObject.SetActive(false));
    }
}
