namespace CineBook.Exceptions
{
    public class SeatNotAvailableException : Exception
    {
        public SeatNotAvailableException(string seatCode) : base($"Kursi {seatCode} sudah dipesan oleh pengguna lain")
        {

        }
    }
}
