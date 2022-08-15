using System.ComponentModel.DataAnnotations;

namespace WebInterface.Models
{
    /// <summary>
    /// Bank model.
    /// </summary>
    public class Bank
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Bank name from A1 excel cell.
        /// </summary>
        public string Name { get; set; }

        public List<Sheet> Sheets { get; set; }
    }
}
