using Microsoft.Xna.Framework;
using System;

namespace FallingSand.ParticleTypes
{
    public class SandParticle : Particle
    {
        public SandParticle(int x, int y) : base(x, y)
        {
            Velocity = 0f;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            Velocity += gravity * 1.0f;
            int newY = (int)(Y + Velocity);

            if (newY >= Game1.gridHeight)
                newY = Game1.gridHeight - 1;

            if (newY < Game1.gridHeight)
            {
                MoveSelf(grid, X, Y + 1);
            }
        }

        public override void MoveSelf(Particle[,] grid, int newX, int newY)
        {
            // Boundary check to ensure we're not going out of grid bounds
            if (newX < 0 || newX >= grid.GetLength(0) || newY < 0 || newY >= grid.GetLength(1))
            {
                return;
            }

            // Take our particles nearby [left, right, above, below]
            Particle[] particlesNear = GetSurroundingParticles(grid);

            // Isolate our left, right, and below
            Particle particleLeft = particlesNear[0];
            Particle particleRight = particlesNear[1];
            Particle particleBelow = particlesNear[3];

            // Check the space directly below
            if (particleBelow == null)
            {
                MoveDown(grid, X, Y + 1);
            }

            else if (particleBelow is WaterParticle)
            {
                MakeWetSand(grid);
            }

            // Check if the particle can move diagonally down-right
            else if (particleRight == null && grid[X + 1, Y + 1] == null)
            {
                MoveDownRight(grid);
            }

            // Check if the particle can move diagonally down-left
            else if (particleLeft == null && grid[X - 1, Y + 1] == null)
            {
                MoveDownLeft(grid);
            }

            // Otherwise, the particle should remain in place
            else
            {
                return;
            }
        }

        private void MakeWetSand(Particle[,] grid)
        {
            // Create a new WetSandParticle
            grid[X, Y] = null;
            grid[X, Y] = new WetSandParticle(X, Y);
        }

        private void MoveDown(Particle[,] grid, int newX, int newY)
        {
            grid[newX, newY] = this;
            grid[X, Y] = null;
            X = newX;
            Y = newY;
        }

        private void MoveDownRight(Particle[,] grid)
        {
            grid[X, Y] = null;
            grid[X + 1, Y + 1] = this;
            X++;
            Y++;
        }

        private void MoveDownLeft(Particle[,] grid)
        {
            grid[X, Y] = null;
            grid[X - 1, Y + 1] = this;
            X--;
            Y++;
        }
    }
}
