using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject puffParticle, boostParticle;
    new Rigidbody2D rigidbody;

    float impulseForce = 6f, upperLimit = 4.5f, boostTime = 4f, boostSpeed = 10f;
    float verticalInput, remainingBoost;
    bool isBoosted;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        isBoosted = false;
        this.gameObject.SetActive(false);
        this.startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {


        if (isBoosted)
        {
            remainingBoost -= Time.deltaTime;
            UIController.instance.SetFuel(remainingBoost / boostTime);
            if (remainingBoost < 0)
            {
                Unboost();
            }

            verticalInput = Input.GetAxis("Vertical");
        }
        else
        {
            if (this.transform.position.y > upperLimit && this.rigidbody.velocity.y > 0)
                this.rigidbody.velocity = Vector2.zero;
            if (Input.GetButtonDown("Jump") && !GameController.instance.isGameOver)
                Jump();
        }

    }

    private void FixedUpdate()
    {
        if (isBoosted)
        {
            this.rigidbody.position += Vector2.up * verticalInput * boostSpeed * Time.deltaTime;
            if (this.rigidbody.position.y > upperLimit)
            {
                this.rigidbody.position = new Vector2(this.rigidbody.position.x, upperLimit);
            }
        }
    }

    void Jump()
    {
        puffParticle.SetActive(false);
        this.rigidbody.velocity = Vector2.zero;
        this.rigidbody.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse);
        puffParticle.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
            UIController.instance.IncrementScore();
        else if (other.gameObject.CompareTag("Boost"))
            Boost();

        other.gameObject.SetActive(false);
    }

    void Boost()
    {
        isBoosted = true;
        remainingBoost = boostTime;
        GameController.instance.isBoosted = true;
        UIController.instance.ShowFuel(true);
        boostParticle.SetActive(true);
        rigidbody.gravityScale = 0;
        this.rigidbody.velocity = Vector2.zero;
    }

    void Unboost()
    {
        isBoosted = false;
        GameController.instance.isBoosted = false;
        UIController.instance.ShowFuel(false);
        boostParticle.SetActive(false);
        rigidbody.gravityScale = 1;
        this.rigidbody.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Unboost();
        GameController.instance.GameOver();
    }

    public void ResetBall()
    {
        this.transform.position = startPosition;
        this.transform.rotation = Quaternion.identity;
        this.gameObject.SetActive(true);
    }
}
