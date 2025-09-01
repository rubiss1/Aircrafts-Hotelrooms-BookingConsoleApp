namespace Aircrafts_Hotelrooms_BookingConsoleApp
{
    public class Plane : Aircraft
    {
        private int revenue = 150;
        private int cost = 1000;
        //plane constructer
        public Plane(string id, string name, char[,] seats) : base(id, name, seats)
        { 
        }
        public override double GetRevenue()
        {
            return revenue;
        }
        public override double GetCost()
        {
            return cost;
        }
        //plane description
        public override string ToString() // MTR – Overrride
        {
            return  base.ToString()+ " | Plane ";
        }
    }
}
