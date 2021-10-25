﻿using System;
using System.Collections.Generic;
using System.Text;
using Math_Library;
using Raylib_cs;

namespace Math_For_Games
{
    class Enemy : Character
    {
        private Actor _actorToChase;
        private float _maxFov;
        public static int EnemyCount;
        private float _timeBetweenShots;
        private float _cooldownTime;

        public Enemy(char icon, float x, float y, Color color, float speed, int health, Actor actor, float maxFov, Vector2 forward, float cooldownTime, string name = "Enemy")
            : base(icon, x, y, color, speed, health, name)
        {
            _actorToChase = actor;
            _maxFov = maxFov;
            EnemyCount++;
            Forward = forward;
            Tag = ActorTag.ENEMY;
            _cooldownTime = cooldownTime;
        }

        public override void Update(float deltaTime)
        {
            _timeBetweenShots += deltaTime;

            //The Enemy runs towards the player's position
            if (_actorToChase == null)
                return;
            Vector2 moveDirection = _actorToChase.Position - Position;

            //The enemy runs away from the player's position
            //Vector2 moveDirection = Position - _actorToChase.Position;

            Velocity = moveDirection.Normalized * Speed * deltaTime;

            if(IsTargetInSight())
                Position += Velocity;
            else if (_timeBetweenShots >= _cooldownTime)
            {
                Vector2 directionOfBullet = _actorToChase.Position - Position;

                _timeBetweenShots = 0;
                Bullet bullet = new Bullet('*', Position, Color.LIME, 200, "Enemy Bullet", directionOfBullet.Normalized.X, directionOfBullet.Normalized.Y, this);
                AABBCollider bulletCollider = new AABBCollider(20, 20, bullet);
                bullet.Collider = bulletCollider;
                Engine.CurrentScene.AddActor(bullet);
            }

            base.Update(deltaTime);
        }

        public bool IsTargetInSight()
        {
            Vector2 directionOfTarget = (_actorToChase.Position - Position).Normalized;
            float distanceOfTarget = Vector2.GetDistance(_actorToChase.Position, Position);

            return (Math.Acos(Vector2.GetDotProduct(directionOfTarget, Forward)) * 180/Math.PI) < _maxFov
                && distanceOfTarget < 200;
        }

        public void TakeDamage()
        {
            Health--;
        }

        public override void OnCollision(Actor actor)
        { }

        public override void Draw()
        {
            base.Draw();
            //if (Collider != null)
                //Collider.Draw();
        }
    }
}
