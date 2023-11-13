using DevExpress.Mvvm.UI.Interactivity.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class UserListModel
    {
        public string ChatName { get; set; }
        public string ChatID { get; set; }
        public int unrMessages { get; set; }
    }
}
