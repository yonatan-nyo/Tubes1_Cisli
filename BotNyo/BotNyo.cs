using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class BotNyo : Bot {

    int turnCounter;

    // The main method starts our bot
    static void Main(string[] args)
    {
        new BotNyo().Start();
    }

    // Constructor, which loads the bot config file
    BotNyo() : base(BotInfo.FromFile("BotNyo.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
		turnCounter = 0;

		GunTurnRate = 15;
		
		while (IsRunning) {
			if (turnCounter % 64 == 0) {
				// Straighten out, if we were hit by a bullet (ends turning)
				TurnRate = 0;

                // Go forward with a target speed of 4
				TargetSpeed = 4;
			}
			if (turnCounter % 64 == 32) {
				// Go backwards, faster
                TargetSpeed = -6;
			}
			turnCounter++;
			Go(); // execute turn
		}
	}

    
    private double getFirePower(double x, double y){
        double distance = DistanceTo(x, y);
        if(Energy < 10){
            return 0.1;
        }
        if (distance < 200){
            return 3;
        } else if (distance < 300){
            return 2;
        } else {
            return 1;
        }
    }


    // We scanned another bot -> fire!
    public override void OnScannedBot(ScannedBotEvent e) {
        double bulletPower = getFirePower(e.X, e.Y);
        double distance = DistanceTo(e.X, e.Y);
        double time = distance / CalcBulletSpeed(bulletPower);

        // Predict enemy's future position
        double futureX = e.X + Math.Sin(e.Direction) * e.Speed * time;
        double futureY = e.Y + Math.Cos(e.Direction) * e.Speed * time;

        // Calculate the angle to the predicted position
        double predictedAngle = Math.Atan2(futureX - X, futureY - Y) * (180 / Math.PI);
        double angleDifference = NormalizeBearing(predictedAngle - GunDirection);

        // Define a threshold for significant angle deviation
        double angleThreshold = 130.0; // Adjust this value if needed

        // Only fire if the predicted angle is within the threshold
        if (GunHeat == 0 && Math.Abs(angleDifference) <= angleThreshold) {
            Fire(bulletPower);
            Console.WriteLine($"Fired at Predicted Position: {futureX}, {futureY} with Angle Difference: {angleDifference}");
        } else {
            Console.WriteLine($"Not firing: Angle difference too large: {angleDifference}");
        }

        Console.WriteLine($"Scanned bot Direction: {e.Direction}");
        Console.WriteLine($"Predicted Position: {futureX}, {futureY} (Angle Diff: {angleDifference})");
    }

    // Helper function to normalize angles between -180 and 180 degrees
    private double NormalizeBearing(double angle) {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
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

 // We won the round -> do a victory dance!
    public override void OnWonRound(WonRoundEvent e)
    {
        // Victory dance turning right 360 degrees 100 times
        TurnLeft(36_000);
    }
}