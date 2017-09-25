using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsWatsonTest
{
    public class DATA
    {

        string CNX = ConfigurationManager.ConnectionStrings["Server1ConnectionString"].ConnectionString.ToString();

        // Test.........
        // Reportes Normativos 32A y 32B
        public DataTable D_ListarTickets(string nroticket)
        {
            return SqlHelper.ExecuteDataset(CNX, CommandType.Text, @" SELECT  r.ref_num,  CASE R.status
                                WHEN 'WIP' THEN 'En Proceso'
                                WHEN 'OP' THEN 'Abierto'
                                WHEN 'ATEN' THEN 'Atendido'
                                WHEN 'RE' THEN 'Resuelto'
                                WHEN 'AEUR' THEN 'Espera de Usuario'
                                WHEN 'SOLREJ' THEN 'Rechazado'
                                WHEN 'CL' THEN 'Cerrado'
                                WHEN 'AWTVNDR' THEN 'Espera de Proveedor'
                      ELSE R.status
                      END as 'status',
                    (SELECT DISTINCT Ct.first_name + ' ' + Ct.last_name FROM ca_contact Ct WHERE Ct.inactive = 0 and Ct.contact_uuid = r.assignee) as 'Asignado Actual'
FROM call_req R
   INNER JOIN ca_contact c ON r.assignee = c.contact_uuid
WHERE (R.type = 'R' OR R.TYPE = 'I') AND R.status IN ('OP', 'WIP', 'AEUR', 'AWTVNDR') 
	 and r.ref_num = '" + nroticket + "'").Tables[0];
        }
    }
}
