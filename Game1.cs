using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FallingSand.ParticleTypes;

namespace FallingSand
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public const int gridWidth = 800;
        public const int gridHeight = 600;
        const int cellSize = 2;
        Particle[,] grid = new Particle[gridWidth, gridHeight]; // Unified grid for all particles
        Texture2D pixel;
        SpriteFont font; // SpriteFont for text labels
        const float gravity = 0.8f;
        Random rand = new Random();
        string currentParticleType = "Sand"; // Default particle type

        // Define palette area
        Rectangle sandButton, waterButton, wetSandButton, fireButton, lavaButton, stoneButton, vaporButton, soilButton, heaterButton, coolerButton;
        Color sandColor = Color.Yellow;
        Color waterColor = Color.Blue;
        Color wetSandColor = new Color(169, 132, 46); // Brown
        Color fireColor = Color.Red;
        Color lavaColor = new Color(253, 83, 21);
        Color stoneColor = new Color(191, 191, 191); // Ashy
        Color vaporColor = Color.White;
        Color soilColor = new Color(62, 49, 23); // Dark Brown
        Color heaterColor = new Color(255, 165, 0); // Orange
        Color coolerColor = new Color(0, 162, 232); // Cyan

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = gridWidth * cellSize;
            _graphics.PreferredBackBufferHeight = gridHeight * cellSize + 80; // Extra space for palette and labels
        }

        protected override void Initialize()
        {
            // Create a 1x1 white pixel texture for drawing particles
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            // Initialize palette buttons -> Rectangle(X-Coordinate, Y-Coordinate)
            sandButton = new Rectangle(10, gridHeight * cellSize + 10, 50, 30);
            waterButton = new Rectangle(110, gridHeight * cellSize + 10, 50, 30);
            wetSandButton = new Rectangle(210, gridHeight * cellSize + 10, 50, 30);
            fireButton = new Rectangle(310, gridHeight * cellSize + 10, 50, 30);
            lavaButton = new Rectangle(410, gridHeight * cellSize + 10, 50, 30);
            stoneButton = new Rectangle(510, gridHeight * cellSize + 10, 50, 30);
            vaporButton = new Rectangle(610, gridHeight * cellSize + 10, 50, 30);
            soilButton = new Rectangle(710, gridHeight * cellSize + 10, 50, 30);
            heaterButton = new Rectangle(810, gridHeight * cellSize + 10, 50, 30);
            coolerButton = new Rectangle(910, gridHeight * cellSize + 10, 50, 30);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the font for labels
            font = Content.Load<SpriteFont>("DefaultFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                // Check if the click is within the palette area
                if (sandButton.Contains(mouseState.Position))
                {
                    currentParticleType = "Sand";
                }
                else if (waterButton.Contains(mouseState.Position))
                {
                    currentParticleType = "Water";
                }
                else if (wetSandButton.Contains(mouseState.Position))
                {
                    currentParticleType = "WetSand";
                }
                else if (fireButton.Contains(mouseState.Position))
                {
                    currentParticleType = "Fire";
                }
                else if (lavaButton.Contains(mouseState.Position))
                {
                    currentParticleType = "Lava";
                }
                else if (stoneButton.Contains(mouseState.Position))
                {
                    currentParticleType = "Stone";
                }
                else if (vaporButton.Contains(mouseState.Position))
                {
                    currentParticleType = "Vapor";
                }
                else if (soilButton.Contains(mouseState.Position))
                {
                    currentParticleType = "Soil";
                }
                else if (heaterButton.Contains(mouseState.Position))
                {
                    currentParticleType = "Heater";
                }
                else if (coolerButton.Contains(mouseState.Position))
                {
                    currentParticleType = "Cooler";
                }
                else
                {
                    // Handle dropping multiple particles into the grid
                    int mouseX = mouseState.X / cellSize;
                    int mouseY = mouseState.Y / cellSize;

                    for (int offsetX = -5; offsetX <= 5; offsetX++)
                    {
                        for (int offsetY = -5; offsetY <= 5; offsetY++)
                        {
                            int particleX = mouseX + offsetX;
                            int particleY = mouseY + offsetY;

                            if (particleX >= 0 && particleX < gridWidth && particleY >= 0 && particleY < gridHeight)
                            {
                                if (grid[particleX, particleY] == null)
                                {
                                    if (currentParticleType == "Sand")
                                    {
                                        grid[particleX, particleY] = new SandParticle(particleX, particleY);
                                    }
                                    else if (currentParticleType == "Water")
                                    {
                                        grid[particleX, particleY] = new WaterParticle(particleX, particleY);
                                    }
                                    else if (currentParticleType == "WetSand")
                                    {
                                        grid[particleX, particleY] = new WetSandParticle(particleX, particleY);
                                    }
                                    else if (currentParticleType == "Fire")
                                    {
                                        grid[particleX, particleY] = new FireParticle(particleX, particleY);
                                    }
                                    else if (currentParticleType == "Lava")
                                    {
                                        grid[particleX, particleY] = new LavaParticle(particleX, particleY);
                                    }
                                    else if (currentParticleType == "Stone")
                                    {
                                        grid[particleX, particleY] = new StoneParticle(particleX, particleY);
                                    }
                                    else if (currentParticleType == "Vapor")
                                    {
                                        grid[particleX, particleY] = new VaporParticle(particleX, particleY);
                                    }
                                    else if (currentParticleType == "Soil")
                                    {
                                        grid[particleX, particleY] = new SoilParticle(particleX, particleY);
                                    }
                                    else if (currentParticleType == "Heater")
                                    {
                                        grid[particleX, particleY] = new HeaterParticle(particleX, particleY);
                                    }
                                    else if (currentParticleType == "Cooler")
                                    {
                                        grid[particleX, particleY] = new CoolerParticle(particleX, particleY);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // First pass: Update smoke particles (top-down to prioritize upward movement)
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (grid[x, y] is VaporParticle)
                    {
                        grid[x, y].Update(gravity, grid);
                    }
                }
            }

            // Second pass: Update water particles (bottom-up to allow falling)
            for (int y = gridHeight - 1; y >= 0; y--)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (grid[x, y] is WaterParticle)
                    {
                        grid[x, y].Update(gravity, grid);
                    }
                }
            }

            // Third pass: Update all other particles (bottom-up to maintain existing behavior)
            for (int y = gridHeight - 1; y >= 0; y--)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (grid[x, y] != null && !(grid[x, y] is WaterParticle || grid[x, y] is VaporParticle)) // Avoid re-updating
                    {
                        grid[x, y].Update(gravity, grid);
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color backgroundColor = new Color(0, 0, 0);
            GraphicsDevice.Clear(backgroundColor);

            _spriteBatch.Begin();

            // Draw particles
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    if (grid[x, y] != null)
                    {
                        Color particleColor;
                        int particleSize = cellSize;

                        if (grid[x, y] is SandParticle)
                        {
                            particleColor = sandColor;
                        }
                        else if (grid[x, y] is WaterParticle)
                        {
                            particleColor = waterColor;
                        }

                        // Wet sand has a large particle size to simulate 'clumping'
                        else if (grid[x, y] is WetSandParticle)
                        {
                            particleColor = wetSandColor;
                            particleSize = cellSize * 2;
                        }

                        else if (grid[x, y] is FireParticle)
                        {
                            particleColor = fireColor;
                        }
                        else if (grid[x, y] is LavaParticle)
                        {
                            particleColor = lavaColor;
                        }
                        else if (grid[x, y] is StoneParticle)
                        {
                            particleColor = stoneColor;
                        }
                        else if (grid[x, y] is VaporParticle)
                        {
                            particleColor = vaporColor;
                            particleSize = cellSize * 2;
                        }
                        else if (grid[x, y] is SoilParticle)
                        {
                            particleColor = soilColor;
                            particleSize = cellSize * 2;
                        }
                        else if (grid[x, y] is HeaterParticle)
                        {
                            particleColor = heaterColor;
                        }
                        else if (grid[x, y] is CoolerParticle)
                        {
                            particleColor = coolerColor;
                        }
                        else
                        {
                            particleColor = Color.White;
                        }
                        _spriteBatch.Draw(pixel, new Rectangle(x * cellSize, y * cellSize, particleSize, particleSize), particleColor);
                    }
                }
            }

            // Draw the palette
            _spriteBatch.Draw(pixel, sandButton, sandColor);
            _spriteBatch.Draw(pixel, waterButton, waterColor);
            _spriteBatch.Draw(pixel, wetSandButton, wetSandColor);
            _spriteBatch.Draw(pixel, fireButton, fireColor);
            _spriteBatch.Draw(pixel, lavaButton, lavaColor);
            _spriteBatch.Draw(pixel, stoneButton, stoneColor);
            _spriteBatch.Draw(pixel, vaporButton, vaporColor);
            _spriteBatch.Draw(pixel, soilButton, soilColor);
            _spriteBatch.Draw(pixel, heaterButton, heaterColor);
            _spriteBatch.Draw(pixel, coolerButton, coolerColor);

            // Draw the labels under each button using SpriteBatch.DrawString
            if (font != null)
            {
                _spriteBatch.DrawString(font, "Sand", new Vector2(sandButton.X, sandButton.Y + sandButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Water", new Vector2(waterButton.X, waterButton.Y + waterButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Wet Sand", new Vector2(wetSandButton.X, wetSandButton.Y + wetSandButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Fire", new Vector2(fireButton.X, fireButton.Y + fireButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Lava", new Vector2(lavaButton.X, lavaButton.Y + lavaButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Stone", new Vector2(stoneButton.X, stoneButton.Y + stoneButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Vapor", new Vector2(vaporButton.X, vaporButton.Y + vaporButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Soil", new Vector2(soilButton.X, soilButton.Y + soilButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Heater", new Vector2(heaterButton.X, heaterButton.Y + heaterButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Cooler", new Vector2(coolerButton.X, coolerButton.Y + coolerButton.Height + 5), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
