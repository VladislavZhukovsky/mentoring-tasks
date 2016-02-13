namespace CustomSerialization.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryID { get; set; }

        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }

        //serialization events implementation

        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
            LoadProducts((DbContext)context.Context);
        }

        [OnSerialized]
        public void OnSerialized(StreamingContext context)
        {
            Products.Clear();
        }

        /// <summary>
        /// Loads collection
        /// </summary>
        /// <param name="dbContext"></param>
        private void LoadProducts(DbContext dbContext)
        {
            if (dbContext != null)
            {
                var oc = (dbContext as IObjectContextAdapter).ObjectContext;
                try
                {
                    oc.LoadProperty(this, x => x.Products);
                }
                catch (InvalidOperationException) { }
            }
        }
    }
}
