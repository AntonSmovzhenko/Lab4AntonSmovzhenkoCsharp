using Lab4AntonSmovzhenkoCsharp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4AntonSmovzhenkoCsharp.Navigation
{
    internal interface INavigatable
    {
        NavigationTypes ViewType
        {
            get;
        }
    }
}
