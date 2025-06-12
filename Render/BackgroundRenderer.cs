using gameProject.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BackgroundRenderer
{
    private Texture2D _tileTexture;
    private int _tileSize;

    public BackgroundRenderer(Texture2D tileTexture, int tileSize)
    {
        _tileTexture = tileTexture;
        _tileSize = tileSize;
    }

    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        // границы камеры
        float left = camera.Position.X - camera.ViewportWidth / 2 / camera.Zoom;
        float top = camera.Position.Y - camera.ViewportHeight / 2 / camera.Zoom;
        float right = left + camera.ViewportWidth / camera.Zoom;
        float bottom = top + camera.ViewportHeight / camera.Zoom;

        // диапазон тайлов для отрисовки
        int startX = (int)Math.Floor(left / _tileSize);
        int startY = (int)Math.Floor(top / _tileSize);
        int endX = (int)Math.Ceiling(right / _tileSize);
        int endY = (int)Math.Ceiling(bottom / _tileSize);

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                Vector2 position = new Vector2(x * _tileSize, y * _tileSize);
                spriteBatch.Draw(_tileTexture, position, Color.White);
            }
        }
    }
}
