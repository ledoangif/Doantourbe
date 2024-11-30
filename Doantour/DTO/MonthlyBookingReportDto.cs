namespace Doantour.DTOs
{
    // DTO cho báo cáo tháng
    public class MonthlyBookingReportDto
    {
        public string Month { get; set; } // Dạng chuỗi "MM/YYYY"
        public int TotalBookings { get; set; }
        public string NameCustomer { get; set; } // Tên khách hàng
        public string PhoneNumber { get; set; }  // Số điện thoại
        public string NameTour { get; set; }     // Tên tour
        public DateTime BookingDate { get; set; } // Ngày đặt tour
        public string StatusBill { get; set; }   // Trạng thái thanh toán
        public decimal TotalBill { get; set; }   // Tổng tiền
    }
}

