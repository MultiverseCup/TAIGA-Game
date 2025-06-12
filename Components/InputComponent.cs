using gameProject.Components;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InputComponent
{
    private SpeedComponent _speed;
    private PositionComponent _position;

    public InputComponent(SpeedComponent speed, PositionComponent position)
    {
        _speed = speed;
        _position = position;
    }

    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        Vector2 movement = Vector2.Zero;

        if (keyboardState.IsKeyDown(Keys.W)) movement.Y -= 1;
        if (keyboardState.IsKeyDown(Keys.S)) movement.Y += 1;
        if (keyboardState.IsKeyDown(Keys.A)) movement.X -= 1;
        if (keyboardState.IsKeyDown(Keys.D)) movement.X += 1;

        if (movement != Vector2.Zero)
            movement.Normalize();

        _position.Position += movement * _speed.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}
