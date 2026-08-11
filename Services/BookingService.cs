using CineBook.DTOs;
using CineBook.Models;
using CineBook.Repositories;

namespace CineBook.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<Seat> _seatRepository;
        private readonly IRepository<BookingSeat> _bookingSeatRepository;

        public BookingService(
            IRepository<Booking> bookingRepository,
            IRepository<Seat> seatRepository,
            IRepository<BookingSeat> bookingSeatRepository)
        {
            _bookingRepository = bookingRepository;
            _seatRepository = seatRepository;
            _bookingSeatRepository = bookingSeatRepository;
        }

        public async Task<Booking?> GetByIdAsync(long id)
        {
            return await _bookingRepository.GetByIdAsync(
                id, "Schedule.Movie", "Schedule.Theater", "BookingSeats.Seat");
        }

        public async Task<List<Booking>> GetAllAsync(string? status, DateTime? date)
        {
            var bookings = await _bookingRepository.GetAllAsync("Schedule.Movie", "Schedule.Theater");

            if (!string.IsNullOrEmpty(status))
            {
                bookings = bookings.Where(b => b.Status.ToString() == status).ToList();
            }

            return bookings;
        }

        public async Task<BookingDetailResponse?> CreateAsync(CreateBookingRequest request)
        {
            var allSeats = await _seatRepository.GetAllAsync();
            var requestedSeats = allSeats
                .Where(s => request.SeatIds.Contains(s.Id))
                .ToList();

            bool anySeatTaken = requestedSeats.Any(s => !s.IsAvailable);
            if (anySeatTaken || requestedSeats.Count != request.SeatIds.Count)
            {
                return null;
            }

            var booking = new Booking
            {
                UserId = 1,
                ScheduleId = request.ScheduleId,
                TotalTickets = request.TotalTickets,
                TotalPrice = request.TotalPrice,
                Status = BookingStatus.PENDING
            };

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            foreach (var seat in requestedSeats)
            {
                seat.IsAvailable = false;
                _seatRepository.Update(seat);

                await _bookingSeatRepository.AddAsync(new BookingSeat
                {
                    BookingId = booking.Id,
                    SeatId = seat.Id
                });
            }

            await _seatRepository.SaveChangesAsync();

            var created = await GetByIdAsync(booking.Id);
            return created != null ? ToDetailResponse(created) : null;
        }

        public async Task<BookingDetailResponse?> UpdateAsync(long id, CreateBookingRequest request)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null) return null;

            booking.TotalTickets = request.TotalTickets;
            booking.TotalPrice = request.TotalPrice;

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            var updated = await GetByIdAsync(id);
            return updated != null ? ToDetailResponse(updated) : null;
        }

        public async Task<bool> CancelAsync(long id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null) return false;

            booking.Status = BookingStatus.CANCELLED;
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            return true;
        }

        public BookingDetailResponse ToDetailResponse(Booking booking)
        {
            return new BookingDetailResponse
            {
                Id = booking.Id,
                UserId = booking.UserId,
                ScheduleId = booking.ScheduleId,
                MovieTitle = booking.Schedule?.Movie?.Title ?? string.Empty,
                TheaterName = booking.Schedule?.Theater?.Name ?? string.Empty,
                StartTime = booking.Schedule?.StartTime ?? default,
                TotalTickets = booking.TotalTickets,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status.ToString()
            };
        }
    }


}
