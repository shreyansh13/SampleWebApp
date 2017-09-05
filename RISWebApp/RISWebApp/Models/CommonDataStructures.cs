using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RISWebApp.Models
{
    public class CommonDataStructures
    {
        public struct CategoryProducts
        {
            public string mCategoryName;
            public List<ProductTable> mProducts;
        }
    }

}