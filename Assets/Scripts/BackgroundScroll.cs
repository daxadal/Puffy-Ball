using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    float width;

    // Start is called before the first frame update
    void Awake()
    {
        this.width = GetComponent<BoxCollider2D>().size.x * this.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.isGameOver)
        {
            this.transform.position += Vector3.left * Time.deltaTime * GameController.instance.scrollSpeed;
            if (this.transform.position.x < -width * 1.5f)
                this.transform.position += Vector3.right * width * 3;
        }

    }
}
