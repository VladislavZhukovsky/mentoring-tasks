using CustomSerialization.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CustomSerialization.SerializationSurrogates
{
    [DataContract]
    public class DataContractProduct
    {
        public DataContractProduct(Product product)
        {
            ProductID = product.ProductID;
            ProductName = product.ProductName;
            SupplierID = product.SupplierID;
            CategoryID = product.CategoryID;
            QuantityPerUnit = product.QuantityPerUnit;
            UnitPrice = product.UnitPrice;
            UnitsInStock = product.UnitsInStock;
            UnitsOnOrder = product.UnitsOnOrder;
            ReorderLevel = product.ReorderLevel;
            Discontinued = product.Discontinued;
            Category = product.Category;
            Order_Details = product.Order_Details.ToList();
            Supplier = product.Supplier;
        }

        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public int? SupplierID { get; set; }
        [DataMember]
        public int? CategoryID { get; set; }
        [DataMember]
        public string QuantityPerUnit { get; set; }
        [DataMember]
        public decimal? UnitPrice { get; set; }
        [DataMember]
        public short? UnitsInStock { get; set; }
        [DataMember]
        public short? UnitsOnOrder { get; set; }
        [DataMember]
        public short? ReorderLevel { get; set; }
        [DataMember]
        public bool Discontinued { get; set; }
        [DataMember]
        public Category Category { get; set; }
        [DataMember]
        public List<Order_Detail> Order_Details { get; set; }
        [DataMember]
        public Supplier Supplier { get; set; }
    }
}
