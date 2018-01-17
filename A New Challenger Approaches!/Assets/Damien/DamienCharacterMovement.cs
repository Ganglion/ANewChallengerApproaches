using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamienCharacterMovement : UnitInput {

    // Components
    [SerializeField]
    public Animator characterAnimator;

	// Used for animations
	//protected bool isFacingRight = true;
	//public bool IsFacingRight { get { return isFacingRight; } }

	public DathanUnitAttributes dua;

	public float jumpCooldown;
	protected float timeAfterLastJump = 999;
	[SerializeField]
	protected int currentJumps;
	protected bool isFacingRight = true;
	public bool IsFacingRight { get { return isFacingRight; } }

    [Header("New: Multiple Jump/Fly")]
    // New: Variables for flying and multiple jumping
    public SecondaryJumpType secJumpType;
    bool ignoreGravity = false;
    bool isFlying = false;

    protected override void Awake() {

        base.Awake();
		dua = GetComponent<DathanUnitAttributes> ();
        characterAnimator = transform.Find("PlayerSprite").GetComponent<Animator>();
    }

    // New: Moved DoPlayerInput to Update for more responsive controls (getkeydown works much better)
    void Update()
    {
        timeAfterLastJump += Time.deltaTime;
        DoPlayerInput(); // MOVED TO UPDATE FOR MORE RESPONSIVE CONTROLS
    }

    protected void FixedUpdate() {

		// Execute this every frame
        
    }

    protected void DoPlayerInput() {
        
		// Retrieves values from attributes every frame (as buffs/debuffs may change them)
        float movementSpeed = characterAttributes.CurrentMovementSpeed;
		float currentAcceleration = characterMovement.collisions.below ? characterAttributes.CurrentGroundAcceleration : characterAttributes.CurrentAirborneAcceleration;
        float jumpHeight = characterAttributes.CurrentJumpHeight;
        float horzInput = Input.GetAxisRaw("Horizontal");

        bool hasMovedHorizontally = false;

        if (characterAnimator)
        {
            if (horzInput != 0)
                characterAnimator.SetBool("isRunning", true);
            else
                characterAnimator.SetBool("isRunning", false);
        }

		// Character not on ground?
        if (!characterMovement.collisions.below) {

            // fall
            // Gravity is multiplied by 5 to make the jump less floaty
            if (!ignoreGravity)
                currentVelocity.y += Time.deltaTime * -9.81f * 5;
		
		// otherwise,
        } else {

			// don't fall
            currentVelocity.y = -0.01f;
            if (currentJumps > 0)
            {
                currentJumps = 0;
                timeAfterLastJump = 0f;
            }
        }

		// Pressed left arrow?
        if (Input.GetKey(KeyCode.LeftArrow)) {

            // Accelerate leftwards towards max speed
            if (currentVelocity.x >= -movementSpeed)
                currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, -movementSpeed, currentAcceleration * Time.deltaTime);
            hasMovedHorizontally = true;
            isFacingRight = false;

        }

		// Pressed right arrow?
        if (Input.GetKey(KeyCode.RightArrow)) {

            // Accelerate rightwards towards max speed
            if (currentVelocity.x <= movementSpeed)
                currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, movementSpeed, currentAcceleration * Time.deltaTime);
            hasMovedHorizontally = true;
            isFacingRight = true;
        }

        // New: Implementation of multiple jumping (Just set characterAttributes.maxJumps to 1 for it to be single jump)
		// Pressed up arrow? or Space
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) ){
            //print("Current jumps: " + currentJumps + "// Max Jumps: " + characterAttributes.maxJumps + "// timeAfterLastJump: " + timeAfterLastJump + "// jumpCooldown: " + jumpCooldown);
            // If character is on ground,
			if (currentJumps < dua.maxJumps && timeAfterLastJump > jumpCooldown) // checks if jump limit is reached and jumpcooldown
            {
                switch (secJumpType) // handles diff jump types
                {
                    case SecondaryJumpType.Flight: 
                        if (currentJumps == 0) // handles the first jump
                        {
                            currentVelocity.y += Time.deltaTime * 9.81f * 5;
                            currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f * 5); // Velocity to achieve ideal height
                            currentJumps += 1;
                            timeAfterLastJump = 0;
                        }
                        else // handles the flying
                        {
                            isFlying = true;
                        }
                        break;
                    case SecondaryJumpType.MultipleJump: // handles multiple jumps 
                        currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f * 5); // Velocity to achieve ideal height
                        currentJumps += 1;
                        timeAfterLastJump = 0;
                        break;
                }
            }
        }

		// Didn't move left or right?
        if (!hasMovedHorizontally) {

			// Decelerate towards 0 speed
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime); // Slow down
        }

        // New: Implementation of fly
        if (Input.GetKey(KeyCode.Space)) // handles holding down of space and flying
        {
            if (secJumpType == SecondaryJumpType.Flight && isFlying)
            {
                ignoreGravity = true;
				currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, dua.flySpeed, dua.flyAccel * Time.deltaTime);
                //characterAttributes.currentEnergy -= characterAttributes.flyEnergyCost * Time.deltaTime;
            }
        } else
        {
            ignoreGravity = false;
            isFlying = false;
        }

        // New: Implementation of dash/fly
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
			if (horzInput != 0 && dua.timeSinceLastDash > dua.dashCooldown)
            {
                //if (characterAttributes.currentEnergy >= characterAttributes.dashEnergyCost) // for energy implementation
                if (!characterMovement.collisions.below)
					currentVelocity.x = horzInput * dua.baseDashDistance;
                else
					currentVelocity.x += horzInput * dua.baseDashDistance;
                //characterAttributes.EnergyChange(characterAttributes.dashEnergyCost); // for energy implementation
				dua.timeSinceLastDash = 0;
            }
        }

        if (!hasMovedHorizontally || Mathf.Abs(currentVelocity.x) > movementSpeed)
        {
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentAcceleration * Time.deltaTime); // Slow down
        }

        // All velocity calculated, time to move the player
        // Pressed down button?
        if (Input.GetKey(KeyCode.DownArrow)) {

			// move player while passing through platforms
            // Use Vector2.down as the second parameter if you want to pass through platforms
            characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.down);

		// otherwise
        } else {			
			// move player normally
            characterMovement.Move(currentVelocity * Time.deltaTime, Vector2.zero);
        }

        if (isFacingRight)
        {
            Vector3 transformScale = transform.localScale;
            transformScale.x = Mathf.Abs(transform.localScale.x);
            transform.localScale = transformScale;
        }
        else
        {
            Vector3 transformScale = transform.localScale;
            transformScale.x = -Mathf.Abs(transform.localScale.x);
            transform.localScale = transformScale;
        }

    }

}

public enum SecondaryJumpType{
	MultipleJump,
	Flight
}
