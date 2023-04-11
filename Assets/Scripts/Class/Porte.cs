using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Porte : MonoBehaviour
{
    [SerializeField] LayerMask layerDetection;

    [SerializeField] int doorPrice;
    [SerializeField] TextMeshPro priceText;
    [SerializeField] BoxCollider2D wallCollider;
    [SerializeField] Animator glowDoorAnimator;
    [SerializeField] Animation ExplodeAnimationDoor;
    [SerializeField] GameObject LockParent;
    [SerializeField] ParticleSystem ExplosionDoorParticles;

    private bool isPay = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPay)
        {
            priceText.text = doorPrice.ToString();
            glowDoorAnimator.SetTrigger("Start");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isPay) glowDoorAnimator.SetTrigger("Stop");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayerController.Instance.isInteract && !isPay && PlayerManager.Instance.Coin >= doorPrice)
        {
            ExplosionDoorParticles.Play();
            ExplodeAnimationDoor.Play();
            LockParent.SetActive(false);
            PlayerManager.Instance.UpdateCoins(-doorPrice);
            glowDoorAnimator.SetTrigger("Stop");
            isPay = true;
        }
    }
}
