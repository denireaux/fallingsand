using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class FireParticle : Particle
    {
        private int lifetime;

        public FireParticle(int x, int y) : base(x, y)
        {
            lifetime = 100; // Fire particles last for a limited time
            Velocity = -0.2f; // Fire particles rise slightly
            isHot = true;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            lifetime--;
            if (lifetime <= 0)
            {
                grid[X, Y] = null; // Remove the particle when its lifetime ends
                return;
            }

            int newY = (int)(Y + Velocity);

            // Ensure fire doesn't move out of bounds
            if (newY < 0)
            {
                grid[X, Y] = null;
                return;
            }

            // Check if fire comes into contact with water
            if (Y + 1 < Game1.gridHeight && (grid[X, Y + 1] is WaterParticle || (newY >= 0 && grid[X, newY] is WaterParticle)))
            {
                // Remove both fire and water particles
                EmitSmokeParticles(grid, X, Y + 1);
                grid[X, Y] = null;
                return;
            }

            // Move fire upwards or spread left/right if blocked
            if (grid[X, newY] == null)
            {
                grid[X, Y] = null;
                Y = newY;
                grid[X, Y] = this;
            }
            else
            {
                Random rand = new Random();
                int direction = rand.Next(0, 2) * 2 - 1;

                if (X + direction >= 0 && X + direction < Game1.gridWidth && grid[X + direction, Y] == null)
                {
                    grid[X, Y] = null;
                    X += direction;
                    grid[X, Y] = this;
                }
            }
        }

        private void EmitSmokeParticles(Particle[,] grid, int x, int y)
        {
            Random rand = new Random();

            
            int offsetX = rand.Next(-2, 3); // Random offset to spread smoke
            int offsetY = rand.Next(-2, 3); // Random offset to spread smoke

            int smokeX = x + offsetX;
            int smokeY = y + offsetY;

            if (smokeX >= 0 && smokeX < Game1.gridWidth && smokeY >= 0 && smokeY < Game1.gridHeight)
            {
                if (grid[smokeX, smokeY] == null)
                {
                    grid[smokeX, smokeY] = new VaporParticle(smokeX, smokeY);
                }
            }
            
        }
        public override void MoveSelf(Particle[,] grid, int newX, int newY) {}
    }
}
