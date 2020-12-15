namespace WindowsFormsApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bank")]
    public partial class Bank
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bank()
        {
            Branches = new HashSet<Branch>();
        }

        public int BankId { get; set; }

        [StringLength(40)]
        public string BankName { get; set; }

        [StringLength(60)]
        public string Address { get; set; }

        [StringLength(15)]
        public string Code { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Branch> Branches { get; set; }
    }
}
