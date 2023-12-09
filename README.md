# Project Fish Tank

### Student Info

-   Name: Julia Rissberger
-   Section: 5

## Simulation Design
A fishtank with two different types of fish. One type tends to flock together, and must be fed by the user. The other type tends to avoid
others of its kind, and will occasionally chase away other fish that get too close.

### Controls

- Left mouse click
      -Places food at the position of the mouse. The fish will seek and eat the food when they're hungry.

## Small Fish

A small, peaceful fish. It sticks together with others, and must be 
fed by the user.

### Wander and flock
The fish wander around, avoiding obstacles and generally staying near other small fish.

#### Steering Behaviors

- Wander
- Cohesion
- Staying in bounds
- Obstacles - Avoids seaweed and rocks
- Seperation - Separates from other small fish
   
#### State Transistions

-This is the default state
-The fish will be in this state until it hasn't eaten in a 
certain amount of time.
   
## Starving

This state occurs when the fish hasn't eaten in a long time. 
Its movement is slowed, it no longer follows other fish, and 
it prioritizes finding food.

#### Steering Behaviors

- Wander
- Seek (Food)
- Staying in bounds
- Obstacles - Avoids seaweed and rocks
- Seperation - Separates from other small fish
   
#### State Transistions
When the timer tracking how long since the fish has eaten reaches a certain amount, it enters this state. 
It exits the state once it's eaten.

## Jellyfish
This jellyfish avoids others of its kind. It wanders around, and isn't interested in the 
food for the smaller fish. Every so often, if a small fish is close enough, it will chase 
the small fish for a short amount of time.

### Wander and avoid
The jellyfish wanders the tank, avoiding obstacles and other jellyfish.

#### Steering Behaviors

- Wander
- Stays in bounds
- Obstacles - avoids seaweed and rocks
- Seperation - this agent separates from other jellyfish.
   
#### State Transistions
This is the default state, and the large fish will be in it whenever it is far away from 
any smaller fish.
   
### Chasing
A smaller fish has gotten too near to the jellyfish, and the jellyfish is 
now chasing it at a higher speed than it normally moves at. The chase continues until the 
jellyfish loses interest (chase timer runs out).

#### Steering Behaviors

- Stays in bounds
- Seek (small fish)
- Obstacles - avoids seaweed and rocks
- Seperation - separates from other jellyfish
   
#### State Transistions
This state is entered when a small fish is within a certain distace of the large fish.
If there are multiple small fish meeting this criteria, the large fish prioritizes the first one.

## Sources
I created all the assets for this project.

## Make it Your Own
Creating own assets (environment, obstacles, fish)

## Known Issues

Slight blind spot with obstacle avoidance. Most noticeable with the seaweed--when fish are between the screen edge and the seaweed they'll move right through the seaweed.

### Requirements not completed



