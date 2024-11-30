using Doantour.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doantour.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingReportController : ControllerBase
    {
        private readonly Hachutravelcontext _context;

        public BookingReportController(Hachutravelcontext context)
        {
            _context = context;
        }

        //[HttpGet("booking-report")]
        //public async Task<IActionResult> GetBookingReport(DateTime startDate, DateTime endDate)
        //{
        //    // Lấy báo cáo thống kê theo ngày
        //    var dailyReport = await _context.Booking
        //        .Where(b => b.CreateDate >= startDate && b.CreateDate <= endDate && !b.IsDeleted)
        //        .Include(b => b.Customer)  // Lấy thông tin khách hàng
        //        .Include(b => b.Tour)      // Lấy thông tin tour
        //        .GroupBy(b => b.CreateDate.Date)  // Nhóm theo ngày
        //        .Select(g => new
        //        {
        //            Date = g.Key,  // Giữ nguyên DateTime, sẽ chuyển đổi sau khi lấy dữ liệu
        //            TotalBookings = g.Count(),  // Tổng số đơn đặt tour trong ngày
        //            Bookings = g.Select(b => new
        //            {
        //                b.Customer.NameCustomer,    // Tên khách hàng
        //                b.Customer.PhoneNumber,     // Số điện thoại khách hàng
        //                b.Tour.NameTour,            // Tên tour
        //                BookingDate = b.CreateDate, // Thời gian đặt tour
        //                b.StatusBill,               // Trạng thái của tour
        //                b.TotalBill                 // Giá tiền của tour
        //            }).ToList()
        //        })
        //        .ToListAsync();  // Chuyển đổi thành danh sách bất đồng bộ

        //    // Chuyển đổi Date thành chuỗi sau khi tải dữ liệu từ cơ sở dữ liệu
        //    var formattedDailyReport = dailyReport.Select(report => new
        //    {
        //        Date = report.Date.ToString("yyyy-MM-dd"),  // Chuyển đổi Date thành chuỗi
        //        TotalBookings = report.TotalBookings,
        //        Bookings = report.Bookings
        //    }).ToList();

        //    // Lấy báo cáo thống kê theo tháng
        //    var monthlyReport = await _context.Booking
        //        .Where(b => b.CreateDate >= startDate && b.CreateDate <= endDate && !b.IsDeleted)
        //        .Include(b => b.Customer)  // Lấy thông tin khách hàng
        //        .Include(b => b.Tour)      // Lấy thông tin tour
        //        .GroupBy(b => new { b.CreateDate.Year, b.CreateDate.Month })  // Nhóm theo năm và tháng
        //        .Select(g => new
        //        {
        //            Month = g.Key,  // Giữ nguyên Month, sẽ chuyển đổi sau khi lấy dữ liệu
        //            TotalBookings = g.Count(),  // Tổng số đơn đặt tour trong tháng
        //            Bookings = g.Select(b => new
        //            {
        //                b.Customer.NameCustomer,    // Tên khách hàng
        //                b.Customer.PhoneNumber,     // Số điện thoại khách hàng
        //                b.Tour.NameTour,            // Tên tour
        //                BookingDate = b.CreateDate, // Thời gian đặt tour
        //                b.StatusBill,               // Trạng thái của tour
        //                b.TotalBill                 // Giá tiền của tour
        //            }).ToList()
        //        })
        //        .ToListAsync();  // Chuyển đổi thành danh sách bất đồng bộ

        //    // Chuyển đổi Month thành chuỗi sau khi tải dữ liệu từ cơ sở dữ liệu
        //    var formattedMonthlyReport = monthlyReport.Select(report => new
        //    {
        //        Month = $"{report.Month.Month}/{report.Month.Year}",  // Chuyển đổi tháng và năm thành chuỗi "MM/YYYY"
        //        TotalBookings = report.TotalBookings,
        //        Bookings = report.Bookings
        //    }).ToList();

        //    // Trả về kết quả dưới dạng JSON
        //    var result = new
        //    {
        //        DailyReport = formattedDailyReport,
        //        MonthlyReport = formattedMonthlyReport
        //    };

        //    return Ok(result);
        //}
        [HttpGet("daily-booking-report")]
        public async Task<IActionResult> GetDailyBookingReport(DateTime startDate, DateTime endDate)
        {
            var bookingReport = await _context.Booking
                .Where(b => b.CreateDate >= startDate && b.CreateDate <= endDate && !b.IsDeleted)
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .Select(b => new
                {
                    Date = b.CreateDate.ToString("yyyy-MM-dd"), // Định dạng ngày nếu cần
                    b.Customer.NameCustomer,
                    b.Customer.PhoneNumber,
                    b.Tour.NameTour,
                    BookingDate = b.CreateDate,
                    b.StatusBill,
                    b.TotalBill
                })
                .OrderBy(b => b.BookingDate) // Sắp xếp theo ngày booking
                .ToListAsync();

            return Ok(new { BookingReport = bookingReport });
        }
        [HttpGet("monthly-booking-report")]
        public async Task<IActionResult> GetMonthlyBookingReport(DateTime startDate, DateTime endDate)
        {
            var monthlyReport = await _context.Booking
                .Where(b => b.CreateDate >= startDate && b.CreateDate <= endDate && !b.IsDeleted)
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .GroupBy(b => new { b.CreateDate.Year, b.CreateDate.Month })
                .Select(g => new
                {
                    Month = g.Key,
                    TotalBookings = g.Count(),
                    Bookings = g.Select(b => new
                    {
                        b.Customer.NameCustomer,
                        b.Customer.PhoneNumber,
                        b.Tour.NameTour,
                        BookingDate = b.CreateDate,
                        b.StatusBill,
                        b.TotalBill
                    }).ToList()
                })
                .ToListAsync();

            var formattedMonthlyReport = monthlyReport.Select(report => new
            {
                Month = $"{report.Month.Month}/{report.Month.Year}",
                TotalBookings = report.TotalBookings,
                Bookings = report.Bookings
            }).ToList();

            return Ok(new { MonthlyReport = formattedMonthlyReport });
        }

    }
}
