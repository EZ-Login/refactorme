using System;

namespace PaymentProcessor
{
    public class InvoicePaymentProcessor
    {
        private readonly InvoiceRepository _invoiceRepository;

        public InvoicePaymentProcessor(InvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        //The method should return an enum item (but not string) to further possibility to change message text and/or add localization without necessity to change tests,
        //then the enum item can be mapped to a text message using InvocePaymentStatusMapper
        public string ProcessPayment(Payment payment)
        {
            //Need to check payment for null also and add certain test

            var invoice = GetInvoiceForPayment(payment.Reference);

            var result = ValidateInvoice(invoice);
            if (result != InvoicePaymentStatus.None) return InvocePaymentStatusMapper.GetMessageByStatus(result);

            result = CheckForFullyPaid(invoice);
            if (result != InvoicePaymentStatus.None) return InvocePaymentStatusMapper.GetMessageByStatus(result);

            result = CheckForGreaterPaid(invoice, payment.Amount);
            if (result != InvoicePaymentStatus.None) return InvocePaymentStatusMapper.GetMessageByStatus(result);

            result = AddPaymentAndCheckStatus(invoice, payment);

            return InvocePaymentStatusMapper.GetMessageByStatus(result);
        }

        protected Invoice GetInvoiceForPayment(string reference)
        {
            var invoice = _invoiceRepository.GetInvoice(reference);

            if (invoice == null)
            {
                throw new InvalidOperationException("There is no invoice matching this payment");
            }

            return invoice;
        }

        protected InvoicePaymentStatus ValidateInvoice(Invoice invoice)
        {
            if (invoice.Amount > 0) return InvoicePaymentStatus.None;

            if (!invoice.IsPaymentExist) return InvoicePaymentStatus.NoPaymentNeeded;
            else throw new InvalidOperationException("The invoice is in an invalid state, it has an amount of 0 and it has payments.");
        }

        protected InvoicePaymentStatus CheckForFullyPaid(Invoice invoice)
        {
            InvoicePaymentStatus result = InvoicePaymentStatus.None;

            //Calculation of Payments sum is not used after correction in ProcessPayment_Should_ReturnFullyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidEqualsInvoiceAmount test.
            //Otherwise sum of all payments should be calculated.
            if (invoice.AmountPaid != 0 && invoice.AmountPaid == invoice.Amount) result = InvoicePaymentStatus.AlreadyFullyPaid;

            return result;
        }

        protected InvoicePaymentStatus CheckForGreaterPaid(Invoice invoice, decimal newPaymentAmount)
        {
            InvoicePaymentStatus result = InvoicePaymentStatus.None;

            var leftToPay = invoice.Amount - invoice.AmountPaid;

            if (newPaymentAmount > leftToPay)
            {
                if (invoice.IsPaymentExist) result = InvoicePaymentStatus.GreaterPartialPayment;
                else result = InvoicePaymentStatus.GreaterPayment;
            }

            return result;
        }

        protected InvoicePaymentStatus AddPaymentAndCheckStatus(Invoice invoice, Payment payment)
        {
            var isPaymentExistAlready = invoice.IsPaymentExist;

            invoice.AddPayment(payment);

            if (invoice.Amount - invoice.AmountPaid == 0)
            {
                if (isPaymentExistAlready) return InvoicePaymentStatus.PartialPaymentFullyPaidNow;
                else return InvoicePaymentStatus.FullyPaidNow;
            }
            else
            {
                if (isPaymentExistAlready) return InvoicePaymentStatus.AnotherPartialPaymentReceived;
                else return InvoicePaymentStatus.FirstPartialPaymentReceived;
            }
        }
    }
}