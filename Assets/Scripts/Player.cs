using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

	private Rigidbody2D playerRigidBody;
	private Animator playerAnimator;

	[SerializeField]
 	private float velocity; 

 	[SerializeField]
 	private GameObject keyState;

	 [SerializeField]
	 private GameObject passwordState;

 	[SerializeField]
 	private AudioSource coinSound;

 	[SerializeField]
 	private AudioSource keySound;

 	private bool direction;
 	private int score;
 	public Text totalScore;
	private bool attackState;
	private bool slideState;
	private bool jumpState;

	private float jumpPower;

	private const float gravity = 9.8f;

	private bool isOnTheFloor;

    void Start()
    {
		jumpPower = 0;

    	score = 0;
    	direction = true;

    	playerRigidBody = GetComponent<Rigidbody2D>();

    	playerAnimator = GetComponent<Animator>();
    }

	void Update() {
		checkKeyboardControls();
	}

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
		
		basicMovements(horizontal);
		
        changeDirection(horizontal);

		playerRigidBody.gravityScale = jumpPower;

		if (jumpPower > gravity) {
			jumpPower -= playerRigidBody.mass;
		} else {
			jumpPower = gravity;
		}
		
		resetValues();
    }

	private void checkKeyboardControls() {
		attackState = Input.GetKeyDown(KeyCode.T);

		slideState = Input.GetKeyDown(KeyCode.Y);

		jumpState = Input.GetKeyDown(KeyCode.U);
	}

    private void basicMovements(float horizontal) {

		if (!this.playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {

			
			playerRigidBody.velocity = new Vector2(horizontal * velocity, playerRigidBody.velocity.y);

			playerAnimator.SetFloat("playerVelocity", Mathf.Abs(horizontal));

			if (attackState) {
				playerAnimator.SetTrigger("attackState");
				playerRigidBody.velocity = Vector2.zero;
			}

			if (slideState) {
				playerAnimator.SetTrigger("slideState");
			}

			if (jumpState && isOnTheFloor) {

				isOnTheFloor = false;

				jumpPower = - playerRigidBody.gravityScale * velocity;
				playerAnimator.SetTrigger("jumpState");
			}

		}
    }

    private void changeDirection(float horizontal) {
    	if (horizontal > 0 && direction == false || horizontal < 0 && direction == true) {
    		direction = !direction;
    		Vector3 currentDirecton = transform.localScale;
    		currentDirecton.x *= -1;
    		transform.localScale = currentDirecton;
    	}
    }

    private void OnCollisionEnter2D(Collision2D collision) {
    	if (collision.gameObject.tag == "Coin") {
    		collision.gameObject.SetActive(false);
    		score = score + 100;
    		updateScore(score);
    		coinSound.Play();
    	}

    	if (collision.gameObject.tag == "Key") {
    		collision.gameObject.SetActive(false);
    		keyState.SetActive(true);
    		keySound.Play();
    	}

		if (collision.gameObject.tag == "Floor") {
			isOnTheFloor = true;
		}

		if (collision.gameObject.name == "Box" && this.playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerSlide")) {
			collision.gameObject.SetActive(false);
			passwordState.SetActive(true);
		}
    }

    private void updateScore(int count) {
    	totalScore.text = "Score: " + count.ToString();
    }

	private void resetValues() {
		
	}
}