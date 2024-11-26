using System;
using UnityEngine;
using UnityEngine.UI;

public class BoxEnemy : MonoBehaviour
{
     public Image fill;
     public GameObject particle;

     private float _damage = 30, _health = 100;

     private void OnCollisionStay(Collision other)
     {
          if (_health >= 0)
          {
               _health -= _damage;
               fill.fillAmount = _health / 100;
          }
          else
          {
               GameObject a = Instantiate(particle, transform.position, Quaternion.identity);
               a.transform.localScale = new Vector3(3,3,3);
               Destroy(gameObject);
          }
          
          Instantiate(particle,other.transform.position,Quaternion.identity);
          Destroy(other.gameObject);
     }
}
