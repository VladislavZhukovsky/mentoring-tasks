using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task01
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var parameterExp = Expression.Parameter(typeof(TSource));

            var bindingExps = new List<MemberBinding>();

            List<MemberInfo> memberInfos = new List<MemberInfo>();

            typeof(Bar).GetFields().ToList().ForEach(fieldInfo => memberInfos.Add(fieldInfo));
            typeof(Bar).GetProperties().ToList().ForEach(propertyInfo => memberInfos.Add(propertyInfo));

            foreach (var memberInfo in memberInfos)
            {
                var fieldExp = Expression.PropertyOrField(parameterExp, memberInfo.Name);
                var bindingExp = Expression.Bind(memberInfo, fieldExp);
                bindingExps.Add(bindingExp);
            }

            var newExp = Expression.New(typeof(TDestination));
            var memberInitExp = Expression.MemberInit(newExp, bindingExps);
            var lambdaExp = Expression.Lambda<Func<TSource, TDestination>>(memberInitExp, parameterExp);

            return new Mapper<TSource, TDestination>(lambdaExp.Compile());
        }

    }
}
