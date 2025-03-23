using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class CisliMafiaBoss : Bot
{
    private double enemyX, enemyY, enemyDistance;
    private double enemyEnergy = 100;
    private double lastEnemyEnergy = 100;
    private Random random = new Random();
    private int moveDirection = 1;
    private int wallHitCount = 0;
    private const double WALL_MARGIN = 100;
    private const double DANGER_DISTANCE = 200;

    static void Main(string[] args) => new CisliMafiaBoss().Start();

    CisliMafiaBoss() : base(BotInfo.FromFile("CisliMafiaBoss.json")) { }

    public override void Run()
    {
        AdjustGunForBodyTurn = true;
        AdjustRadarForBodyTurn = true;
        AdjustRadarForGunTurn = true;
        BodyColor = Color.DarkGray;
        GunColor = Color.Black;
        RadarColor = Color.Red;

        while (IsRunning)
        {
            SetTurnRadarRight(double.PositiveInfinity);
            ExecuteMovement();
            Go();
        }
    }

    private void ExecuteMovement()
    {
        if (IsNearWall())
        {
            EvadeWall();
            return;
        }
        EnhancedMovement();
    }

    private void EnhancedMovement()
    {
        if (enemyDistance == 0)
        {
            StayNearWalls();
            return;
        }

        if (lastEnemyEnergy > enemyEnergy && (lastEnemyEnergy - enemyEnergy) <= 3.0 && (lastEnemyEnergy - enemyEnergy) >= 0.1)
        {
            EvadeBullet();
        }
        else if (enemyDistance < DANGER_DISTANCE)
        {
            RunAway();
        }
        else
        {   
            OrbitEnemy();
        }
        lastEnemyEnergy = enemyEnergy;
    }

    private bool IsNearWall()
    {
        return X < WALL_MARGIN || Y < WALL_MARGIN || X > ArenaWidth - WALL_MARGIN || Y > ArenaHeight - WALL_MARGIN;
    }

    private void EvadeWall()
    {
        Console.WriteLine("Evading Wall");
        SetTurnLeft(BearingTo(ArenaWidth / 1.5, ArenaHeight / 1.5));
        SetForward(100);
    }

    private void StayNearWalls()
    {
        if (IsNearWall())
        {
            EvadeWall();
        }
    }

    private void EvadeBullet()
    {
        if (IsNearWall())
        {
            EvadeWall();
            return;
        }

        double angleToEnemy = BearingTo(enemyX, enemyY);
        int evasionDirection = (random.NextDouble() > 0.5) ? 1 : -1;
        SetTurnLeft(angleToEnemy + 90 * evasionDirection);
        SetForward(Math.Min(150, 300000 / (enemyDistance * enemyDistance + 1)));
    }

    private void RunAway()
    {
        if (IsNearWall())
        {
            EvadeWall();
            return;
        }

        double angleToEnemy = BearingTo(enemyX, enemyY);
        SetTurnLeft(angleToEnemy + 180);
        SetForward(150);
    }

    private void OrbitEnemy()
    {
        if (IsNearWall())
        {
            EvadeWall();
            return;
        }

        double angleToEnemy = BearingTo(enemyX, enemyY);
        SetTurnLeft(angleToEnemy + 90 * moveDirection);
        SetForward(150);

        if (random.NextDouble() < 0.05)
            moveDirection *= -1;
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        enemyX = e.X;
        enemyY = e.Y;
        enemyDistance = DistanceTo(e.X, e.Y);
        enemyEnergy = e.Energy;
        double angleToEnemy = Direction + BearingTo(e.X, e.Y);
        double radarTurn = NormalizeAngle(angleToEnemy - RadarDirection);
        radarTurn += radarTurn < 0 ? -15 : 15;
        SetTurnRadarLeft(radarTurn);
        double speed = e.Speed;
        if(e.Energy == 0) speed = 0;
        PredictEnemyAndFire(e.X, e.Y, speed, e.Direction, enemyDistance);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        SetBack(50);
        SetTurnRight(90);
        wallHitCount++;
        if (wallHitCount > 2)
        {
            SetTurnLeft(BearingTo(ArenaWidth / 1.5, ArenaHeight / 1.5));
            SetForward(200);
            wallHitCount = 0;
        }
    }

    private double NormalizeAngle(double angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }

    private void PredictEnemyAndFire(double x, double y, double speed, double heading, double distance)
    {
        double bulletPower = distance < 100 ? 3.0 : distance < 200 ? 2.0 : distance < 400 ? 1.0 : 0.5;
        double bulletSpeed = 20 - 3 * bulletPower;
        double time = distance*0.7 / bulletSpeed;
        double futureX = x + Math.Sin(heading) * speed * time * 0.7;
        double futureY = y + Math.Cos(heading) * speed * time * 0.7;
        double gunBearing = GunBearingTo(futureX, futureY);
        SetTurnGunLeft(gunBearing);
        if (Math.Abs(gunBearing) < 5) Fire(bulletPower);
    }
}