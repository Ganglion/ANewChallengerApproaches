# A New Challenger Approaches Developer Guide

## Scripts

#### CharacterInput
##### Description
Handles player movement. Controls are set to up, down, left, right, arrows. **Requires ObjectMovement script.** This is an example script, you can edit and create your own script. ${CharacterName}Input.cs. 

To edit controls, edit the if statements in DoPlayerInput()
 ```csharp
     protected void DoPlayerInput() {
        //handle collision and gravity
        if (Input.GetKey(KeyCode.LeftArrow)) // Move left
        if (Input.GetKey(KeyCode.RightArrow)) // Move right
        if (Input.GetKey(KeyCode.UpArrow)) // Jump
        //handle horizontal movement
        if (Input.GetKey(KeyCode.DownArrow)) { //falling down
            // pass through platforms
        } else {
           //dont pass through platform
        }
    }
   ```

#### UnitAttributes
##### Description
Handles the attributes of attached character/unit. Handles damage calculation and changes to unit's behaviour (eg movement speed, jump height)

|Serialized Fields (editable from inspector)||
|---|---|
|Base Max Health|Max Health of the character|
|Base Movement speed|Movement speed of character|
|Time Taken To Reach Max Speed|How fast the character reaches his maximum movement speed (default is base)|
|Base Jump Height|Jump height of character|
|Base Damage Taken Factor|Multiplier for damage received. Default = 1|
|Base Damage Output Factor|Multiplier for damage dealt. Default = 1|

|Public Variables||
|---|---|
|float **CurrentHealth**|Returns current health of unit|
|float **CurrentDamageOutputFactor**|Returns nultiplier for damage dealt|
|float **CurrentMovementSpeed**|Returns current speed of unit|
|float **CurrentGroundAcceleration**|Returns current ground acceleration of unit|
|float **CurrentAirborneAcceleration**|Returns current airborne acceleration of unit|
|float **CurrentJumpHeight**|Returns current jump height of unit|
|bool **CanMove**|WIP|
|bool **CanExecuteActions**|WIP|
|float **MovementSpeedMultiplier**|Returns speed multiplier of unit|
|float **JumpHeightMultiplier**|Returns jump height multiplier of unit|
|float **DamageTakenFactorMultiplier**|Returns multiplier for damaged received|
|float **DamageOutPutFactorMultiplier**|Returns multiplier for damage dealt|
|float **DamagePerSecond**|Returns damage over time inflicted on unit|


|Public methods||
|---|---|
|void ApplyAttack(float **damagedealt**, Vector2 **point**)|Does **damageDealt** amount of damage to the character. **point** is the location for damage text to appear|
|void ApplyAttack(float **damageDealt**, Vector2 **point**, Color **damageColor**)|Does **damageDealt** amount of damage to the character. **point** is the location for damage text to appear. Provide a **damageColor** to change color of damage text|
|void ApplyAttack(float **damageDealt**, Vector2 **point**, params Buff[] **attackBuffs**)| Does **damageDealt** amount of damage to the character. **point** is the location for damage text to appear. Applys buffs in **attackBuffs** to the character|
|virtual void ApplyAttack(float **damageDealt**, Vector2 **point**, Color **damageColor**, params Buff[] **attackBuffs**)|Does **damageDealt** amount of damage to the character. **point** is the location for damage text to appear. Applys buffs in **attackBuffs** to the character. Provide a **damageColor** to change color of damage text|



#### ObjectMovement
##### Description
Handles movement of attached object (eg player, projectile, enemy, minions). Also does detection of slope, vertical and horizontal collision.
|Public methods||
|---|---|
|void Move(Vector2 **moveAmount**, bool **standingOnPlatform**)|Translates object by **moveAmount**. **standingOnPlatform** = *true* if character/enemy is on a platform|
|void Move(Vector2 **moveAmount**, Vector2 **input**, bool **standingOnPlatform** = *false*)|Translates object by **moveAmount**. **standingOnPlatform** = *true* if character/enemy is on a platform. If not provided **standingOnPlatform** = *false*. **input** is the player's input. WIP|  
|


