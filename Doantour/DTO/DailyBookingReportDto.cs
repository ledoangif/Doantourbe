namespace Doantour.DTOs
{
    public class DailyBookingReportDto
    {
        public DateTime Date { get; set; }   // Ngày
        public int TotalBookings { get; set; } // Tổng số booking trong ngày
        public string NameCustomer { get; set; }  // Tên khách hàng
        public string PhoneNumber { get; set; }   // Số điện thoại
        public string NameTour { get; set; }      // Tên tour
        public DateTime BookingDate { get; set; }  // Ngày đặt tour
        public string StatusBill { get; set; }    // Trạng thái thanh toán
        public decimal TotalBill { get; set; }    // Tổng tiền
    }
}
