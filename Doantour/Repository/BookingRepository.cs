using Doantour.DTO;
using Doantour.Helpers;
using Doantour.Models;
using Doantour.Repository;
using Microsoft.EntityFrameworkCore;

namespace Doantour.Respository
{
    public class BookingRepository : BaseRepository<Booking>
    {
        private readonly Hachutravelcontext _context;
        public BookingRepository(Hachutravelcontext context) : base(context)
        {
            _context = context;
        }
        public Task<List<ExcelBookingDTO>> GetInfoBooking()
        {
            var items = from a in _context.Booking
                        join b in _context.Tour on a.TourId equals b.Id
                        join c in _context.Customer on a.CustomerId equals c.Id
                        select new ExcelBookingDTO
                        {
                            tour = b.NameTour,
                            customerName = c.NameCustomer,
                            TotalBill = a.TotalBill,
                            Paymented = a.Paymented,
                            PaymentBy = a.PaymentBy,
                            PaymentTime = a.PaymentTime,
                            Child = a.Child,
                            Adult = a.Adult,
                            StatusBill = a.StatusBill,
                            customerInTours = a.customerInTours,
                        };
            return items.ToListAsync();
        }
        public async Task<MailDTO> InfoToSendMail(int id)
        {
            var item = await (from a in _context.Tour
                              join b in _context.Booking on a.Id equals b.TourId
                              join c in _context.Customer on b.CustomerId equals c.Id
                              join d in _context.Country on a.CountryId equals d.Id
                              
                              where b.Id == id
                              select new MailDTO
                              {
                                  NameTour = a.NameTour,
                                  DateEnd = (DateTime)a.DateEnd,
                                  DateStart = (DateTime)a.DateStart,
                                  Place = a.Place,
                                  timeStart = a.timeStart,
                                  timeEnd = a.timeEnd,
                                  placeStart = a.placeStart,
                                  TourId = a.Id,
                                  placeEnd = a.placeEnd,
                                  NumOfDay = a.NumOfDay,
                                  PriceSale = a.PriceSale,
                                  Email = c.Email,
                                  Address = c.Address,
                                  CountryName = d.CountryName,
                                  CustomerName = c.NameCustomer,
                                  CustomerPhone = c.PhoneNumber,
                                  TotalBill = b.TotalBill,
                                  Paymented = b.Paymented,
                                  PaymentBy = b.PaymentBy,
                                  PaymentTime = b.PaymentTime,
                                  MeetingPoint = a.MeetingPoint,
                                  Child = b.Child,
                                  Adult = b.Adult,
                                  customerInTours = b.customerInTours,
                                  BookingID = b.Id,

                                  Plan = a.Plan,
                              }).SingleOrDefaultAsync();

            return item;
        }

        public async Task<decimal[]> ToTalBillEachMonth(int year)
        {
            // Lấy dữ liệu cho năm nào
            var bookingsForYear = _context.Booking
                .Where(order => order.StatusBill == Constants.Success && !order.IsDeleted && order.UpdateDate.Year == year);

            // Tính tổng hóa đơn cho mỗi tháng của năm nào
            var monthlyTotal = await bookingsForYear
                .GroupBy(order => order.UpdateDate.Month)
                .Select(group => new
                {
                    Month = group.Key,
                    TotalValue = group.Sum(order => order.TotalBill)
                })
                .ToListAsync();

            // Khởi tạo mảng tổng hóa đơn của mỗi tháng và gán giá trị mặc định là 0 cho tất cả các tháng
            decimal[] totalBills = new decimal[12];

            // Duyệt qua kết quả nhóm để lấy tổng hóa đơn của mỗi tháng
            foreach (var item in monthlyTotal)
            {
                totalBills[item.Month - 1] = item.TotalValue;
            }

            return totalBills;
        }
        public async Task<int> CountBookingCancelorNotCancel(string status)
        {
            var results = _context.Booking.Where(x => x.StatusBill == status && x.IsDeleted == false && x.UpdateDate.Year == DateTime.Now.Year).Count();
            return results;
        }


    }
}

