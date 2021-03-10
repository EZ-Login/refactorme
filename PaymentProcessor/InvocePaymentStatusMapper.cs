namespace PaymentProcessor
{
    public class InvocePaymentStatusMapper
    {
        public static string GetMessageByStatus(InvoicePaymentStatus invoicePaymentStatus)
        {
            string result = string.Empty;

            switch (invoicePaymentStatus)
            {
                case InvoicePaymentStatus.None:
                    break;

                case InvoicePaymentStatus.NoPaymentNeeded:
                    result = "no payment needed";
                    break;

                case InvoicePaymentStatus.AlreadyFullyPaid:
                    result = "invoice was already fully paid";
                    break;

                case InvoicePaymentStatus.GreaterPayment:
                    result = "the payment is greater than the invoice amount";
                    break;

                case InvoicePaymentStatus.GreaterPartialPayment:
                    result = "the payment is greater than the partial amount remaining";
                    break;

                case InvoicePaymentStatus.FullyPaidNow:
                    result = "invoice is now fully paid";
                    break;

                case InvoicePaymentStatus.PartialPaymentFullyPaidNow:
                    result = "final partial payment received, invoice is now fully paid";
                    break;

                case InvoicePaymentStatus.FirstPartialPaymentReceived:
                    result = "invoice is now partially paid";
                    break;

                case InvoicePaymentStatus.AnotherPartialPaymentReceived:
                    result = "another partial payment received, still not fully paid";
                    break;

                default:
                    // or throw an Exception
                    break;
            }

            return result;
        }
    }
}