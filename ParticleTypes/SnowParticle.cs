using Microsoft.Xna.Framework;
using System;
using System.Threading;

namespace FallingSand.ParticleTypes
{
    public class SnowParticle : Particle
    {
        public SnowParticle(int x, int y) : base(x, y)
        {
            Velocity = 0f;
        }

        public override void Update(float gravity, Particle[,] grid) 
        {
            // Check if particle should melt
            bool shouldMelt = WarmSurroundings(grid);

            // Melt the particle if it should
            if (shouldMelt) { Melt(grid); }

            // Apply gravity very minimally or not at all
            Velocity += gravity * 0.1f; // Snow falls very slowly
            int newY = (int)(Y + Velocity);

            if (newY >= Game1.gridHeight)
                newY = Game1.gridHeight - 1;

            if (newY < Game1.gridHeight)
            {
                if (grid[X, newY] == null)
                {
                    // Move down if empty
                    grid[X, Y] = null;
                    Y = newY;
                    grid[X, Y] = this;
                }
                else if (grid[X, newY] is WaterParticle)
                {
                    // Snow should not sink, so don't swap positions
                    Velocity = 0; // Reset velocity to prevent further downward movement
                }
                else
                {
                    // Try sliding diagonally
                    Random rand = new Random();
                    int direction = rand.Next(0, 2) * 2 - 1;

                    if (X + direction >= 0 && X + direction < Game1.gridWidth && Y + 1 < Game1.gridHeight)
                    {
                        if (grid[X + direction, Y + 1] == null)
                        {
                            grid[X, Y] = null;
                            X += direction;
                            Y += 1;
                            grid[X, Y] = this;
                        }
                        else if (grid[X + direction, Y + 1] is WaterParticle)
                        {
                            // Snow should sit on top of water, so stop further movement
                            Velocity = 0; // Reset velocity
                        }
                    }
                }
            }
        }

        public bool WarmSurroundings(Particle[,] grid)
        {
            Particle[] particlesNearby = GetSurroundingParticles(grid);
            foreach (Particle particle in particlesNearby)
            {
                if (particle != null && particle.isHot) { return true; }
            }
            return false;
        }

        public void Melt(Particle[,] grid)
        {
            // Remove the snow particle when it melts
            grid[X, Y] = null; 

            // Create WaterParticle
            for (int index = 1; index < 10; index++) 
            { 
                grid[X, Y] = new WaterParticle(X, Y); 
            }
            

            return;
        }
    }
}
