using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

using System.Drawing;
using System.IO;
using Microsoft.Extensions.Configuration;
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
            SetTurnLeft(90);
            Forward(100);
            TurnGunLeft(10);
        }
    }

    private double getFirePower(double x, double y){
        double distance = DistanceTo(x, y);
        if(Energy < 10){
            return 0.1;
        }
        if(distance <10){
            return 50;
        }
        if (distance < 200){
            return 3;
        } else if (distance < 300){
            return 2.5;
        }  else if (distance < 400){
            return 2;
        }else if (distance < 500){
            return 1.5;
        } else {
            return 1;
        }
    }
    
    private double getAngleAdjustMent(double bearingFromGun){
        if(Energy < 10){
            return 0.1;
        }
        if (bearingFromGun < 45){
            return 0;
        } else if (bearingFromGun < 90){
            return 10;
        } else if (bearingFromGun < 145){
            return 25;
        } else{
            return 37;
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        double bulletPower = getFirePower(e.X, e.Y);
        Fire(bulletPower);
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
        double angleAdjustment=getAngleAdjustMent(bearingFromGun);

        if (bearingFromGun <= 0)
        {
            TurnGunRight(Math.Abs(bearingFromGun)+angleAdjustment);
        }
        else
        {
            TurnGunLeft(Math.Abs(bearingFromGun)+angleAdjustment);
        }
        double bulletPower = getFirePower(e.Bullet.X, e.Bullet.Y);
        Fire(bulletPower);
    }
}