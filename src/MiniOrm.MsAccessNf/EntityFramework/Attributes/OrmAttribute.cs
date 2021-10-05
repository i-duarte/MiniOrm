using System;

namespace MiniOrm.EntityFramework.Attributes
{
    public class OrmAttribute
        : Attribute
    {
        public string Name { get; set; }
    }
}
