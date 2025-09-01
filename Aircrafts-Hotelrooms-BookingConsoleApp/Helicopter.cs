namespace Aircrafts_Hotelrooms_BookingConsoleApp
{
    public class Helicopter : Aircraft
    {
        private int revenue = 300;
        private int cost = 500;
        //bool for military planes
        private bool isMilitary;

        //getter for ismilitary
        public bool IsMilitaryGetter()
        {
            return isMilitary;
        }
        public override double GetRevenue()
        {
            return revenue;
        }
        public override double GetCost()
        {
            return cost;
        }
        //constructer for helicopter
        public Helicopter(string id, string name, char[,] seats, bool isMilitary) : base(id, name, seats)
        {
            this.isMilitary = isMilitary;
        }

        //descriptiion for helicopter
        public override string ToString() // MTR – Overrride
        {
            return  base.ToString() + " | Helicopter";
        }
    }
}
