using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common45.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tuhui.Reception.Model
{
    [Table("tv_busLine")]
    public partial class tv_busLine : BaseEntity
    {
        [Key]
        public int stationID { get; set; }

        public string busGroupID { get; set; }

        public string busLineOrg { get; set; }

        public string lineCode { get; set; }

        public string lineName { get; set; }

        public string upOrDown { get; set; }

        public string upOrDownCode { get; set; }

        public string stationCode { get; set; }

        public string stationName { get; set; }

        public int sort { get; set; }

        public string lat { get; set; }

        public string lng { get; set; }

        public string stationType { get; set; }

        public string stationState { get; set; }

    }
}
