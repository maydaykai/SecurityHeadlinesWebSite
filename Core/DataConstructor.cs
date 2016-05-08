using APICloud.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class DataConstructor
    {
        public static Factory Factory(string modelName)
        {
            var resouce = new Resource("A6994668718085", "664D29B1-760E-1B76-5871-CEB71FB2D993");
            return resouce.Factory(modelName);
        }
        public static Resource Resource()
        {
            return new Resource("A6994668718085", "664D29B1-760E-1B76-5871-CEB71FB2D993");
        }
    }
}
