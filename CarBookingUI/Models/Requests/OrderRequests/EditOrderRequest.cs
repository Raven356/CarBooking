﻿namespace CarBookingUI.Models.Requests.OrderRequests
{
    public class EditOrderRequest
    {
        public int Id { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
