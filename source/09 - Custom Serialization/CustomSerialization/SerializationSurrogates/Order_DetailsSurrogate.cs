using CustomSerialization.DB;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CustomSerialization.SerializationSurrogates
{
    //Surrogated class with IDataContractSurrogate implemention for customizing serialization
    public class Order_DetailsSurrogated : IDataContractSurrogate
    {
        //DbContext added to use it for loading referenced properties
        public Northwind DbContext { get; set; }

        #region IDataContractSurrogate

        public Type GetDataContractType(Type type)
        {
            if (typeof(Order_Detail).IsAssignableFrom(type))
            {
                return typeof(Order_DetailsSurrogated);
            }
            //DataContractProduct is made for serialization of Order_Details hash set
            if (typeof(Product).IsAssignableFrom(type))
            {
                return typeof(DataContractProduct);
            }
            return type;
        }

        //serialization
        public object GetObjectToSerialize(object obj, Type targetType)
        {
            //loading Order_Detail properties
            if (obj is IEnumerable<Order_Detail>)
            {
                var orderDetails = (obj as IEnumerable<Order_Detail>).ToList();
                for (var i = 0; i < orderDetails.Count; i++)
                {
                    (DbContext as IObjectContextAdapter).ObjectContext.LoadProperty(orderDetails[i], x => x.Product);
                    (DbContext as IObjectContextAdapter).ObjectContext.LoadProperty(orderDetails[i], x => x.Order);
                    orderDetails[i].Product.Order_Details.Clear();
                    orderDetails[i].Order.Order_Details.Clear();
                }
            }
            if (obj is Product)
            {
                var dcp = new DataContractProduct((Product)obj);
                return dcp; 
            }
            return obj;
        }

        //deserialization
        public object GetDeserializedObject(object obj, Type targetType)
        {
            if (typeof(DataContractProduct).IsAssignableFrom(obj.GetType()))
            {
                var dcp = obj as DataContractProduct;
                return new Product()
                {
                    ProductID = dcp.ProductID,
                    ProductName = dcp.ProductName,
                    SupplierID = dcp.SupplierID,
                    CategoryID = dcp.CategoryID,
                    QuantityPerUnit = dcp.QuantityPerUnit,
                    UnitPrice = dcp.UnitPrice,
                    UnitsInStock = dcp.UnitsInStock,
                    UnitsOnOrder = dcp.UnitsOnOrder,
                    ReorderLevel = dcp.ReorderLevel,
                    Discontinued = dcp.Discontinued,
                    Category = dcp.Category,
                    Order_Details = new HashSet<Order_Detail>(dcp.Order_Details),
                    Supplier = dcp.Supplier
                };
            }
            return obj;
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
