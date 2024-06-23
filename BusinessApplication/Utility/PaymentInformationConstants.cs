namespace BusinessApplication.Utility
{
    public static class PaymentInformationConstants
    {
        public static class PaymentMethods
        {
            public const string CreditCard = "Credit Card";
            public const string PayPal = "Pay Pal";
            public const string BankTransfer = "Bank Transfer";
            public const string ApplePay = "Apple Pay";
            public const string SamsungPay = "Samsung Pay";
        }

        public static class PaymentStatuses
        {
            public const string Pending = "Pending";
            public const string Paid = "Paid";
            public const string Cancelled = "Cancelled";
        }
    }
}
