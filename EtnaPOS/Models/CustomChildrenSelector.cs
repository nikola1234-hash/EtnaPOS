using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtnaPOS.Models
{
    public class CustomChildrenSelector : DevExpress.Xpf.Grid.IChildNodesSelector
    {
        public IEnumerable SelectChildren(object item)
        {
            if(item == null)
            {
                return null;
            }
            if (item is Product)
            {
                if (((Product)item).Products != null && ((Product)item).Products.Count > 0)
                {
                    return ((Product)item).Products;
                }
            }
            //if (item is Category && ((Category)item).Products.Count > 0)
            //    return ((Category)item).Products;
            return null;
            
        }
    }
}
