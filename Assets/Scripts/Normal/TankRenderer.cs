using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRenderer : MonoBehaviour
{
   public GameObject Renderer;
   public List<Transform> _canonTransforms;
   public List<SpriteRenderer> Sprite;
   public List<ParticleSystem> ShootFX;
}
