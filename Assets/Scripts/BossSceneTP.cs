using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneTP : MonoBehaviour
{
    public GameObject starSpawnCultist;
    public Transform sscTransform;
    public bool gunPickedUp;

    void OnDestroy()
    {
        if (!gunPickedUp)
        {
            Instantiate(starSpawnCultist, sscTransform);

            Vector2 plaPos = new Vector2(-58, 10);
            GameObject.FindWithTag("Player").transform.position = plaPos;

            foreach(BossSceneTP g in GameObject.FindObjectsOfType<BossSceneTP>())
            {
                g.gunPickedUp = true;
            }
        }
    }
}
