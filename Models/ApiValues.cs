using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KentekenApp.Models
{
    public class ApiValues
    {
        public string cilinderinhoud {  get; set; }
        public string kenteken { get; set; }
        public string voertuigsoort { get; set; }
        public string merk { get; set; }
        public string catalogusprijs {  get; set; }
        public string handelsbenaming { get; set; }
        public string datum_tenaamstelling { get; set; }
        public string massa_ledig_voertuig { get; set; }
        public string datum_eerste_toelating { get; set; }
        public string datum_eerste_tenaamstelling_in_nederland { get; set; }
        public string wam_verzekerd { get; set; }
        public string vervaldatum_apk { get; set; }
        public string jaar_laatste_registratie_tellerstand { get; set; }
        public string tellerstandoordeel { get; set; }
        public string code_toelichting_tellerstandoordeel { get; set; }
        public string tenaamstellen_mogelijk { get; set; }
        public DateTime datum_tenaamstelling_dt { get; set; }
        public DateTime datum_eerste_toelating_dt { get; set; }
        public DateTime datum_eerste_tenaamstelling_in_nederland_dt { get; set; }
    }
}
