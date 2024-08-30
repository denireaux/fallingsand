using Microsoft.Xna.Framework;
using System;
using System.Reflection.Metadata.Ecma335;

namespace FallingSand.ParticleTypes
{
    public class WaterParticle : Particle
    {
        public WaterParticle(int x, int y) : base(x, y)
        {
            Velocity = 10.0f;
        }

        public override void Update(float gravity, Particle[,] grid)
        {
            Velocity += gravity * 0.5f;
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

            // [left, right, above, below]
            Particle[] particlesNear = GetSurroundingParticles(grid);

            Particle particleLeft = particlesNear[0];
            Particle particleRight = particlesNear[1];
            Particle particleBelow = particlesNear[3];
            Particle particleLowerleft = particlesNear[4];
            Particle particleLowerRight = particlesNear[5];

            // Check the space directly below
            if (particleBelow == null && Y + 1 < grid.GetLength(1))
            {
                MoveDown(grid, X, Y + 1);
            }

            // Check if the particle can move diagonally down-left
            // If empty space left, and empty space diagonally left
            else if (LeftInbounds(grid) && DownInbounds(grid) && particleLowerleft == null)
            {
                MoveDownLeft(grid);
            }

            // Check if the particle can move diagonally down-right
            // If empty space right, and empty space diagonally right
            else if (RightInbounds(grid) && DownInbounds(grid) && particleLowerRight == null)
            {
                MoveDownRight(grid);
            }

            // Check if it can move right
            else if (particleRight == null && RightInbounds(grid))
            {
                MoveRight(grid);
            }

            // Check if it can move left
            else if (particleLeft == null && LeftInbounds(grid))
            {
                MoveLeft(grid);
            }

            // If both left and right are free, choose a random direction
            else if (particleLeft == null && particleRight == null)
            {
                Random rand = new Random();
                int randomNumber = rand.Next(1, 11);

                if (randomNumber >= 6 && X + 1 < grid.GetLength(0)) { MoveRight(grid); }
                else if (X - 1 >= 0) { MoveLeft(grid); }
            }

            // Otherwise, the particle should remain in place
            else
            {
                return;
            }
        }

        // Checks if the cell underneath it is inbounds
        private bool DownInbounds(Particle[,] grid)
        {
            if (Y + 1 < grid.GetLength(1)) { return true; }
            return false;
        }

        // Checks if the cell right of it is inbounds
        private bool RightInbounds(Particle[,] grid)
        {
            if (X + 1 < grid.GetLength(0)) { return true; }
            return false;
        }

        // Checks if the cell left of it is inbounds
        private bool LeftInbounds(Particle[,] grid)
        {
            if (X - 1 >= 0) { return true; }
            return false;
        }

        // Moves one cell down
        private void MoveDown(Particle[,] grid, int newX, int newY)
        {
            if (newX >= 0 && newX < grid.GetLength(0) && newY >= 0 && newY < grid.GetLength(1))
            {
                grid[newX, newY] = this;
                grid[X, Y] = null;
                X = newX;
                Y = newY;
            }
        }

        // Moves one cell down, one cell right
        private void MoveDownRight(Particle[,] grid)
        {
            if (X + 1 < grid.GetLength(0) && Y + 1 < grid.GetLength(1))
            {
                grid[X, Y] = null;
                grid[X + 1, Y + 1] = this;
                X++;
                Y++;
            }
        }

        // Moves one cell right
        private void MoveRight(Particle[,] grid)
        {
            if (X + 1 < grid.GetLength(0))
            {
                grid[X, Y] = null;
                grid[X + 1, Y] = this;
                X++;
            }
        }

        // Moves on cell down, one cell left
        private void MoveDownLeft(Particle[,] grid)
        {
            if (X - 1 >= 0 && Y + 1 < grid.GetLength(1))
            {
                grid[X, Y] = null;
                grid[X - 1, Y + 1] = this;
                X--;
                Y++;
            }
        }

        // Moves one cell left
        private void MoveLeft(Particle[,] grid)
        {
            if (X - 1 >= 0)
            {
                grid[X, Y] = null;
                grid[X - 1, Y] = this;
                X--;
            }
        }

        // Moves one cell right, pause, moves one cell down
        private void SecondMoveDownRight(Particle[,] grid, int newX, int newY)
        {
            grid[newX, newY] = this;
            grid[X, Y] = null;
            X = newX;
            Y = newY;

            grid[X, Y] = null;
            grid[X + 1, Y] = this;
            X++;
        }
    }
}
