using CustomSerialization.SerializationSurrogates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CustomSerialization.DB;
using CustomSerialization.TestHelpers;

namespace CustomSerialization
{
    [TestClass]
    public class SerializationSolutions
    {
        Northwind dbContext;

        [TestInitialize]
        public void Initialize()
        {
            dbContext = new Northwind();
        }

        [TestMethod]
        public void SerializationCallbacks()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            //adding DbContext to StreamingContext
            var tester = new XmlDataContractSerializerTester<IEnumerable<Category>>(new NetDataContractSerializer(new StreamingContext(StreamingContextStates.All, dbContext)), true);
            var categories = dbContext.Categories.ToList();

            tester.SerializeAndDeserialize(categories);
        }

        [TestMethod]
        public void ISerializable()
        {
            dbContext.Configuration.ProxyCreationEnabled = true;

            //adding DbContext to StreamingContext
            var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(new NetDataContractSerializer(new StreamingContext(StreamingContextStates.All, dbContext)), true);
            var products = dbContext.Products.ToList();
            tester.SerializeAndDeserialize(products);
        }

        [TestMethod]
        public void ISerializationSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            //using surrogates
            //DataContractSurrogate used instead of SerializationSurrogate because 
            //it is not compatible with DataCOntractSerializer
            var orderDetailsSurrogate = new Order_DetailsSurrogated() { DbContext = dbContext };

            var serializer = new DataContractSerializer(
                    typeof(Product),
                    new Type[] { typeof(List<Order_Detail>), typeof(Order_Detail) },
                    int.MaxValue,
                    false,
                    true,
                    orderDetailsSurrogate
                    );

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(serializer);
            var orderDetails = dbContext.Order_Details.ToList();

            tester.SerializeAndDeserialize(orderDetails);
        }

        [TestMethod]
        public void IDataContractSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = true;
            dbContext.Configuration.LazyLoadingEnabled = true;

            //using surrogate
            var ordersSurrogate = new OrdersSurrogate() { DbContext = dbContext };
            var serializer = new DataContractSerializer(
                typeof(IEnumerable<Order>),
                new Type[] { },
                int.MaxValue,
                false,
                true,
                ordersSurrogate
                );

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order>>(serializer, true);
            var orders = dbContext.Orders.ToList();
            tester.SerializeAndDeserialize(orders);
        }
    }
}
