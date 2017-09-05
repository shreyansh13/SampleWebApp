using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RISWebApp.Models
{
    public class CategoryProductModel
    {
        public string mCategoryName { get; set; }
        public List<ProductTable> mProduct { get; set; }
    }
}