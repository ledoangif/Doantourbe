namespace Doantour.DTO
{
    public class ExcelBookingDTO
    {
        public string tour { get; set; }
        public string customerName { get; set; }
        public decimal TotalBill { get; set; }
        public decimal Paymented { get; set; }
        public string? PaymentBy { get; set; }
        public DateTime PaymentTime { get; set; }
        public int Child { get; set; }
        public int Adult { get; set; }
        public string StatusBill { get; set; }
        public string customerInTours { get; set; }
    }
}
