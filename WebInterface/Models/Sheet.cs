using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebInterface.Models
{
    public class Sheet
    {
        [Key]
        public int Id { get; set; }

        public int Account { get; set; }

        public int TwoDigitAccount { get; set; }

        public decimal InputBalanceActive { get; set; }

        public decimal InputBalancePassive { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public decimal OutputBalanceActive { get; set; }

        public decimal OutputBalancePassive { get; set; }

        public string ClassName { get; set; }

        public int BankId { get; set; }

        [ForeignKey("BankId")]
        public Bank Bank { get; set; }
    }
}
