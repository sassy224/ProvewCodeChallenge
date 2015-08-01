using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proview.CodeChallenge.BLL.ControlObject
{
    public class ResultModel
    {
        public List<System.Object> Result { get; set; }

        public int TotalSize { get; set; }

        public String Status { get; set; }

        public String Error { get; set; }

        public ResultModel(List<System.Object> result, int totalSize)
        {
            this.Result = result;
            this.TotalSize = totalSize;
        }
    }
}
