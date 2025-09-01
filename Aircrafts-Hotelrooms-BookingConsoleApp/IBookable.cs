namespace Aircrafts_Hotelrooms_BookingConsoleApp
{
    //Interface for booking
    public interface IBookable
    {
        //methode to book
        public void Book(int amount);

        //methode to check if its bookable
        public bool isBookable(int amount);
    }
}
