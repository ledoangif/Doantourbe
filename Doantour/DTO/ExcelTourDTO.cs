namespace Doantour.DTO
{
    public class ExcelTourDTO
    {
        public string? NameTour { get; set; }
        public int Seat { get; set; }
        public int slot { get; set; }
        public decimal Cost { get; set; }
        public int? Discount { get; set; }
        public decimal PriceSale { get; set; }
        public string timeStart { get; set; }

        public DateTime? DateStart { get; set; }

        public string timeEnd { get; set; }
        public DateTime? DateEnd { get; set; }
        public int NumOfDay { get; set; }
        public string? Place { get; set; }

        public string? placeStart { get; set; }
        public string? placeEnd { get; set; }
        public string? IsFinish { get; set; }

        public bool IsLocal { get; set; }
        public string country { get; set; }
    }
}
