namespace Aircrafts_Hotelrooms_BookingConsoleApp
{
    public enum RoomType
    {
        Basic,
        Comfort,
        Deluxe,
        Exclusive
    }
    public class Hotelroom : IBookable
    {
        //variables to be used
        private int roomNumber;
        private RoomType roomType;
        public bool occupied;
        private double revenue = 40;
        private double cost = 80;
        //roomtype getter
        public RoomType GetRoomType()
        {
          return roomType;
        }
        //booking a room
        public void Book(int amount)
        {
            if (isBookable(amount))
            {
                occupied = true;
            }
        }

        //checking if room is available
        public bool isBookable(int amount)
        {
            return !occupied;

        }
        public double GetRevenue()
        {
            return revenue;
        }
        public double GetCost()
        {
            return cost;
        }
        //hotelroom constructer 
        public Hotelroom(RoomType roomType, int roomNumber)
        {
            this.roomType = roomType;
            this.roomNumber = roomNumber;
            this.occupied = false;

        }
        //description for the hotelrooms
        public override string ToString()
        {
            return $"|   {roomNumber}\t |    {roomType}\t |\t{ (occupied ? "X" : "-")}\t|";
        }
    }
}
