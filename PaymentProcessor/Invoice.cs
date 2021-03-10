using System;
using System.Collections.Generic;

namespace PaymentProcessor
{
    public class Invoice
    {
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public List<Payment> Payments { get; set; }

        public bool IsPaymentExist { get { return Payments != null && Payments.Count > 0; } }

        #region Methods

        public void AddPayment(Payment payment)
        {
            AmountPaid += payment.Amount;
            Payments.Add(payment);
        }

        #endregion Methods
    }
}