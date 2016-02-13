namespace CustomSerialization.DB
{
    using CustomSerialization.DB;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;
    using System.Linq;

    [Serializable]
    public partial class Product: ISerializable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        //ISerializable special ctor
        public Product(SerializationInfo information, StreamingContext context)
        {
            ProductID = information.GetInt32("ProductID");
            SupplierID = information.GetInt32("SupplierID");
            CategoryID = information.GetInt32("CategoryID");
            QuantityPerUnit = information.GetString("QuantityPerUnit");
            UnitPrice = information.GetDecimal("UnitPrice");
            UnitsInStock = information.GetInt16("UnitsInStock");
            UnitsOnOrder = information.GetInt16("UnitsOnOrder");
            ReorderLevel = information.GetInt16("ReorderLevel");
            Discontinued = information.GetBoolean("Discontinued");
            Category = (Category)information.GetValue("Category", typeof(Category));
            Supplier = (Supplier)information.GetValue("Supplier", typeof(Supplier));
            Order_Details = (List<Order_Detail>)information.GetValue("OrderDetails", typeof(List<Order_Detail>));
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual Supplier Supplier { get; set; }

        //ISerializable implementation
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var dbContext = context.Context as Northwind;
            if (dbContext != null)
            {
                (dbContext as IObjectContextAdapter).ObjectContext.LoadProperty(this, x => x.Category);
                (dbContext as IObjectContextAdapter).ObjectContext.LoadProperty(this, x => x.Supplier);
                (dbContext as IObjectContextAdapter).ObjectContext.LoadProperty(this, x => x.Order_Details);

                //unproxying objects to serialize real ones
                info.AddValue("Category", DBHelper.UnProxy<Category>(dbContext, Category));
                info.AddValue("Supplier", DBHelper.UnProxy<Supplier>(dbContext, Supplier));

                var orderDetails = Order_Details.Select(x => DBHelper.UnProxy<Order_Detail>(dbContext, x)).ToList();
                info.AddValue("OrderDetails", orderDetails);
            }
            else
            {
                info.AddValue("Category", Category);
                info.AddValue("Supplier", Supplier);
                info.AddValue("OrderDetails", Order_Details);
            }

            info.AddValue("ProductID", ProductID);
            info.AddValue("ProductName", ProductName);
            info.AddValue("SupplierID", SupplierID.Value);
            info.AddValue("CategoryID", CategoryID.Value);
            info.AddValue("QuantityPerUnit", QuantityPerUnit);
            info.AddValue("UnitPrice", UnitPrice);
            info.AddValue("UnitsInStock", UnitsInStock);
            info.AddValue("UnitsOnOrder", UnitsOnOrder);
            info.AddValue("ReorderLevel", ReorderLevel);
            info.AddValue("Discontinued", Discontinued);


        }
    }
}
