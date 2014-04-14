using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RU.NekAlex.Sbis.SDK;

namespace RU.NekAlex.Sbis.Exception
{
        public class SbisJsonErrorException : ApplicationException
        {
            private string response { get; set; }
            public SbisJsonErrorException(string response)
            {
                this.response = response;
            }
            public override string ToString()
            {
                return SBisRequests.GetErrorInfo(this.response).ToString();
                //return base.ToString();
            }
        }
}
