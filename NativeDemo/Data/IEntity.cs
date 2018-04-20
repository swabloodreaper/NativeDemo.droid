using System;
using System.Collections.Generic;
using System.Text;

namespace NativeDemo.Data
{
    public interface IEntity
    {
        int DbId { get; set; }
      
        DateTime UpdatedOn { get; set; }

      
    }
}
