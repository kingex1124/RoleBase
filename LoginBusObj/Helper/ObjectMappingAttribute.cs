﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginBusObj.Helper
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ObjectMappingAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public ObjectMappingAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }
    }
}
