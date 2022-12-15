using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectoIntento24.Core
{
    public class ParamList :Expressions
    {
        public List<string> @params { get; set; }

        public ParamList(List<string> @params)
        {
            this.@params = @params;
        }
    }
}
