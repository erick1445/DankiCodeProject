using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCoin : MonoBehaviour
{
   public int coinValue;

   private void OnTriggerEnter2D(Collider2D collision)
   {
       if(collision.gameObject.tag == "Player")
       {
           GameController.instance.UpdateScore(coinValue);
           Destroy(gameObject);
       }
   }
   
}
