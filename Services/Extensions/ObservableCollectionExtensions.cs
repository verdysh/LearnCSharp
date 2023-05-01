using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void RemoveRange<T>(this ObservableCollection<T> collection, IEnumerable<T> itemsToRemove)
        {
            foreach(var item in itemsToRemove)
            {
                collection.Remove(item);
            }
        }
    }
}
