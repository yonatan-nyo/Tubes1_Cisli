using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

// ------------------------------------------------------------------
// BotAlvin
// ------------------------------------------------------------------
// A sample bot original made for Robocode by Joshua Galecki.
// Ported to Robocode Tank Royale by Flemming N. Larsen.
//
// Example bot of how to use turn rates.
// ------------------------------------------------------------------
public class BotAlvin : Bot { 

    int turnCounter;
    private double lastNearestDistance = Double.MaxValue;
    private long lastNearestBotId = -1;
    private int scanCounter = 0;

    // The main method starts our bot
    static void Main(string[] args)
    {
        new BotAlvin().Start();
    }

    // Constructor, which loads the bot config file
    BotAlvin() : base(BotInfo.FromFile("BotAlvin.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        turnCounter = 0;

        GunTurnRate = 15;

        while (IsRunning) {
            if (turnCounter % 128 == 0) {
                // Start turning left
                TurnRate = 10;
                TargetSpeed = 5;
            }
            if (turnCounter % 128 == 64) {
                // Start turning right
                TurnRate = -10;
                TargetSpeed = 5;
            }
            turnCounter++;
            Go(); // execute turn
        }
    }

    // We scanned another bot -> fire!
    public override void OnScannedBot(ScannedBotEvent e) {
        double distance = Math.Sqrt(Math.Pow(e.X - X, 2) + Math.Pow(e.Y - Y, 2));
        double botSpeed = Math.Abs(e.Speed);
        double firePower = 0;
        
        // Track the nearest bot
        if (distance < lastNearestDistance || scanCounter >= 5) {
            lastNearestDistance = distance;
            lastNearestBotId = e.ScannedBotId;
            scanCounter = 0;
        } else {
            scanCounter++;
        }
        
        // Only fire if this is the nearest bot or we haven't found a nearer one recently
        if (e.ScannedBotId == lastNearestBotId) {
            // Don't shoot if the target is far away and moving fast (hard to hit, wastes energy)
            if (!(distance > 500 && botSpeed > 4)) {
                if (distance < 100) {
                    firePower = 3;
                } else if (distance < 300) {
                    firePower = 2; 
                } else {
                    firePower = 1;
                }
                
                // Only fire if we determined a non-zero fire power
                if (firePower > 0) {
                    Fire(firePower);
                }
            }
        }
    }

    // We were hit by a bullet -> set turn rate
    public override void OnHitByBullet(HitByBulletEvent e) {
        // Turn to confuse the other bots
        TurnRate = 5;
    }
    
    // We hit a wall -> move in the opposite direction
    public override void OnHitWall(HitWallEvent e) {
        // Move away from the wall by reversing the target speed.
        // Note that current speed is 0 as the bot just hit the wall.
        TargetSpeed = -1 * TargetSpeed;
    }
}