using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Entities;

public class Explosion
{
    private AnimatedSprite _sprite;
    private Vector2 _position;
    private TimeSpan _time = TimeSpan.Zero;
    private TimeSpan _maxtime;

    public bool IsFinished { get; private set; } = false;

    public Explosion(AnimatedSprite sprite, Vector2 position) {
        _sprite = sprite;
        _position = position;
        _maxtime = _sprite.Animation.Frames.Count * _sprite.Animation.Delay;
    }

    public void Update(GameTime gameTime)
    {
        _sprite.UpdateAnimetion(gameTime);

        _time += gameTime.ElapsedGameTime;

        if (_time >= _maxtime)
            IsFinished = true;
    }

    public void Draw()
    {
        _sprite.Draw(Core.SpriteBatch, _position);
    }
}