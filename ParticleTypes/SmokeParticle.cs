using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class SmokeParticle : Particle
    {
        public SmokeParticle(int x, int y) : base(x, y)
        {
            Velocity = -0.8f; // Slow down the rise
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            // Apply the velocity to move the smoke upwards
            int newY = (int)(Y + Velocity);

            // Ensure smoke doesn't move out of bounds
            if (newY < 0)
            {
                newY = 0; // Clamp to the top of the screen
            }

            // If the position above is empty, move the smoke particle up
            if (newY >= 0 && grid[X, newY] == null)
            {
                grid[X, Y] = null;
                Y = newY;
                grid[X, Y] = this;
            }
            else if (newY >= 0 && grid[X, newY] is SmokeParticle)
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
            Velocity *= 0.99f;
        }
    }
}
