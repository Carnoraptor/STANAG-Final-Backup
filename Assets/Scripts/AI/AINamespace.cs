using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyGeneric
{
    public class EnemyUtils
    {
        //Function to deal damage to the player
        public void DealDamage(int damage, int armorPierce)
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().TakeDamage(damage, armorPierce);
        }
        
        public void SetSpriteToReal(GeneralAI genAI)
        {
            SpriteRenderer sr;
            foreach(Transform child in genAI.transform)
            {
                if (child.name.Contains("Image") || child.name.Contains("Img"))
                {
                    sr = child.GetComponent<SpriteRenderer>();
                    sr.sprite = genAI.enemySprite;
                }
            }
        }
    }
}