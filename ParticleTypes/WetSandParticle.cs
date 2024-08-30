using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class WetSandParticle : Particle
    {
        public WetSandParticle(int x, int y) : base(x, y)
        {
            Velocity = 0f;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            // Apply gravity to the velocity, but slower than dry sand
            Velocity += gravity * 0.5f;
            int newY = (int)(Y + Velocity);

            if (newY >= Game1.gridHeight)
                newY = Game1.gridHeight - 1;

            // Ensure the wet sand sinks properly
            MoveSelf(grid, X, newY);
        }

        public override void MoveSelf(Particle[,] grid, int newX, int newY)
        {
            // Boundary check to ensure we're not going out of grid bounds
            if (newX < 0 || newX >= grid.GetLength(0) || newY < 0 || newY >= grid.GetLength(1))
            {
                return;
            }

            // Check if the position directly below is empty or contains water
            if (grid[X, newY] == null || grid[X, newY] is WaterParticle)
            {
                // If moving into water, swap positions
                if (grid[X, newY] is WaterParticle)
                {
                    Particle temp = grid[X, newY];
                    grid[X, newY] = this;
                    grid[X, Y] = temp;
                    temp.X = X;
                    temp.Y = Y;
                }
                else
                {
                    // Just move down if it's empty
                    grid[X, Y] = null;
                    Y = newY;
                    grid[X, Y] = this;
                }
            }
            else
            {
                // If directly below is not empty, try to slide diagonally
                AttemptDiagonalSlide(grid);
            }
        }

        private void AttemptDiagonalSlide(Particle[,] grid)
        {
            Random rand = new Random();
            int direction = rand.Next(0, 2) * 2 - 1; // Randomly pick -1 (left) or 1 (right)

            int newX = X + direction;
            int newY = Y + 1;

            if (newX >= 0 && newX < Game1.gridWidth && newY < Game1.gridHeight)
            {
                if (grid[newX, newY] == null || grid[newX, newY] is WaterParticle)
                {
                    if (grid[newX, newY] is WaterParticle)
                    {
                        Particle temp = grid[newX, newY];
                        grid[newX, newY] = this;
                        grid[X, Y] = temp;
                        temp.X = X;
                        temp.Y = Y;
                    }
                    else
                    {
                        grid[X, Y] = null;
                    }

                    X = newX;
                    Y = newY;
                    grid[X, Y] = this;
                }
            }
        }
    }
}
