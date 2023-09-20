using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Collections.Generic;


public class CharacterController2D : MonoBehaviour
{
	[Header("General")]
	public float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	public float gravity;
	public float maxVertSpeed = 15f;
	//[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	public Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	public playerSettings m_Settings;

	[Header("Specialized Jump")]
	private PlayerController m_PlayerController;
	public int coyoteTimer = 3;
	public bool doubleJump = false;
	public float dashSpeed = 1.2f;
	public bool _dashing = false;
	public bool canDash = true;

	[Header("Audio")]
	public AudioClip[] jumpSFX;
	public AudioClip landingSFX;
	public AudioClip wallGrabSFX;
	public AudioClip[] dashSFX;
	bool playWallGrabSFX = true;
	bool playLandingSFX = true;

	[Header("WallJump")]
	public float wallJumpTime = 0.2f;
	public float wallSlideSpeed = 0.8f;
	public float wallSlideBackUp;
	public float wallHoldTimer = 3f; 
	public float wallHoldTimerBackUp;
	public float wallDistance = 1.3f;
	[SerializeField] bool isWallSliding = false;
	[SerializeField] int jumpCounter = 0;
	[SerializeField] RaycastHit2D wallCheckHit;
	public GameObject lastWall;
	float jumpTime;
	GameObject newCollision;
	GameObject prevCollision;

	public Animator PlayerAnim;
	bool canMove = true;

	public Vector2 dashVector;
	public SpriteRenderer m_SpriteRenderer;
	public List<Sprite> dashImages = new List<Sprite>();
	public List<Sprite> dashGhostImages = new List<Sprite>();
	public int numGhosts;
	public float ghostTimer = 0.05f;
	public float ghostTimePassed = 0f;

	public GameObject m_dashGhost;

	public bool fromPossession = false;
	public bool fromPossessionDash = false;
	public possessionObject lastPossession;

	float shakeTimer = 0;
	bool crouching = false;
	private float moveCheck;
	public bool holdingWall = false;

	public bool fanActive = false;
	public Vector2 fanForce;

	[Header("Gravity")]
	public float tempGravScale;

	private void Awake()
	{
		//Set private values and disable stuff
		PlayerAnim = this.GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_PlayerController = GetComponent<PlayerController>();
		m_Settings = FindObjectOfType<playerSettings>();
		m_SpriteRenderer = GetComponent<SpriteRenderer>();

		wallSlideBackUp = wallSlideSpeed;
		wallHoldTimerBackUp = wallHoldTimer;

		tempGravScale = m_Rigidbody2D.gravityScale;
	}

	private void Update()
	{
		//Running coyote timer and using canMove
		if (coyoteTimer > 0 && m_Grounded == false)
		{
			coyoteTimer--;
		}
		else if (coyoteTimer <= 0 && m_Grounded == true)
        {
			coyoteTimer = 30;
		}

		if (shakeTimer > 0)
        {
			shakeTimer -= Time.deltaTime;
        }
        else
        {
			CinemachineVirtualCamera vcam = Camera.main.gameObject.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
			CinemachineBasicMultiChannelPerlin shake = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			shake.m_AmplitudeGain = 0f;
		}

		canMove = m_PlayerController.canMove;

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		if (horizontal == 0 && vertical == 0)
		{
			if (m_FacingRight)
				dashVector = new Vector2(1.25f, 0);
			else
				dashVector = new Vector2(-1.25f, 0);
		}
        else
        {
			dashVector = new Vector2(horizontal, vertical).normalized;
			//Debug.Log(dashVector);
		}

        if (_dashing && numGhosts < 3)
        {
			if (ghostTimePassed > ghostTimer)
			{
				int dashImage;
				dashImage = dashImages.IndexOf(m_SpriteRenderer.sprite);
				if (dashImage < 0)
					dashImage = 3;

				dashGhost ghost = Instantiate(m_dashGhost).GetComponent<dashGhost>();
				ghost.location.position = this.transform.position;
				ghost.location.localScale = this.transform.localScale;
				ghost._sprite.sprite = dashGhostImages[dashImage];

				ghostTimePassed = 0f;
				numGhosts++;
			}
            else
            {
				ghostTimePassed += Time.deltaTime;
            }
        }

        if (fanActive)
        {
			if (m_Grounded)
				m_Rigidbody2D.AddForce(fanForce);
			else
				m_Rigidbody2D.AddForce(fanForce * 2);

			if (m_Rigidbody2D.velocity.y > maxVertSpeed && !_dashing)
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, maxVertSpeed);
        }
	}

	private void FixedUpdate()
	{
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject && !(colliders[i].gameObject.GetComponent<pushableObject>() && colliders[i].gameObject.GetComponent<pushableObject>().hidden == true))
			{
				if (playLandingSFX)
				{
					this.GetComponent<AudioSource>().PlayOneShot(landingSFX);
					playLandingSFX = false;
				}

				if(colliders[i].isTrigger == false)
					m_Grounded = true;
				canDash = true;

				jumpCounter = 0;
				prevCollision = null;
				newCollision = null;
			}
		}

		if(canMove)
			PlayerAnim.SetBool("isGrounded", m_Grounded);

	}

	public void playerJump()
    {
		isWallSliding = false;


		if (holdingWall)
        {
			if (m_Rigidbody2D.gravityScale > 0)
				m_Rigidbody2D.gravityScale = tempGravScale;
			else
				m_Rigidbody2D.gravityScale = -tempGravScale;

			m_Rigidbody2D.velocity = Vector2.zero;
			holdingWall = false;
		}

		//Adjusting for reverse gravity
		/*
		if (m_Rigidbody2D.gravityScale < 0)
			jumpForce = -m_JumpForce;
		else
			jumpForce = m_JumpForce;
		*/

		//Debug.Log(new Vector2(0f, m_JumpForce));

		m_Grounded = false;

		m_Rigidbody2D.velocity = new Vector2(0, 0);

		if (m_Rigidbody2D.gravityScale > 0)
			m_Rigidbody2D.AddForce(new Vector2(0, m_JumpForce));
		else
			m_Rigidbody2D.AddForce(new Vector2(0, -m_JumpForce));

	}

	/*
	public void facingRightCheck()
    {
		float test = m_Rigidbody2D.velocity.x;
//		Debug.Log(test < 0 && m_FacingRight || test > 0 && !m_FacingRight);
		if (test < 0 && m_FacingRight || test > 0 && !m_FacingRight)
		{
			Invoke("forceFlip", 0.4f);
		}
	}
	*/

	public void forceFlip()
    {
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
	}

	public void dashing(bool fromPossession = false)
    {
		canDash = false;
		
		//screen shake
		CinemachineVirtualCamera vcam = Camera.main.gameObject.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
		CinemachineBasicMultiChannelPerlin shake = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		shake.m_AmplitudeGain = 0.5f;
		shakeTimer = 0.15f;

		m_Rigidbody2D.velocity = new Vector2(0, 0);
		float temp = dashSpeed * 0.5f;

		PlayerAnim.SetTrigger("Dash");
		PlayerAnim.SetBool("dashing", true);

		if (dashVector.y > 0.5f)
		{
			if (dashVector.x > 0.75f || dashVector.x < -0.75f)
				m_Rigidbody2D.AddForce(new Vector2(dashVector.x * dashSpeed * 1.5f, dashVector.y * temp), ForceMode2D.Impulse);
			else
				m_Rigidbody2D.AddForce(new Vector2(dashVector.x * dashSpeed, dashVector.y * temp), ForceMode2D.Impulse);
		}
		else
		{
			if (dashVector.x > 0.75f || dashVector.x < -0.75f)
				m_Rigidbody2D.AddForce(new Vector2(dashVector.x * dashSpeed * 1.5f, dashVector.y * dashSpeed), ForceMode2D.Impulse);
			else
				m_Rigidbody2D.AddForce(dashVector * dashSpeed, ForceMode2D.Impulse);
		}

		if (dashVector.x < 0.5 && dashVector.y < 0)
			m_PlayerController.downwardDash = true;

		jumpCounter = 0;


		_dashing = true;

		Invoke("resetDash", 0.4f);
	}

	void resetDash()
    {
		_dashing = false;
		numGhosts = 0;
		m_PlayerController.downwardDash = false;
		PlayerAnim.SetBool("dashing", false);
		if (m_Settings.infiniteDash || fromPossession)
			canDash = true;
		fromPossessionDash = false;
		fromPossession = false;
	}

	public void deadSFX(AudioClip deathSFX)
    {
		this.GetComponent<AudioSource>().PlayOneShot(deathSFX);
	}

	public void Move(float move, bool jump, bool dash, bool crouch, bool hold)
	{
		moveCheck = move;

		//if (!jump)
		//holdingWall = hold;

		if (canMove)
		{
			if (move != 0 && m_Grounded)
			{
				if(!PlayerAnim.GetBool("isWalking"))
					PlayerAnim.SetBool("isWalking", true);
				PlayerAnim.SetBool("Glide", false);
			}
			else if (m_Grounded && crouch)
			{
				if (!crouching)
                {
					PlayerAnim.SetTrigger("Crouch");
					crouching = true;
				}


				PlayerAnim.SetBool("crouching", true);
				PlayerAnim.SetBool("isWalking", false);
				PlayerAnim.SetBool("Glide", false);
			}
			else if (m_Grounded && !crouch)
			{
				crouching = false;
				PlayerAnim.SetBool("crouching", false);
				PlayerAnim.SetBool("isWalking", false);
				PlayerAnim.SetBool("Glide", false);
			}
			else if (!m_Grounded)
            {
				playLandingSFX = true;

				//PlayerAnim.SetTrigger("fall");
				PlayerAnim.SetBool("isWalking", false);
				PlayerAnim.SetBool("crouching", false);
			}
			else
			{
				PlayerAnim.SetBool("isWalking", false);
				PlayerAnim.SetBool("crouching", false);
				PlayerAnim.SetBool("Glide", false);
			}

			//only control the player if grounded or airControl is turned on
			if (m_Grounded || m_AirControl)
			{
				Vector3 targetVelocity;
				targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);     // And then smoothing it out and applying it to the character

				// If the input is moving the player right and the player is facing left...
				if (move > 0)
				{
					Flip(true);             // ... flip the player.
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0)
				{
					Flip(false);             // ... flip the player.
				}
                if (m_Grounded)
                {
					if (m_Rigidbody2D.gravityScale > 0)
						m_Rigidbody2D.gravityScale = tempGravScale;
					else
						m_Rigidbody2D.gravityScale = -tempGravScale;
				}

			}

			// If the player should jump...
			if (coyoteTimer > 0 && jump && jumpCounter < 1 || isWallSliding && jump && jumpCounter < 1)// || doubleJump && jump)
			{
				PlayerAnim.SetTrigger("Jump");

				coyoteTimer = 0;
				jumpCounter++;
				playerJump();

				int n = Random.Range(0, 2);
				this.GetComponent<AudioSource>().PlayOneShot(jumpSFX[n]);

				//if (!doubleJump)
				//	doubleJump = true;
				//else

				if(doubleJump)
					doubleJump = false;
			}
            else
            {
				if (!m_Grounded)
				{
					if (m_Settings.glideUnlock && hold && !dash && !jump && m_Rigidbody2D.velocity.y <= 0)
					{
						PlayerAnim.SetBool("Glide", true);

						if (m_Rigidbody2D.gravityScale > 0)
							m_Rigidbody2D.gravityScale = tempGravScale/3;
						else
							m_Rigidbody2D.gravityScale = -tempGravScale/3;
					}
					else
					{
						PlayerAnim.SetBool("Glide", false);
						PlayerAnim.SetTrigger("fall");

						if (m_Rigidbody2D.gravityScale > 0)
							m_Rigidbody2D.gravityScale = tempGravScale;
						else
							m_Rigidbody2D.gravityScale = -tempGravScale;

						if (Input.GetAxis("Vertical") < 0 && m_Rigidbody2D.gravityScale > 0)
						{
							m_Rigidbody2D.AddForce(new Vector2(0, -3));
						}
						else if (Input.GetAxis("Vertical") < 0 && m_Rigidbody2D.gravityScale < 0)
						{
							m_Rigidbody2D.AddForce(new Vector2(0, 3));
						}
					}
				}
			}

			//Wall Jump
			//Wall Jump needs tob e adjusted for 4 dimensions
			if (m_FacingRight)
			{
				wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0f), wallDistance, m_WhatIsGround);
				Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.red);
			}
			else
			{
				wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0f), wallDistance, m_WhatIsGround);
				Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.red);
			}

			if (wallCheckHit && !m_Grounded && Input.GetAxis("Horizontal") != 0)
			{
				isWallSliding = true;
				wallHoldTimer = wallHoldTimerBackUp;
				jumpTime = Time.time + wallJumpTime;

				if (jumpCounter >= 1 && wallCheckHit.transform.gameObject != lastWall)
				{
					jumpCounter = 0;
					lastWall = wallCheckHit.transform.gameObject;
				}
                else
                {
					lastWall = wallCheckHit.transform.gameObject;
				}

				if (playWallGrabSFX)
				{
					this.GetComponent<AudioSource>().PlayOneShot(wallGrabSFX);
					playWallGrabSFX = false;
				}
			}
			else if (jumpTime < Time.time)
			{
				isWallSliding = false;
				playWallGrabSFX = true;
			}

			//holdingWall = false;
			/*
			if (isWallSliding && !jump && !dash && !hold)
			{
				if (m_Rigidbody2D.gravityScale > 0)
					m_Rigidbody2D.gravityScale = tempGravScale;
				else
					m_Rigidbody2D.gravityScale = -tempGravScale;

				wallHoldTimer = wallHoldTimerBackUp;
			}
			else
            {
				holdingWall = false;
				wallHoldTimer = wallHoldTimerBackUp;

				if (m_Rigidbody2D.gravityScale > 0)
					m_Rigidbody2D.gravityScale = tempGravScale;
				else
					m_Rigidbody2D.gravityScale = -tempGravScale;
			}
			*/

			if (m_Grounded && !_dashing)
				canDash = true;
			else if (_dashing)
				canDash = false;

			if (dash && canDash && !wallCheckHit && m_Settings.dashUnlock)
            {
				int n = Random.Range(0, dashSFX.Length);
				this.GetComponent<AudioSource>().PlayOneShot(dashSFX[n]);

				dashing();
				canDash = false;
            }
		}
	}

	public void fanSet(Vector2 fanVelocity)
    {
		fanForce = fanVelocity;
		fanActive = true;
    }

	public void fanDeset()
    {
		m_Rigidbody2D.AddForce(fanForce, ForceMode2D.Force);
		fanForce = Vector2.zero;
		fanActive = false;
    }

	public void stopVelocity()
	{
		PlayerAnim.SetBool("isWalking", false);
		m_Rigidbody2D.velocity = Vector3.zero;
	}

	public void endLevel()
	{
		PlayerAnim.SetTrigger("Fade");
		Invoke("transition", 1.5f);
	}

	void transition()
	{
		GameObject.Find("UI").GetComponent<Animator>().SetTrigger("endLevel");
		Invoke("toLevelMenu", 1);
	}

	void toLevelMenu()
    {
		SceneManager.LoadScene("Level Menu");
	}

	public void Flip(bool truth)
	{
		Vector3 theScale = transform.localScale;
		if (truth)
		{
			if (theScale.x < 0)
				theScale.x *= -1;
			transform.localScale = theScale;
			m_FacingRight = true;
		}
		else if (!truth)
		{
			if (theScale.x > 0)
				theScale.x *= -1;
			transform.localScale = theScale;
			m_FacingRight = false;
		}
	}

	public void gravFlip()
    {
		m_Rigidbody2D.gravityScale = -m_Rigidbody2D.gravityScale;
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 3)
		{
 			jumpCounter = 0;

			newCollision = collision.gameObject;
			if (!m_Grounded && prevCollision != null && newCollision == prevCollision)
			{
				jumpCounter++;
			}
		}
	}

    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.layer == 3)
		{
			prevCollision = newCollision;
			newCollision = null;
		}
    }
}
