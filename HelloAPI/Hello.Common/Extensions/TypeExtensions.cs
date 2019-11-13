using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Hello.Common.Extensions
{    
    public static class TypeExtensions
    {        
        public static IEnumerable<Type> GetNonInheritedInterfaces(this Type t)
        {
            Validators.ThrowArgNullExcIfNull(t, nameof(t));
            
            IEnumerable<Type> interfacesImplemented = t.GetInterfaces();
            if (interfacesImplemented == null || !interfacesImplemented.Any())
            {
                return null;
            }

            IList<Type> nonInheritedInterfaces = new List<Type>();
            foreach (Type interfaceImplemented in interfacesImplemented)
            {
                bool isAssignableFromOtherInterfaces = false;
                foreach (Type otherInterfaceImplemented in interfacesImplemented)
                {
                    if (interfaceImplemented == otherInterfaceImplemented)
                    {
                        continue;
                    }

                    if (interfaceImplemented.IsAssignableFrom(otherInterfaceImplemented))
                    {
                        isAssignableFromOtherInterfaces = true;
                        break;
                    }
                }
                if (!isAssignableFromOtherInterfaces)
                {
                    nonInheritedInterfaces.Add(interfaceImplemented);
                }
            }
            return nonInheritedInterfaces;
        }
        
        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> data)
        {            
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
           
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }

    }

}
