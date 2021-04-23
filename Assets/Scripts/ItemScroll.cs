using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScroll : MonoBehaviour
{
    void Update()
    {
        if (!GameController.instance.isGameOver)
        {
            this.transform.position += Vector3.left * Time.deltaTime * GameController.instance.scrollSpeed;
            if (this.transform.position.x < GameController.instance.destroyX)
                this.gameObject.SetActive(false);
        }
        

    }
}
