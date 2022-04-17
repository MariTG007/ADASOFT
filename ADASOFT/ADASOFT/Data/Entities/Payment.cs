namespace ADASOFT.Data.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public float TotalPayment { get; set; }

        public Enrollment Enrollment { get; set; }

        public DateTime DatePayment { get; set; }
    }
}
