using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	[Header("General")]
	public CharacterController2D controller;

	public float runSpeed = 40f;

	public float horizontalMove = 0f;
	public bool jump = false;
	public bool dash = false;
	public bool crouch = false;
	public bool hold = false;
	public bool downwardDash = false;

	public Color colorA = new Color32(230, 230, 230, 255);

	public bool hasKey = false;
	[SerializeField] private bool areDead = false;

	[Header("Respawn")]
	public Vector3 spawnPoint;
	public int deathCount = 0;
	public bool canMove = true;

	public key _key;
	public GameObject items;
	public GameObject hazards;
	private Transform[] objects;
	public CinemachineVirtualCamera startCam;
	public CinemachineBrain camBrain;

	[Header("UI")]
	public Image reset;
	//public SpriteRenderer background;

	public TextMeshProUGUI visDeathCounter;
	public string levelPlayerPref;
	public GameObject pauseMenu;


	public Transform[] breakables;
	public Transform[] moveables;
	public Transform[] hanging;

	//[Header("Audio")]
	AudioClip deathSFX;

	private void Awake()
    {
		//Setting item refreshes and spawn point
		pauseMenu.SetActive(false);

		objects = items.GetComponentsInChildren<Transform>();
		spawnPoint = this.transform.position;
	}

    void Update()
	{
		// This will be for an eventual pause menu we still  to build
		if (Input.GetKeyDown("escape") || Input.GetKeyDown("joystick button 7"))
        {
			//SceneManager.LoadScene("Level Menu");
			if (pauseMenu.activeSelf == true)
			{
				pauseMenu.SetActive(false);
				pauseMenu.GetComponent<pauseMenu>().mainPauseMenu.SetActive(true);
				pauseMenu.GetComponent<pauseMenu>().settingsMenu.SetActive(false);

				Time.timeScale = 1f;
			}
			else
			{
				pauseMenu.SetActive(true);
				Time.timeScale = 0f;
			}
		
		}

		if (canMove)
		{
			//movement, but it's handled in CharacterController2D.cs
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

			if(Input.GetAxisRaw("Vertical") < 0)
            {
				crouch = true;
			}

			if (Input.GetKeyDown(controller.m_Settings.jump))
			{
				jump = true;
			}
			else if (Input.GetKeyDown(controller.m_Settings.dash))
            {
				dash = true;
            }

			if (Input.GetKeyDown(controller.m_Settings.hold))
            {
				hold = true;
            }
			else if (Input.GetKeyUp(controller.m_Settings.hold))
            {
				hold = false;
            }
		}
	}

	void FixedUpdate ()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash, crouch, hold);

		jump = false;
		dash = false;
		crouch = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		//Running all of player collision from this script. These are all generally pretty self-explanatory based off of their tags
		//It might not be a bad idea to redo this in the future but for now, this works.

		if (collision.tag == "Death")
		{
			if (!controller.m_Settings.invincibility || (controller.m_Settings.invincibility && collision.GetComponent<deathObject>().typeOfObject == 0))
			{
				if (!areDead)
				{
					areDead = true;
					canMove = false;
					this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
					deathSFX = collision.GetComponent<deathObject>().sendDeathAudio();
					Invoke("onDeath", 0.4f);
					//onDeath();
				}
			}
		}

		else if (collision.tag == "camSwap")
        {
			collision.gameObject.GetComponent<cameraSwitch>().setEnterDirection(transform);
        }

		else if (collision.tag == "Key")
        {
			hasKey = true;
			collision.gameObject.transform.parent = transform;
			collision.gameObject.GetComponent<key>().following = true;
			collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}

		else if (collision.tag == "DoubleJump")
        {
			controller.doubleJump = true;
			collision.gameObject.SetActive(false);
        }

		else if (collision.tag == "Checkpoint")
        {
			//change checkpoint image (probably going to be an animation)
			if (collision.GetComponent<checkpoint>().checkpointActive == false)
			{
				spawnPoint = collision.gameObject.transform.position;
				collision.GetComponent<checkpoint>().setCheckpoint();
				startCam = collision.GetComponent<checkpoint>().vcam;
			}

        }

		else if (collision.tag == "Spring")
		{
			controller.m_Rigidbody2D.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
			controller.canDash = true;
		}
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Breakable")
        {
			if (controller._dashing && controller.m_Settings.downSmashUnlock)
				collision.gameObject.SetActive(false);
        }

		if (collision.gameObject.tag == "MovingPlatform" || (collision.transform.parent && collision.transform.parent.tag == "MovingPlatform"))
        {
			this.transform.parent = collision.transform.parent;
        }

		if (collision.gameObject.tag == "Door")
		{
			if (!hasKey)
				collision.gameObject.GetComponent<Door>().locked();
			else
			{
				this.GetComponentInChildren<key>().speed = 8;
				controller.stopVelocity();
				this.GetComponentInChildren<key>().followSpot = collision.transform;
				this.GetComponentInChildren<key>().startDoor(collision.gameObject.GetComponent<Door>());
				this.GetComponentInChildren<key>().resetParent();
			}
		}

		if(collision.gameObject.tag == "Minecart" && controller._dashing)
        {
			collision.gameObject.GetComponent<minecart>().enter();
			canMove = false;
        }

        if (controller._dashing && collision.gameObject.GetComponent<possessionObject>())
        {
			collision.gameObject.GetComponent<possessionObject>().setUp();
		}
	}

    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.tag == "MovingPlatform" || (collision.transform.parent && collision.transform.parent.tag == "MovingPlatform"))
		{
			this.transform.parent = null;
		}
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.tag == "camSwap")
		{
			collision.gameObject.GetComponent<cameraSwitch>().checkCamSwap(transform);
		}
	}

    private void resetBoard(GameObject board)
    {
		foreach (BoxCollider2D collider in board.GetComponents<BoxCollider2D>())
			collider.enabled = true;
	}

    public void setDouble()
    {
		controller.doubleJump = true;
    }

    public void onDeath()
    {
		//death has become overly complicated but the short version is this. Play the sound, add to the count, reset
		controller.deadSFX(deathSFX);
		deathCount += 1;
		Invoke("resetLevel", 0f);
	}

	public void itemReset()
    {
		//Reseting the items. This is run every death and every time the player switches cameras
		foreach (Transform item in objects)
			item.gameObject.SetActive(true);
	}

	private void resetLevel()
    {
		//Resets gravity if needed
		if (this.GetComponent<Rigidbody2D>().gravityScale < 0)
		{
			controller.gravFlip();
			this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}

		//Resets key if needed
		if (this.GetComponentInChildren<key>())
		{
			this.GetComponentInChildren<key>().resetKey();
			hasKey = false;
		}

		//Resets all breakable objects
		foreach (Transform platform in breakables)
			platform.gameObject.SetActive(true);

		while (hazards.transform.childCount > 0)
		{
			DestroyImmediate(hazards.transform.GetChild(0).gameObject);
		}

		//resets hanging boxes
		downwardDash = false;
		foreach (Transform box in hanging)
		{
			box.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			box.GetComponent<pushableObject>().frozen = true;
			box.GetComponent<pushableObject>().moveBack();
		}

		//Resets normal boxes
		foreach (Transform box in moveables)
			box.GetComponent<pushableObject>().moveBack();

		itemReset();

		//Move the camera back
		camBrain.m_DefaultBlend.m_Time = 0.05f;
		camBrain.m_DefaultBlend.m_Time = 1f;

		Invoke("setCharacter", 0.1f);

		if (controller.m_Rigidbody2D.gravityScale < 0)
			controller.m_Rigidbody2D.gravityScale = -controller.m_Rigidbody2D.gravityScale;

		//Move character to spawn point
		this.transform.position = FindObjectOfType<groupLoading>().returnCheckpoint()	;
		areDead = false;
	}

	void setCharacter()
    {
		//Reenable movement
		canMove = true;
	}

}
