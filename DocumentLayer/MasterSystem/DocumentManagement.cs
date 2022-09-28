using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentLayer.MasterSystem
{
    public class DocumentManagement : BaseSystemMethod
    {

        private static DocumentManagement instance = null;

        public static DocumentManagement Instance
        {
            get
            {
                if (instance == null)
                    instance = new DocumentManagement();
                return instance;
            }
        }
    }
}
