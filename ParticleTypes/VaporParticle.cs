using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class VaporParticle : Particle
    {
        private float energyLossFactor = 0.8f;
        private bool canCondense;
        private bool willCondense;

        public VaporParticle(int x, int y) : base(x, y)
        {
            Velocity = -0.2f; // Initial upward movement
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            // Apply gravity (or anti-gravity for upward movement)
            Velocity -= gravity * 0.1f;

            // Check if condensation conditions are met
            CondenseFromCooling(grid);
            canCondense = CheckAltitude();
            willCondense = RainFactor();

            if (canCondense && willCondense)
            {
                MakeWater(grid);
                return;
            }

            // Calculate new Y position based on velocity
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

            // Handle movement up if the space above is empty
            if (grid[X, newY] == null)
            {
                grid[X, Y] = null;
                Y = newY;
                grid[X, Y] = this;
            }
            else if (grid[X, newY] is VaporParticle)
            {
                // Clump with other vapor particles
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

        private void MakeWater(Particle[,] grid)
        {
            // Replace the VaporParticle with a WaterParticle
            grid[X, Y] = new WaterParticle(X, Y);
        }

        private bool CheckAltitude()
        {
            // If the VaporParticle is in the upper 25% of the screen, it can condense
            return Y <= Game1.gridHeight * 0.25;
        }

        private bool RainFactor()
        {
            // 10% chance for the VaporParticle to condense into a WaterParticle
            Random random = new Random();
            return random.Next(0, 100) < 10;
        }

        private void CondenseFromCooling(Particle[,] grid)
        {
            // Check whether there is a cooling particle nearby
            Particle[] particlesNear = GetSurroundingParticles(grid);

            foreach (Particle particle in particlesNear)
            {
                if (particle != null && particle.isCold)
                {
                    MakeWater(grid);
                    return;
                }
            }
        }

        public override void MoveSelf(Particle[,] grid, int newX, int newY) { return; }
    }
}
