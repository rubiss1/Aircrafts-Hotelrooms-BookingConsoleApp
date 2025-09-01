namespace Aircrafts_Hotelrooms_BookingConsoleApp
{
    public class Aircraft : IBookable
    {
        //variables to be used
        private string id;
        private string name;
        private char[,] seats = new char[,] { };
        private bool isAirborne;
        private double revenue;
        private double cost;
        

        //getters
        public char[,] GetSeats()
        {
            return seats;
        }
        public bool GetIsAirborne()
        {
            return isAirborne;
        }
        // aircraft constructer
        public Aircraft(string id, string name, char[,] seats)
        {
            this.id = id;
            this.name = name;
            this.seats = seats;
            isAirborne = false;
        }
        public void Depart()//methode to depart an aircraft
        {
            isAirborne = true;
           
        }
        public void Arrive() //methode to arrive an aicraft
        {
            for (int i = 0; i < seats.GetLength(1); i++) //loop to change all X to -
            {
                for (int j = 0; j < seats.GetLength(0); j++)
                {
                    if (seats[j, i] == 'X')
                    {
                        seats[j, i] = '-';
                       
                    }
                }
            }
            isAirborne = false;
        }
        public void Book(int amount) // methode to book a seat in an aicraft
        {
            if (isBookable(amount) == true)// checking if bookable
            {
                int book = 0;
                for (int i = 0; i < seats.GetLength(1); i++) // loop to book (changing - to X)
                {
                    for (int j = 0; j < seats.GetLength(0); j++)
                    {
                        if (seats[j, i] == '-')
                        {
                            seats[j, i] = 'X';
                            book++;
                            if (book == amount) return; //get out of loop when amount is reached 
                        }
                    }
                }
            }
        }
        public bool isBookable(int amount) //methode to check if a seat is bookable
        {
            int notReserved = 0;
            foreach (char seat in seats) //loop to check based on amount given if seat is reserved
            {
                if (seat == '-') notReserved++;
                if (notReserved >= amount) return true; //true if not 
            }
            return false; 
        }
        public virtual double GetRevenue()
        {
            return revenue;
        }
        public virtual double GetCost()
        {
            return cost;
        }
        public override string ToString()// description of aircraft that will be shown 
        {
            return $" id = {id} | Name = {name} | Airborne = {isAirborne}" ;
        }
    }
}
