# All Roads Lead to Cordyceps

## Game Overview
You are trying to cook a lovely meal, but the kitchen is a wreck! Search for the missing ingredients while something tries to make you their own meal.

A first-person survival cooking game where you must gather ingredients and complete recipes while avoiding a dangerous entity lurking in the kitchen.

## Links
- **MVP Recording**: [Watch on YouTube](https://youtu.be/epUgD7ljUZ](https://youtu.be/qb26KB0L2tU)
- **Draft Game**: [Play on Unity](https://play.unity.com/api/v1/games/game/de9a3fed-991a-4ca0-aea7-e39f6276eb67/build/latest/frame)

## Gameplay Features
- **First-Person Exploration**: Navigate a kitchen environment
- **Item Interaction System**: Pick up and place ingredients with visual feedback
- **Cooking Mechanics**: Combine ingredients in a wok to create dishes
- **Recipe System**: Complete dishes by gathering the required ingredients
- **Stealth Mechanics**: Crouch to move quietly and avoid detection
- **Enemy AI**: A patrolling entity that hunts the player when detected
- **Dynamic Highlighting**: Objects glow when you can interact with them

## Controls
- **WASD** - Move
- **Mouse** - Look around
- **E** - Interact (pick up items, place in wok, eat dish)
- **Left Shift** - Crouch/Stealth mode

## Technical Implementation

### Core Systems (C#)
- **Player Controller**: First-person movement with camera controls and stealth mechanics
- **Item Interaction System**: Raycast-based detection with object highlighting using outline shaders
- **Cooking System**: Recipe tracking with ingredient validation and dish completion
- **Inventory Management**: Dynamic item holding with proper positioning and physics
- **Enemy AI**: Patrol system with chase behavior and player detection
- **Collision System**: Game over triggers and scene management
- **Respawn System**: Ingredient reset functionality after dish consumption

## Assets & Tools
- **Engine**: Unity
- **Language**: C#
- **3D Modeling**: Blender
- **Outline System**: Cakeslice Outline Shader
- **Physics**: Unity Character Controller and Rigidbody

## Development Status
**Current Version**: Draft  
**Status**: In Development

---
## Credits:  

Tutorials
https://www.youtube.com/watch?v=AmGSEH7QcDg&list=PLzDRvYVwl53v2AvoQ8qGnAD2cXQIbZIsS
https://youtu.be/MxTz8vNY7XE?si=cak9hQpy_SYwhssi
https://youtu.be/yIkDdE-utjA?si=GLfZL2VDImfc5rea
https://youtu.be/WKTZgf7ZDGs?si=qLcvmdG0M30BGTLQ
https://youtu.be/YyFx5HDbgSU?si=m3TIMsUyMfsfanYg
https://youtu.be/AOgl-MRi26A?si=kXT_gP22MQ1OWQ4X
https://youtu.be/5ycmDpYen-4?si=SNrCSGlbMUlrygMq

*Outline Effect: José
Guerreiro*
*Music: Vincent Teoule*
*Entity Sound Effect: Michael Barker*
*Footsteps Sound Effect: Damien Combe*
*Roar Sound Effect: King Balthizar*
*Kitchen Sound Effect: Arion Digital*


*Made with Unity*
