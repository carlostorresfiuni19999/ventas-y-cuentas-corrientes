import { Fragment } from "react"
import { Column, TableWithBrowserPagination } from "react-rainbow-components"


const PersonList = ({ personas, onDelete, onView }) => {
    return (
        <Fragment>
            <TableWithBrowserPagination keyField="Id" data={personas} pageSize={9}>
                <Column header ="Nombre" field="Nombre"></Column>
                <Column header ="Apellido" field="Apellido"></Column>
                <Column header ="Tipo Doc" field="DocumentoTipo"></Column>
                <Column header ="DOC" field="Documento"></Column>
                <Column header ="Username" field="UserName"></Column>
            </TableWithBrowserPagination>
        </Fragment>
    )
}

export default PersonList;