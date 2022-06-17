import { Fragment } from "react";
import { Column, TableWithBrowserPagination } from "react-rainbow-components"

const StockList = ({ stocks }) => {
    return (
        <Fragment>
            <TableWithBrowserPagination keyField="Id" data={stocks} pageSize={9} >
                <Column header="Cantidad" field="Cantidad"></Column>
                <Column header="Deposito" field="Deposito" ></Column>
                <Column header="Marca" field="Marca" ></Column>
                <Column header="Producto" field="Producto" ></Column>
                <Column header="Precio" field="PrecioUnitario" ></Column>
                <Column header="Iva" field="Iva" ></Column>
            </TableWithBrowserPagination>
        </Fragment>
    )
}

export default StockList;