using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class LavaParticle : Particle
    {
        private int delayCounter;
        private static readonly int maxDelay = 2; // Control how often the lava moves (higher = slower movement)

        public LavaParticle(int x, int y) : base(x, y)
        {
            Velocity = 0.05f; // Slower velocity to simulate viscosity
            delayCounter = 0;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            // Apply gravity to the velocity, but with less impact due to viscosity
            Velocity += gravity * 0.5f;

            // First, check for water interactions before applying any movement logic
            if (CheckAndHandleWaterInteraction(grid, X, Y + 1) || // Below
                CheckAndHandleWaterInteraction(grid, X, Y - 1) || // Above
                CheckAndHandleWaterInteraction(grid, X + 1, Y) || // Right
                CheckAndHandleWaterInteraction(grid, X - 1, Y) || // Left
                CheckAndHandleWaterInteraction(grid, X + 1, Y + 1) || // Bottom right diagonal
                CheckAndHandleWaterInteraction(grid, X - 1, Y + 1) || // Bottom left diagonal
                CheckAndHandleWaterInteraction(grid, X + 1, Y - 1) || // Top right diagonal
                CheckAndHandleWaterInteraction(grid, X - 1, Y - 1))   // Top left diagonal
            {
                return; // If interaction occurred, stop further processing
            }

            // Delay movement to simulate viscous flow
            delayCounter++;
            if (delayCounter < maxDelay)
            {
                return; // Skip movement this frame
            }
            delayCounter = 0; // Reset delay counter

            // Movement logic for the viscous lava
            int newY = (int)(Y + Velocity);

            // Ensure lava doesn't move out of bounds
            if (newY >= Game1.gridHeight)
                newY = Game1.gridHeight - 1;

            if (newY < Game1.gridHeight && grid[X, newY] == null)
            {
                // Move lava downwards if the space is empty
                grid[X, Y] = null;
                Y = newY;
                grid[X, Y] = this;
            }
            else
            {
                // Try sliding sideways with a lower probability
                Random rand = new Random();
                int direction = rand.Next(0, 3) - 1; // -1, 0, or 1 (0 is no movement)

                // Check if the sideways move is within bounds and if the space is empty
                if (X + direction >= 0 && X + direction < Game1.gridWidth && Y + 1 < Game1.gridHeight)
                {
                    if (grid[X + direction, Y + 1] == null)
                    {
                        grid[X, Y] = null;
                        X += direction;
                        Y += 1;
                        grid[X, Y] = this;
                    }
                }
            }
        }

        private bool CheckAndHandleWaterInteraction(Particle[,] grid, int x, int y)
        {
            // Check if the coordinates are within bounds and if there's a water particle
            if (x >= 0 && x < Game1.gridWidth && y >= 0 && y < Game1.gridHeight)
            {
                if (grid[x, y] is WaterParticle)
                {
                    grid[x, y] = null; // Remove the water particle
                    grid[X, Y] = new StoneParticle(X, Y); // Turn lava into stone
                    return true; // Indicate that an interaction occurred
                }
            }
            return false;
        }
    }
}
