using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PcoBase;

namespace PcoWeb.Models
{
    public class DienstplanMatrixModel
    {
        public DienstplanMatrixModel()
        {
            this.ServiceTypes = new Collection<string>();
            this.ServiceTypeList = new Collection<ServiceType>();
        }

        [UIHint("Date")]
        [DisplayName("Start-Datum")]
        [DataType(DataType.Date)]
        public DateTime? Start { get; set; }

        [UIHint("Date")]
        [DisplayName("End-Datum")]
        [DataType(DataType.Date)]
        public DateTime? Ende { get; set; }

        public Collection<string> ServiceTypes { get; set; }

        public Collection<ServiceType> ServiceTypeList { get; private set; }

        public IEnumerable<MatrixPlan> Items { get; set; }
    }
}