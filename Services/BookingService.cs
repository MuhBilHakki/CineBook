using CineBook.DTOs;
using CineBook.Exceptions;
using CineBook.Models;
using CineBook.Repositories;

namespace CineBook.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<Seat> _seatRepository;
        private readonly IRepository<BookingSeat> _bookingSeatRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IRepository<Booking> bookingRepository,
            IRepository<Seat> seatRepository,
            IRepository<BookingSeat> bookingSeatRepository,
            ILogger<BookingService> logger)
            
        {
            _bookingRepository = bookingRepository;
            _seatRepository = seatRepository;
            _bookingSeatRepository = bookingSeatRepository;
            _logger = logger;
        }

        //public async Task<Booking?> GetByIdAsync(long id)
        //{
        //    return await _bookingRepository.GetByIdAsync(
        //        id, "Schedule.Movie", "Schedule.Theater", "BookingSeats.Seat");
        //}
        public async Task<Booking?> GetByIdAsync(long id)
        {
            try
            {
                //Schedule? sched = null;
                //string title = sched.Movie!.Title;

                return await _bookingRepository.GetByIdAsync(id, "Schedule.Movie", "Schedule.Theater", "BookingSeats.Seat");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
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

        public async Task<BookingDetailResponse?> CreateAsync(CreateBookingRequest request, long userId)
        {
            await _bookingRepository.BeginTransactionAsync();

            try
            {

                var allSeats = await _seatRepository.GetAllAsync();
                var requestedSeats = allSeats
                    .Where(s => request.SeatIds.Contains(s.Id))
                    .ToList();

                if(requestedSeats.Count != request.SeatIds.Count)
                {
                    await _bookingRepository.RollbackTransactionAsync();
                    throw new NotFoundException("Salah satu kursi yang diminta tidak ditemukan");
                }

                Seat? takenSeat = requestedSeats.FirstOrDefault(s => !s.IsAvailable);
                if (takenSeat != null)
                {
                    _logger.LogWarning("Kursi {SeatNumber} sudah dipesan, percobaan booking oleh user {UserId}",
                    takenSeat.SeatNumber, userId);

                    await _bookingRepository.RollbackTransactionAsync();
                    throw new SeatNotAvailableException(takenSeat.SeatNumber);
                }

                var booking = new Booking
                {
                    UserId = userId,
                    ScheduleId = request.ScheduleId,
                    TotalTickets = request.TotalTickets,
                    TotalPrice = request.TotalPrice,
                    Status = BookingStatus.PENDING
                };

                await _bookingRepository.AddAsync(booking);
          

                foreach (var seat in requestedSeats)
                {
                    seat.IsAvailable = false;
                    _seatRepository.Update(seat);

                    await _bookingSeatRepository.AddAsync(new BookingSeat
                    {
                        Booking = booking,
                        SeatId = seat.Id
                    });
                }

                await _bookingRepository.SaveChangesAsync();

                await _bookingRepository.CommitTransactionAsync();

                _logger.LogInformation("Booking berhasil dibuat: booking {BookingId}, user {UserId}, {SeatCount} kursi",
                booking.Id, userId, requestedSeats.Count);


                var created = await GetByIdAsync(booking.Id);
                return created != null ? ToDetailResponse(created) : null;
            }
            catch (SeatNotAvailableException)
            {
                throw;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Booking gagal untuk user {UserId}", userId);
                await _bookingRepository.RollbackTransactionAsync();
                
                throw;
            }
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
