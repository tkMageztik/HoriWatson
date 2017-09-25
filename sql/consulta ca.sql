SELECT  r.ref_num,  CASE R.status
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
