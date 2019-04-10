using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Czeum.Client.Interfaces {
    interface IInsertable
    {
        void Insert(Panel insertedControl);
    }
}
