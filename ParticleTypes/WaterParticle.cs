using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;

namespace FallingSand.ParticleTypes
{
    public class WaterParticle : Particle
    {
        public WaterParticle(int x, int y) : base(x, y)
        {
            Velocity = 0.1f;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            Velocity += gravity * 0.5f;
            int newY = (int)(Y + Velocity);
            int oldY = (int)(Y - Velocity); // Trying to figure out how to get an above tile

            if (newY >= Game1.gridHeight)
                newY = Game1.gridHeight - 1;

            // Check all surrounding spaces for FireParticle
            bool fireNearby = IsFireNearby(grid);

            if (fireNearby)
            {
                // Emit SmokeParticle and delete WaterParticle
                grid[X, Y] = new SmokeParticle(X, Y);
                return; // Exit early since the WaterParticle is deleted
            }

            // The below logic is necessary for water's interactions with other particles
            // Note that grid[X, Y] is reference to the existing water particle
            // Note that grid[X, newY] is reference to the particle below
            // This will continuously check below it to determine particle type, and how to react
            if (newY < Game1.gridHeight)
            {
                // If the below space is empty, move down to occupy the space
                if (grid[X, newY] == null)
                {
                    grid[X, Y] = null;
                    Y = newY;
                    grid[X, Y] = this;
                }

                // If the below space is a SandParticle, transform it into a WetSandParticle and delete the water
                else if (grid[X, newY] is SandParticle)
                {
                    grid[X, newY] = new WetSandParticle(X, newY);
                    grid[X, Y] = null;
                }

                // If the below space is a FireParticle, emit SmokeParticle and delete WaterParticle
                else if (grid[X, newY] is FireParticle)
                {
                    for (int index = 0; index < 1000; index++) { grid[X, Y] = new SmokeParticle(X, Y); }
                    
                    grid[X, Y] = null;
                }

                // If the below space is a LavaParticle, emit smoke and stone, and delete LavaParticle
                // The deletion of the WaterParticle is handled by LavaParticle.CheckAndHandleWaterInteraction()
                else if (grid[X, newY] is LavaParticle)
                {
                    // Emit SmokeParticle
                    grid[X, Y] = new SmokeParticle(X, Y);

                    // Emit StoneParticle
                    grid[X, Y] = new StoneParticle(X, Y);

                    // Delete LavaParticle -> Delete WaterParticle
                    grid[X, newY] = null; 
                }
                else
                {
                    Random rand = new Random();
                    int direction = rand.Next(0, 2) * 2 - 1;

                    // Check if the water can slide diagonally down-left or down-right
                    if (X + direction >= 0 && X + direction < Game1.gridWidth && Y + 1 < Game1.gridHeight)
                    {
                        if (grid[X + direction, Y + 1] == null)
                        {
                            grid[X, Y] = null;
                            X += direction;
                            Y += 1;
                            grid[X, Y] = this;
                        }
                        else if (grid[X + direction, Y + 1] is SandParticle)
                        {
                            grid[X + direction, Y + 1] = new WetSandParticle(X + direction, Y + 1);
                            grid[X, Y] = null;
                        }
                        else if (grid[X + direction, Y + 1] is FireParticle)
                        {
                            grid[X, Y] = new SmokeParticle(X, Y);
                            grid[X + direction, Y + 1] = null; // Remove the fire particle
                        }
                        else if (grid[X + direction, Y] == null)
                        {
                            grid[X, Y] = null;
                            X += direction;
                            grid[X, Y] = this;
                        }
                    }
                }
            }
        }

        // Check if there is a FireParticle Left, Right, Above, or Below
        private bool IsFireNearby(Particle[,] grid)
        {
            Particle[] particlesNear = GetSurroundingParticles(grid);
            bool fireNearby = false;

            foreach (Particle particle in particlesNear)
            {
                if (particle != null && particle.isHot) 
                {
                    fireNearby = true;
                    return fireNearby;
                }
            }
            return fireNearby;
        }
    }
}
