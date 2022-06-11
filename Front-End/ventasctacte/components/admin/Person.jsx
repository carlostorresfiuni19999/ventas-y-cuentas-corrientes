import { Fragment } from "react/cjs/react.production.min"
import 'bootstrap/dist/css/bootstrap.min.css';


const Person = ({ person, onView, onDelete }) => {
    return (
        <Fragment>
            <tr>
                <td>{person.Nombre}</td>
                <td>{person.Apellido}</td>
                <td>{person.DocumentoTipo}</td>
                <td>{person.Documento}</td>
                <td>{person.UserName}</td>
                <td>
                    <i className="bi bi-archive" onClick={onDelete(person.Id)}></i>
                    <i className="bi bi-eye" onClick={onView(person.Id)}></i>
                </td>
            </tr>
        </Fragment>
    )
}
export default Person;