namespace PaymentProcessor
{
    public enum InvoicePaymentStatus : byte
    {
        None = 0,
        NoPaymentNeeded,
        AlreadyFullyPaid,
        GreaterPayment,
        GreaterPartialPayment,
        FullyPaidNow,
        PartialPaymentFullyPaidNow,
        FirstPartialPaymentReceived,
        AnotherPartialPaymentReceived
    }
}