## Description
    This is a falling sand simulation similar to those of the past created using C# and MonoGame.
    To access the MGCB Editor, cd into the project directory (Where Game1.cs is located) and enter: mgcb-editor via Command Line.

## Particle Types

    Particle - The abstract class all other particles inherit from.
        Attributes:
            X           - The X position of the particle
            Y           - The Y position of the particle
            Velocity    - The velocity of the particle

        Methods:
            Update      - Takes gravity(float) and grid(2-dimensional Particle array) as arguments to Update the Particles Position


    FireParticle - A static-to-cursor, temporary 'bundle' of particles emitted via pressing or holding left-click on the mouse
        - Turns WaterParticles into SmokeParticles and deletes existing WaterParticle
        - Not affected by gravity as it 'follows' the pointer while lef-click is held

        Methods:
            - EmitSmokeParticles - Takes the current grid(2-dimensional Particle array), x(int), and y(int) as arguments
                                 - Emits 10 SmokeParticles when called
                                 - Called when FireParticle is interacting with a WaterParticle 
                                 - returns void
    

    LavaParticle - A dynamic 'bundle' of particles emitted via pressing or holding left-click on the mouse
        - Turns WaterParticles into SmokeParticles, deletes existing WaterParticle, itself, and Emits a StoneParticle
        - Affected by gravity

        Methods:
            - CheckAndHandleWaterInteraction - Takes grid(2-dimensional Particle array), x(int), and y(int) as arguments
                                             - First checks if a WaterParticle is interacting with it
                                             - Deletes WaterParticle
                                             - Spawns 10 SmokeParticles
                                             - returns bool

    SandParticle - A dynamic 'bundle' or particles emitted via pressing or holding left-click on the mouse
        - Default particle
        - Affected by gravity
        - Turns into WetSandParticle when WaterParticles interact it

    SmokeParticle - A dynamic 'bundle' or particles emitted via pressing or holding left-click on the mouse
        - Created when FireParticles, or LavaParticles interact WaterParticles
        - Affected by gravity inversely

    StoneParticle - A static-once-placed, temporary 'bundle' of particles emitted via pressing or holding left-click on the mouse
        - Created when WaterParticles interact with LavaParticles
        - Not affected by gravity

    WaterParticle - A dynamic, highly-reactive 'bundle' or particles emitted via pressing or holding left-click on the mouse
        - Interaction with FireParticles emit SmokeParticles and deletes self
        - Interaction with LavaParticles emit SmokeParticles, StoneParticles and deletes self
        - Interaction with SandParticles emit WetSandParticles, and deletes self
        - Affected by gravity

    WetSandParticle - A dynamic 'bundle' or particles emitted via pressing or holding left-click on the mouse
        - Created when WaterParticles and SandParticles interact
        - Affected by gravity
