using System;
using System.Reflection;
using Dapper;

namespace Hotels.API.Queries
{
    public class QueryBuilder
    {

        public static (string condition, object parameters) PrepareCondition(object filter)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            string sqlCondition = string.Empty;

            PropertyInfo[] props = filter.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object propValue  = prop.GetValue(filter,null);
                if(propValue != null)
                {
                    sqlCondition += $" And {prop.Name} = '{propValue}'";
                    dynamicParameters.Add(prop.Name, propValue);
                }
            }

            return (sqlCondition, dynamicParameters);

        }
    }
}
