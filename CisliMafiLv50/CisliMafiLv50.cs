using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class CisliMafiLv50 : Bot
{
    int turnDirection = 1; 
    int turnCounter = 0;  

    static void Main(string[] args)
    {
        new CisliMafiLv50().Start();
    }

    CisliMafiLv50() : base(BotInfo.FromFile("CisliMafiLv50.json")) { }

    public override void Run()
    {   
        BodyColor = Color.Red;
        GunColor = Color.Orange;
        RadarColor = Color.Yellow;
        BulletColor = Color.Green;
        ScanColor = Color.Blue;
        TracksColor = Color.Indigo;

        turnCounter = 0;
        
        do
        {
            if (RadarTurnRemaining == 0.0)
                SetTurnRadarRight(double.PositiveInfinity);
            
            if (turnCounter % 128 == 0) {
                TurnRate = 10;
                TargetSpeed = 5;
            }
            else if (turnCounter % 128 == 64) {
                TurnRate = -10;
                TargetSpeed = 5;
            }
            MaxSpeed = 8;
            turnCounter++;
            Go();
        } while (true);
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        int firePower;
        double distance = DistanceTo(e.X, e.Y);
        double angleToEnemy = Direction + BearingTo(e.X,e.Y);
        double radarTurn = NormalizeRelativeAngle(angleToEnemy - RadarDirection);
        
        double extraTurn = 5;
        
        if (radarTurn < 0)
            radarTurn -= extraTurn;
        else
            radarTurn += extraTurn;
        
        SetTurnRadarLeft(radarTurn);
        TurnToFaceTarget(e.X,e.Y);
        if (distance < 100) {
            firePower = 3;
        } else if (distance < 300) {
            firePower = 2; 
        } else {
            firePower = 1;
        }
        Fire(firePower);
    }

    private void TurnToFaceTarget(double x, double y)
    {
        var bearing = BearingTo(x, y);
        if (bearing >= 0)
            turnDirection = 1;
        else
            turnDirection = -1;

        SetTurnLeft(bearing);
    }
    
    public override void OnHitWall(HitWallEvent e) {
        TargetSpeed = -1 * TargetSpeed;
    }
}