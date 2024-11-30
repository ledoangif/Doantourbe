// Service Code
using Doantour.DTOs;
using Doantour.Models;
using Microsoft.EntityFrameworkCore;

public class BookingReportService
{
    private readonly Hachutravelcontext _context;

    public BookingReportService(Hachutravelcontext context)
    {
        _context = context;
    }

    public async Task<object> GetBookingReport(DateTime? startDate, DateTime? endDate)
    {
        // Lấy tất cả các booking không bị xóa
        var bookings = _context.Booking
            .Where(b => !b.IsDeleted);

        // Lọc theo startDate và endDate nếu có
        if (startDate.HasValue)
            bookings = bookings.Where(b => b.CreateDate >= startDate.Value);

        if (endDate.HasValue)
            bookings = bookings.Where(b => b.CreateDate <= endDate.Value);

        // Thống kê theo ngày
        var dailyReport = await bookings
            .GroupBy(b => b.CreateDate.Date)
            .Select(g => new DailyBookingReportDto
            {
                Date = g.Key,
                TotalBookings = g.Count()
            })
            .OrderBy(e => e.Date)
            .ToListAsync(); // Chú ý dùng ToListAsync() ở đây

        // Thống kê theo tháng
        var monthlyReport = await bookings
            .GroupBy(b => new { b.CreateDate.Year, b.CreateDate.Month })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalBookings = g.Count()
            })
            .OrderBy(e => e.Year)
            .ThenBy(e => e.Month)
            .ToListAsync(); // Chú ý dùng ToListAsync() ở đây

        // Chuyển đổi lại tháng năm theo định dạng "MM/yyyy" sau khi đã lấy dữ liệu
        var monthlyBookingReportDtos = monthlyReport.Select(m => new MonthlyBookingReportDto
        {
            Month = $"{m.Month:D2}/{m.Year}",
            TotalBookings = m.TotalBookings
        }).ToList();

        // Trả về kết quả dưới dạng một đối tượng với dữ liệu cần thiết
        return new
        {
            DailyReport = dailyReport,
            MonthlyReport = monthlyBookingReportDtos
        };
    }
}