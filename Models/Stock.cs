﻿namespace WebApplication1.Models
{
    public class Stock
    {
        public int StockID { get; set; }
        public int Amount { get; set; }
        public int ProductID { get; set; }
        public DateTime Date {  get; set; }
        public int WarehouseID { get; set; }
    }
}
