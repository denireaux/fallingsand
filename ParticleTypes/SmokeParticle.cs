using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class SmokeParticle : Particle
    {
        private float energyLossFactor = 0.8f; // Factor by which velocity is reduced upon bouncing

        public SmokeParticle(int x, int y) : base(x, y)
        {
            Velocity = -0.2f; // Very slow upward movement
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            // Apply the velocity to move the smoke upwards
            int newY = (int)(Y + Velocity);

            // Handle boundary collision for the Y-axis (top and bottom boundaries)
            if (newY < 0)
            {
                newY = 0; // Clamp to the top of the screen
                Velocity = -Velocity * energyLossFactor; // Invert velocity and reduce it
            }
            else if (newY >= Game1.gridHeight)
            {
                newY = Game1.gridHeight - 1; // Clamp to the bottom of the screen
                Velocity = -Velocity * energyLossFactor; // Invert velocity and reduce it
            }

            // Handle boundary collision for the X-axis (left and right boundaries)
            if (X <= 0)
            {
                X = 0; // Clamp to the left boundary
                Velocity = -Velocity * energyLossFactor; // Invert velocity and reduce it
            }
            else if (X >= Game1.gridWidth - 1)
            {
                X = Game1.gridWidth - 1; // Clamp to the right boundary
                Velocity = -Velocity * energyLossFactor; // Invert velocity and reduce it
            }

            // If the position above is empty, move the smoke particle up
            if (grid[X, newY] == null)
            {
                grid[X, Y] = null;
                Y = newY;
                grid[X, Y] = this;
            }
            else if (grid[X, newY] is SmokeParticle)
            {
                // Clump with other smoke particles
                Random rand = new Random();
                int direction = rand.Next(0, 2) * 2 - 1;

                // Try to move left or right if blocked
                if (X + direction >= 0 && X + direction < Game1.gridWidth && grid[X + direction, Y] == null)
                {
                    grid[X, Y] = null;
                    X += direction;
                    grid[X, Y] = this;
                }
                else if (X - direction >= 0 && X - direction < Game1.gridWidth && grid[X - direction, Y] == null)
                {
                    grid[X, Y] = null;
                    X -= direction;
                    grid[X, Y] = this;
                }
            }

            // Slightly reduce the velocity to slow down over time
            Velocity *= 0.999f;
        }
    }
}
