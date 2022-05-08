# Procedural Generation Methods
 Showcase of Procedural Generation Methods utilizing Unity and C#

## Included Generation Methods
### Drunkard's Wander
- Pick a random point on a grid.
- Choose a random cardinal direction.
- Move in that direction, and generate a floor.
- Repeat steps 2-3, until you have generated as many floors as desired.

### Diffusion Limited Aggregation
- Choose a random cardinal direction
- Generate a ray within the axis bounds of currently generated floors
- When the ray hits a floor, generate another floor 1 unit closer to the ray origin
- Update axis bounds if necessary
