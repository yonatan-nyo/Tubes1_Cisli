using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

using System.Drawing;
using System.IO;
using Microsoft.Extensions.Configuration;
using System;

public class CisliCrewLv10 : Bot
{
    int turnCounter;

    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("CisliCrewLv10.json");

        var config = builder.Build();
        var botInfo = BotInfo.FromConfiguration(config);

        new CisliCrewLv10(botInfo).Start();
    }

    private CisliCrewLv10(BotInfo botInfo) : base(botInfo) { }

    public override void Run()
    {
        BodyColor = Color.Black;
        TurretColor = Color.Black;
        RadarColor = Color.Black;
        BulletColor = Color.Black;
        ScanColor = Color.Black;

        do
        {
            if (RadarTurnRemaining == 0.0)
                SetTurnGunRight(double.PositiveInfinity);
            if (turnCounter % 40 == 0)
            {
                TargetSpeed = 6;
            }
            else if (turnCounter % 40 == 20)
            {
                TargetSpeed = -6;
            }
            turnCounter++;
            Go();
        } while (true);
    }

    private double getFirePower(double x, double y)
    {
        double distance = DistanceTo(x, y);
        if (Energy < 10)
        {
            return 0.1;
        }
        if (distance < 100)
        {
            return 3;
        }
        if (distance < 200)
        {
            return 2.5;
        }
        else if (distance < 300)
        {
            return 2;
        }
        else if (distance < 400)
        {
            return 1.5;
        }
        else if (distance < 500)
        {
            return 1;
        }
        else
        {
            return 0.5;
        }
    }
    public override void OnScannedBot(ScannedBotEvent e)
    {
        //Gun lockon to closter target
        double angleBodyToEnemy = (360 + Direction - DirectionTo(e.X, e.Y)) % 360;
        double angleToEnemy = Direction + BearingTo(e.X, e.Y);
        double radarTurn = NormalizeRelativeAngle(angleToEnemy - RadarDirection);

        if (radarTurn < 0)
            radarTurn -= 10;
        else
            radarTurn += 10;

        SetTurnGunLeft(radarTurn);
        double bulletPower = getFirePower(e.X, e.Y);
        Fire(bulletPower);

        //Strafing
        if (angleBodyToEnemy < 360 && angleBodyToEnemy > 275)
        {
            TurnRate = -2;
        }
        else if (angleBodyToEnemy < 265 && angleBodyToEnemy > 180)
        {
            TurnRate = 2;
        }
        else if (angleBodyToEnemy < 180 && angleBodyToEnemy > 95)
        {
            TurnRate = -2;
        }
        else if (angleBodyToEnemy < 85 && angleBodyToEnemy > 0)
        {
            TurnRate = 2;
        }
        else
        {
            TurnRate = 0;
        }
    }
}