using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Porte : MonoBehaviour
{
    [SerializeField] int doorPrice;
    [SerializeField] TextMeshPro priceText;
    [SerializeField] BoxCollider2D wallCollider;
    [SerializeField] Animator glowDoorAnimator;
    [SerializeField] Animation ExplodeAnimationDoor;
    [SerializeField] GameObject LockParent;
    [SerializeField] ParticleSystem ExplosionDoorParticles;
    [SerializeField] List<Transform> floatingTextTransform;
    [SerializeField] List<Transform> placeHolderSpawnEnemies;

    [SerializeField] bool isNextRoomHaveSeveralsEntry;
    [SerializeField] List<Porte> otherDoorInNextRoom;

    private bool isPay = false;

    private void Start()
    {
        float rotationToSet = 0;
        switch (transform.rotation.eulerAngles.z)
        {
            case 0: 
                rotationToSet = 0; 
                break;
            case 90f: 
                rotationToSet = -90;
                break;
            case 270:
                rotationToSet = 90;
                break;
        }

        foreach (Transform t in floatingTextTransform)
        {
            t.transform.eulerAngles = new Vector3(
                t.transform.eulerAngles.x,
                t.transform.eulerAngles.y,
                t.transform.eulerAngles.z + rotationToSet
            );
        }
    }

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
        if (PlayerController.Instance.IsInteract && !isPay && PlayerManager.Instance.Coin >= doorPrice)
        {
            AudioManager.Instance.PlaySound("OpenDoor");

            wallCollider.gameObject.SetActive(false);
            ExplosionDoorParticles.Play();
            ExplodeAnimationDoor.Play();
            LockParent.SetActive(false);
            PlayerManager.Instance.UpdateCoins(-doorPrice);
            glowDoorAnimator.SetTrigger("Stop");
            foreach(Transform t in placeHolderSpawnEnemies)
            {
                SpawnPointEnemyManager.Instance.enemySpawnPoints.Add(t);
            }
            if (isNextRoomHaveSeveralsEntry)
            {
                foreach(Porte porte in otherDoorInNextRoom)
                {
                    porte.ClearListSpawnPoint();
                }
            }

            isPay = true;
        }
    }

    public void ClearListSpawnPoint()
    {
        placeHolderSpawnEnemies.Clear();
    }
}
