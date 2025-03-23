using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class CisliCrookLv1 : Bot { 

    int turnCounter;
    private double lastNearestDistance = Double.MaxValue;
    private long lastNearestBotId = -1;
    private int scanCounter = 0;

    static void Main(string[] args)
    {
        new CisliCrookLv1().Start();
    }

    CisliCrookLv1() : base(BotInfo.FromFile("CisliCrookLv1.json")) { }

    public override void Run()
    {
        BodyColor = Color.Red;
        TurretColor = Color.White;
        GunColor = Color.White;
        RadarColor = Color.White;
        BulletColor = Color.Red;
        ScanColor = Color.Blue;

        turnCounter = 0;

        GunTurnRate = 15;

        while (IsRunning) {
            if (turnCounter % 128 == 0) {
                TurnRate = 10;
                TargetSpeed = 5;
            }
            if (turnCounter % 128 == 64) {
                TurnRate = -10;
                TargetSpeed = 5;
            }
            turnCounter++;
            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e) {
        double distance = Math.Sqrt(Math.Pow(e.X - X, 2) + Math.Pow(e.Y - Y, 2));
        double botSpeed = Math.Abs(e.Speed);
        double firePower = 0;

        if (distance < lastNearestDistance || scanCounter >= 5) {
            lastNearestDistance = distance;
            lastNearestBotId = e.ScannedBotId;
            scanCounter = 0;
        } else {
            scanCounter++;
        }
        
        if (e.ScannedBotId == lastNearestBotId) {
            if (!(distance > 500 && botSpeed > 4)) {
                if (distance < 100) {
                    firePower = 3;
                } else if (distance < 300) {
                    firePower = 2; 
                } else {
                    firePower = 1;
                }
                
                if (firePower > 0) {
                    Fire(firePower);
                }
            }
        }
    }

    public override void OnHitByBullet(HitByBulletEvent e) {
        TurnRate = 5;
    }
    public override void OnHitWall(HitWallEvent e) {
        TargetSpeed = -1 * TargetSpeed;
    }
}