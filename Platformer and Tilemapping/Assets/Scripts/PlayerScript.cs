using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    private int scoreValue;
    private int livesValue;
    public bool isDead;

    bool rightFace = true;
    public bool isOnGround;

    Animator anim;

    public float jump;
    public float speed;
    public TextMeshProUGUI score;
    public TextMeshProUGUI lives;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public AudioSource musicSource;
    public AudioSource effectSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioClip hurtSound;
    public AudioClip coinSound;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {

        scoreValue = 0;
        livesValue = 3;

        musicSource.clip = musicClipOne;
        musicSource.Play();

        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();

        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (hozMovement == 0.0 && livesValue > 0 && !isDead && isOnGround)
        {
          anim.SetInteger("State", 0);
        }

        if (hozMovement > 0.0 && livesValue > 0 && !isDead)
        {
            if (isOnGround)
            {
                anim.SetInteger("State", 1);
            }
            if (!rightFace)
            {
                Flip();
            }
        }

        if (hozMovement < 0.0 && livesValue > 0 && !isDead)
        {
            if (isOnGround)
            {
                anim.SetInteger("State", 1);
            }
      
            if (rightFace)
            {
                Flip();
            }
         }

        anim.SetBool("Airborne", !isOnGround);
        
        if (isDead)
        {
            anim.SetInteger("State", 6);
            jump = 0;
            speed = 0;
            anim.SetBool("Dead", isDead);
        }

    }

    void  Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        rightFace = !rightFace;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            effectSource.clip = coinSound;
            effectSource.Play();

            if (scoreValue == 4)
            {
                transform.position = new Vector2(0f, 12.67f);
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();
            }

            if (scoreValue == 8)
            {
                transform.position = new Vector2(0f, 27.31f);
                musicSource.clip = musicClipTwo;
                musicSource.Play();
                winTextObject.SetActive(true);
            }
        }
        
        if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            livesValue--;
            lives.text = "Lives: " + livesValue.ToString();

            anim.SetInteger("State", 6);
            effectSource.clip = hurtSound;
            effectSource.Play();

            if (livesValue <= 0)
            {
                isDead = true;
                loseTextObject.SetActive(true);
            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W) && !isDead)
            {
                rd2d.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);
            }
        }
    }

}