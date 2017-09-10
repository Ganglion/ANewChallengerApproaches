# A New Challenger Approaches Developer Guide

## User Guide [WIP]
1. Download ZIP from the repo/ clone repo
2. Start Unity and Open Project
3. Begin your journey


## Creating a Character with movement
### Scripts required:
1. 2D Collider (Box Collider 2D, Capsule Collider 2D)
2. UnitAttributes
    * Ensure that jump height, movement speed, max health and time taken to reach max speed is not 0 in the inspector
3. ObjectMovement
    * Collision mask set to structure
    * Make sure any platforms/wall/flooring/ceiling is in the structure layer
    * Any platforms' tag is through if you want a unit to jump on in
    * Player is under player layer
4. CharacterInput

### Adding Example Linear Projectile Firing to Character

1. Add Firing Projectile Skill Example script to character
    * Ensure Projectile Life Span is greater than 0 in the inspector
2. Drag ExampleLinearProjectile prefab onto "Projectile" on the Firing Projectile Skill Example script Component 
3. Create a firing indicator
    1. Create empty gameObject and rename to firingIndicator
    2. Drag firing indicator onto character in the hierachy. This will se firing indicator as a child of the character
    3. Set the x position of the firingIndicator to be 1 (or greater than width of your character) in the inspector
4. Drag firingIndicator gameObject onto "firing indicator" on the Firing Projectile Skill Example script Component
5. Drag your character's gameObject onto "firing pivotr" on the Firing Projectile Skill Example script Component

## Scripts
### CharacterInput
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

### UnitAttributes
##### Description
Handles the attributes of attached character/unit. Handles damage calculation and changes to unit's behaviour (eg movement speed, jump height)

|Serialized Fields (editable from inspector)||
|---|---|
|float **Base Max Health**|Max Health of the character|
|float **Base Movement speed**|Movement speed of character|
|float **Time Taken To Reach Max Speed**|How fast the character reaches his maximum movement speed (default is base)|
|float **Base Jump Height**|Jump height of character|
|float **Base Damage Taken Factor**|Multiplier for damage received. Default = 1|
|float **Base Damage Output Factor**|Multiplier for damage dealt. Default = 1|

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



### ObjectMovement
##### Description
Handles movement of attached object (eg player, projectile, enemy, minions). Also does detection of slope, vertical and horizontal collision.

|Serialized Fields (editable from inspector)||
|---|---|
|float maxSlopeAngle|Maximum slope angle the object can climb|

|Public methods||
|---|---|
|void Move(Vector2 **moveAmount**, bool **standingOnPlatform**)|Translates object by **moveAmount**. **standingOnPlatform** = *true* if character/enemy is on a platform|
|void Move(Vector2 **moveAmount**, Vector2 **input**, bool **standingOnPlatform** = *false*)|Translates object by **moveAmount**. **standingOnPlatform** = *true* if character/enemy is on a platform. If not provided **standingOnPlatform** = *false*. **input** is the player's input. WIP|

## Projectile
##### Description
Script for basic projectiles. Handles collision detection and movement.

|Serialized Fields (editable from inspector)||
|---|---|
|gameObject **projectileHitEffect**|Effect when projectile hits enemy|
|bool **faceTowardsMovement**|if true, object will rotate to face direction of its motion|
|bool **facingDirectionOffset**| WIP, default = 0|
#### You can override these methods
|Virtual methods ||
|---|---|
|void OnProjectileDeath()|Destroys projectile|
|void OnTriggerEnter2D(Collider2D **other**)|triggers when projectile hits any **gameObject** with 2D Trigger Collider. [Unity Docs](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html)|
|void OnHitEnemy(GameObject **hitObject**)|triggers when projectile hits **hitObject** that is on the **Enemy** layer. Default implementation is to instantiate **projectileHitEffect** and **ApplyAttack()** on enemy's **UnitAttributes** component. **OnProjectileDeath()** will be triggered as well|
|void OnHitDestructible(GameObject **hitObject**)|triggers when projectile hits **hitObject** that is on the **Destructible** layer. Default implementation is same as OnHitEnemy|
|void OnHitStructure(GameObject **hitObject**)|triggers when projectile hits **hitObject** that is on the **Structure** layer. Default implementation is to trigger **OnProjectileDeath()**|
|void OnHitPlayer(GameObject **hitObject**)|triggers when projectile hits **hitObject** that is on the **Player** layer. Default implementation is do nothing|
|virtual void MoveProjectile()| WIP|

#### Example of writing your own projectile script
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach this script to your new projectile prefab
public class FriendlyFireProjectile : Projectile {

    //set up projectile to work with FiringProjectileSkillExample.cs
    public void SetupProjectile(float damage, float speed, float lifespan, Vector2 direction, params Buff[] buffs) {
        projectileDamage = damage;
        projectileSpeed = speed;
        projectileLifespan = lifespan;
        projectileBuffs = buffs;
        unitProjectileDirection = direction.normalized;
    }

    protected override void OnHitPlayer(gameObject hitObject) {
          if (projectileHitEffect != null) {
            Instantiate(projectileHitEffect, transform.position, Quaternion.Euler(Vector3.zero));
        }
        hitObject.GetComponent<UnitAttributes>().ApplyAttack(projectileDamage, transform.position, projectileBuffs);
        OnProjectileDeath();
    }

}
```



