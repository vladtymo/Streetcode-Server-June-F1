using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Toponyms
{
    public class StreetcodeRecordDTO
    {
        public int StreetcodeId { get; set; }

        public int ToponymId { get; set; }

        public StreetcodeDTO? Streetcode { get; set; }

        public ToponymDTO? Toponym { get; set; }
    }
}
