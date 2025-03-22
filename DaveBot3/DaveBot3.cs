using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

using System.Drawing;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Collections.Generic;
using System;

public class DaveBot3 : Bot
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("DaveBot3.json");

        var config = builder.Build();
        var botInfo = BotInfo.FromConfiguration(config);

        new DaveBot3(botInfo).Start();
    }

    private DaveBot3(BotInfo botInfo) : base(botInfo) {}

    public override void Run()
    {
        BodyColor = Color.Blue;
        TurretColor = Color.Blue;
        RadarColor = Color.Black;
        ScanColor = Color.Yellow;

        while (IsRunning)
        {
            SetTurnLeft(100);
            Forward(100);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        var distance = DistanceTo(e.X, e.Y);
        if (distance > 500)
        {
            Fire(1);
        }
        else if (distance > 300)
        {
            Fire(2);
        }
        else
        {
            Fire(3);
        }
    }

    public override void OnHitBot(HitBotEvent e)
    {
        var bearing = BearingTo(e.X, e.Y);
        if (bearing > -10 && bearing < 10)
        {
            var distance = DistanceTo(e.X, e.Y);
            if (distance > 500)
            {
                Fire(1);
            }
            else if (distance > 300)
            {
                Fire(2);
            }
            else
            {
                Fire(3);
            }
        }
        if (e.IsRammed)
        {
            TurnLeft(10);
        }
    }

        public override void OnBulletHit(BulletHitBotEvent e)
    {
        var bearingFromGun = GunBearingTo(e.Bullet.X, e.Bullet.Y);
        var distance = DistanceTo(e.Bullet.X, e.Bullet.Y);
        if (bearingFromGun <= 0)
        {
            TurnGunRight(Math.Abs(bearingFromGun));
        }
        else
        {
            TurnGunLeft(Math.Abs(bearingFromGun));
        }

        if (distance > 500)
        {
            Fire(1);
        }
        else if (distance > 300)
        {
            Fire(2);
        }
        else
        {
            Fire(3);
        }
    }
}