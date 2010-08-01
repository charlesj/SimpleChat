using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System;

namespace RooChat.Models
{
    public partial class RooChatDataContext
    {
        public RooChatDataContext() :
            base(global::System.Configuration.ConfigurationManager.ConnectionStrings["RooChatConnectionString"].ConnectionString, mappingSource)
        {
            OnCreated();
        }
    }
}
