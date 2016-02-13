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
    public class OrdersSurrogate : IDataContractSurrogate
    {
        public Northwind DbContext { get; set; }

        #region IDataContractSurrogate

        public Type GetDataContractType(Type type)
        {
            return type;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            if (obj is IEnumerable<Order>)
            {
                //unproxying OrderProxy list
                var unproxiedOrders = (obj as IEnumerable<Order>)
                .Select(x =>
                {
                    var customer = DBHelper.UnProxy<Customer>(DbContext, x.Customer);
                    var employee = DBHelper.UnProxy<Employee>(DbContext, x.Employee);
                    var order_Details = x.Order_Details.Select(y => DBHelper.UnProxy<Order_Detail>(DbContext, y)).ToList();
                    var shipper = DBHelper.UnProxy<Shipper>(DbContext, x.Shipper);
                    var unproxied = DBHelper.UnProxy<Order>(DbContext, x);
                    unproxied.Customer = customer;
                    unproxied.Employee = employee;
                    unproxied.Order_Details = order_Details;
                    unproxied.Shipper = shipper;
                    return unproxied;
                })
                .ToList();
                return unproxiedOrders;
            }
            return obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
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
