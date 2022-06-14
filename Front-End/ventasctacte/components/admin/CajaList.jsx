import { Fragment } from "react";
import { Column, TableWithBrowserPagination, MenuItem } from "react-rainbow-components"
const CajaList = ({ cajas }) => {
    return (
        <Fragment>
            <TableWithBrowserPagination keyField="IdCaja" data={cajas} pageSize={9} >
                <Column header= "Caja" field="Nombre"></Column>
                <Column header= "Cajero" field="Cajero" ></Column>
                <Column header= "Username" field="UserName" ></Column>
                <Column header= "Saldo" field="Saldo" ></Column>
            </TableWithBrowserPagination>

        </Fragment>

    )
}

export default CajaList;