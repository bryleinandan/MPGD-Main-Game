# Credits
Skybox - https://assetstore.unity.com/packages/2d/textures-materials/sky/stellar-sky-99558

# General systems / debugging

## Inventory slots
Item is a scriptable object.
InventoryManager throws a bunch of InventorySlot objects into arrays.

if you're getting index out of bounds.
- have you disabled a piece of the inventory system by accident? do not set inactive
- unity editor might be doing weird things in its editor to overwrite it 

## Enemy
Uses NavMesh AI to set new position targets based off of player's position at max awareness.

If enemy is not moving:
- make sure player (or any other targets) are on target layer. and that player has been assigned to be its target in script.
- make sure any obstacles in the enemy's radius are on obstruction layer if you want them to... obstruct view.
- make sure navmesh is BAKED into the terrain - the nav mesh agent won't work without it

Can't see FOV whilst editing:
- make sure you're in scene editor > gizmos on


## Item interaction

"Item" as a class refers to the inventory item / the scriptable object that's used in the inventory system.
"overworldItem" is the class with a physical model.

The scriptable object is the container for each item.
	in order to be seen in the world, please assign an overworld item object to it!
	
	overworldItem inherits from IInteractable interface:
	anything that can be interacted with must be on the correct layer! (Interactable)
	there must be an iinteractable.cs script on that object!

manually overriding the label position must be done in the global inspector and not the prefab one

## Camera fixes
make sure playercam is on the movement script on player
there is one line in fixed update that reroutes the movement through the camera's position.
it can be commented out to revert to the original _MovePlayer but be aware this movement does not account for camera orientation

## health
Health System encompasses
- health class (encompasses the number)
- health bar class (does the bar stuff)

Player Health System is just the player's health system
it's a separate prefab with one different script but still uses most of health system

HealthSystemController just links the healthbar to the player
that means if you get the player object as a target, you can call .TakeDamage() on the player
(because the health bar is on the ui)


## Implementing IAttack
define WaitForCooldown with an invoke and cooldown