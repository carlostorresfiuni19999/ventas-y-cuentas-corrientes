import { Fragment } from "react";
import { Column, TableWithBrowserPagination } from "react-rainbow-components"

const StockList = ({ stocks }) => {
    return (
        <Fragment>
            <TableWithBrowserPagination keyField="Id" data={stocks} pageSize={9} >
                <Column header="Cantidad" field="Cantidad"></Column>
                <Column header="Deposito" field="Deposito.NombreDeposito" ></Column>
                <Column header="Marca" field="Producto.MarcaProducto" ></Column>
                <Column header="Producto" field="Producto.NombreProducto" ></Column>
                <Column header="Precio" field="Producto.Precio" ></Column>
                <Column header="Iva" field="Producto.Iva" ></Column>
            </TableWithBrowserPagination>
        </Fragment>
    )
}

export default StockList;