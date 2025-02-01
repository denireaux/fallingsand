using Microsoft.Xna.Framework;
using System;
using System.Threading;

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
            Velocity += gravity * 1.0f;
            int newY = (int)(Y + Velocity);

            if (newY >= Game1.gridHeight)
                newY = Game1.gridHeight - 1;

            if (newY < Game1.gridHeight)
            {
                MoveSelf(grid, X, Y + 1);
            }
        }

        private void HandleVerticalMovement(Particle[,] grid, int newY)
        {
            if (newY < 0 || newY >= Game1.gridHeight) return;

            Particle belowParticle = grid[X, newY];

            if (belowParticle == null)
            {
                MoveParticle(grid, X, newY);
            }
            else if (belowParticle is SandParticle)
            {
                grid[X, newY] = new WetSandParticle(X, newY);
                grid[X, Y] = null;
            }
            else if (belowParticle is FireParticle)
            {
                EmitSmoke(grid);
            }
            else if (belowParticle is LavaParticle)
            {
                grid[X, newY] = new StoneParticle(X, newY);
                EmitSmoke(grid);
            }
        }

        private void HandleLateralMovement(Particle[,] grid)
        {
            Random rand = new Random();
            int direction = rand.Next(0, 2) * 2 - 1;

            if (X + direction >= 0 && X + direction < Game1.gridWidth && Y + 1 < Game1.gridHeight)
            {
                Particle diagonalParticle = grid[X + direction, Y + 1];
                Particle sideParticle = grid[X + direction, Y];

                if (diagonalParticle == null)
                {
                    MoveParticle(grid, X + direction, Y + 1);
                }
                else if (diagonalParticle is SandParticle)
                {
                    grid[X + direction, Y + 1] = new WetSandParticle(X + direction, Y + 1);
                    grid[X, Y] = null;
                }
                else if (diagonalParticle is FireParticle)
                {
                    EmitSmoke(grid);
                    grid[X + direction, Y + 1] = null;
                }
                else if (sideParticle == null)
                {
                    MoveParticle(grid, X + direction, Y);
                }
            }
        }

        private bool HeatDetected(Particle[,] grid)
        {
            Particle[] particlesNear = GetSurroundingParticles(grid);

            foreach (Particle particle in particlesNear)
            {
                if (particle != null && particle.isHot)
                {
                    return true;
                }
            }
            return false;
        }

        private void MakeSmoke(Particle[,] grid)
        {
            grid[X, Y] = new VaporParticle(X, Y);
        }

        private void MoveParticle(Particle[,] grid, int newX, int newY)
        {
            if (newX < 0 || newX >= Game1.gridWidth || newY < 0 || newY >= Game1.gridHeight) return;

            grid[X, Y] = null;
            X = newX;
            Y = newY;
            grid[X, Y] = this;
        }

        private void EmitSmoke(Particle[,] grid)
        {
            grid[X, Y] = new VaporParticle(X, Y);
        }

        public override void MoveSelf(Particle[,] grid, int newX, int newY) 
        {
            if (newX < 0 || newX >= grid.GetLength(0) || newY < 0 || newY >= grid.GetLength(1))
            {
                return;
            }

            Particle[] particlesNear = GetSurroundingParticles(grid);
            Particle particleLeft = particlesNear[0];
            Particle particleRight = particlesNear[1];
            Particle particleBelow = particlesNear[3];

            bool ShouldMoveLeft = WaterParticle.GenerateBoolean();

            // default behavior
            if (particleBelow == null) { MoveDown(grid, X, Y + 1); }

            else if (particleLeft == null && X - 1 >= 0 && grid[X - 1, Y + 1] == null && ShouldMoveLeft) 
            { 
                MoveDown(grid, X, Y + 1);
                MoveLeft(grid);
            }

            else if (particleRight == null && X + 1 < Game1.gridWidth && grid[X + 1, Y + 1] == null) 
            { 
                MoveDown(grid, X, Y + 1);
                MoveRight(grid);
            }

            else if (particleBelow is WaterParticle && particleLeft == null) 
            {
                MoveLeft(grid); 
                if (particleLeft == null) { MoveLeft(grid); }
            }
            else if (particleBelow is WaterParticle && particleRight == null) 
            {
                MoveRight(grid); 
                if (particleRight == null) { MoveRight(grid); }
            }
        /* else if (particleBelow is SandParticle) { MakeWetSand(grid); } */
            else { return; }
        }

        private void MakeWetSand(Particle[,] grid)
        {
            grid[X, Y] = null;
            grid[X, Y] = new WetSandParticle(X, Y);
        }

        private void MoveDown(Particle[,] grid, int newX, int newY)
        {
            if (newX >= 0 && newX < Game1.gridWidth && newY >= 0 && newY < Game1.gridHeight)
            {
                grid[newX, newY] = this;
                grid[X, Y] = null;
                X = newX;
                Y = newY;
            }
        }

        private void MoveDownRight(Particle[,] grid)
        {
            if (X + 1 < Game1.gridWidth && Y + 1 < Game1.gridHeight)
            {
                grid[X, Y] = null;
                grid[X + 1, Y + 1] = this;
                X++;
                Y++;
            }
        }

        private void MoveDownLeft(Particle[,] grid)
        {
            if (X - 1 >= 0 && Y + 1 < Game1.gridHeight)
            {
                grid[X, Y] = null;
                grid[X - 1, Y + 1] = this;
                X--;
                Y++;
            }
        }

        private void MoveRight(Particle[,] grid)
        {
            if (X + 1 < Game1.gridWidth)
            {
                grid[X, Y] = null;
                grid[X + 1, Y] = this;
                X++;
            }
        }

        private void MoveLeft(Particle[,] grid)
        {
            if (X - 1 >= 0)
            {
                grid[X, Y] = null;
                grid[X - 1, Y] = this;
                X--;
            }
        }

        private static bool GenerateBoolean() 
        {
            var random = new Random();
            int randomNumber = random.Next(10);

            if (randomNumber <= 5) { return true; }
            return false;
        }
    }
}
