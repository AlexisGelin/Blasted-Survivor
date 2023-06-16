using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClose : Enemy
{
    public override void Init()
    {
        base.Init();
        StartCoroutine(MooveEnemyClose());
    }
   
    IEnumerator MooveEnemyClose()
    {
        while (_health > 0)
        {
            AIPath.destination = PlayerController.Instance.transform.position;
            Vector2 direction = PlayerController.Instance.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            yield return new WaitForEndOfFrame();
        }
    }
}
