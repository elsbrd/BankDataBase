namespace WindowsFormsApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Branch")]
    public partial class Branch
    {
        public int BranchId { get; set; }

        [StringLength(40)]
        public string Name { get; set; }

        [StringLength(60)]
        public string Address { get; set; }

        public int? BankId_ID { get; set; }

        public virtual Bank Bank { get; set; }
    }
}
