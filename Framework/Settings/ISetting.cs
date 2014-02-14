using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Settings
{
    public interface ISetting
    {
        public void FromString();
        public override string ToString();
    }
}
