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
        const float gravity = 0.1f;
        Random rand = new Random();
        string currentParticleType = "Sand"; // Default particle type

        // Define palette area
        Rectangle sandButton, waterButton, wetSandButton, fireButton, lavaButton, stoneButton;
        Color sandColor = Color.Yellow;
        Color waterColor = Color.Blue;
        Color wetSandColor = new Color(139, 69, 19); // Brown
        Color fireColor = Color.Red;
        Color lavaColor = Color.Orange;
        Color stoneColor = new Color(82, 38, 32); // Maroonish

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the font for labels
            font = Content.Load<SpriteFont>("DefaultFont"); // Ensure you have this font file in your Content

            // Other content loading code...
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
                else
                {
                    // Handle dropping multiple particles into the grid
                    int mouseX = mouseState.X / cellSize;
                    int mouseY = mouseState.Y / cellSize;

                    for (int i = 0; i < 40; i++)
                    {
                        int offsetX = rand.Next(-5, 6);
                        int offsetY = rand.Next(-5, 6);

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
                            }
                        }
                    }
                }
            }

            // First pass: Update water particles
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

            // Second pass: Update all other particles
            for (int y = gridHeight - 1; y >= 0; y--)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (grid[x, y] != null && !(grid[x, y] is WaterParticle)) // Avoid re-updating water particles
                    {
                        grid[x, y].Update(gravity, grid);
                    }
                }
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            // Draw particles
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    if (grid[x, y] != null)
                    {
                        Color particleColor;

                        if (grid[x, y] is SandParticle)
                        {
                            particleColor = sandColor;
                        }
                        else if (grid[x, y] is WaterParticle)
                        {
                            particleColor = waterColor;
                        }
                        else if (grid[x, y] is WetSandParticle)
                        {
                            particleColor = wetSandColor;
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
                            particleColor = stoneColor; // Gray color for stone
                        }
                        else if (grid[x, y] is SmokeParticle)
                        {
                            particleColor = Color.Gray; // Gray color for smoke
                        }
                        else
                        {
                            particleColor = Color.White; // Default color, in case you add other particles
                        }

                        _spriteBatch.Draw(pixel, new Rectangle(x * cellSize, y * cellSize, cellSize, cellSize), particleColor);
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

            // Draw the labels under each button using SpriteBatch.DrawString
            if (font != null)
            {
                _spriteBatch.DrawString(font, "Sand", new Vector2(sandButton.X, sandButton.Y + sandButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Water", new Vector2(waterButton.X, waterButton.Y + waterButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Wet Sand", new Vector2(wetSandButton.X, wetSandButton.Y + wetSandButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Fire", new Vector2(fireButton.X, fireButton.Y + fireButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Lava", new Vector2(lavaButton.X, lavaButton.Y + lavaButton.Height + 5), Color.White);
                _spriteBatch.DrawString(font, "Stone", new Vector2(stoneButton.X, stoneButton.Y + stoneButton.Height + 5), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
