import { Fragment } from "react";
import { Card } from "react-bootstrap"

const ModalViewPerson = ({person}) => {
    return(
        <Fragment>
            <Card style={{with:"18rem"}}>
                <Card.Body>
                    <Card.Title>
                        Nombre
                    </Card.Title>
                    <Card.Text>
                        {`${person.Nombre} ${person.Apellido}`}
                    </Card.Text>
                    <Card.Title>
                        Username
                    </Card.Title>
                    <Card.Text>
                        {`${person.UserName}`}
                    </Card.Text>
                    <Card.Title>
                        Telefono
                    </Card.Title>
                    <Card.Text>
                        {`${person.Telefono}`}
                    </Card.Text>

                    <Card.Title>
                        Documento
                    </Card.Title>
                    <Card.Text>
                        {`${person.Documento}`}
                    </Card.Text>
                    <Card.Title>
                        Linea de Credito
                    </Card.Title>
                    <Card.Text>
                        {`${person.LineaDeCredito}`}
                    </Card.Text>
                    <Card.Title>
                        Saldo Disponible
                    </Card.Title>
                    <Card.Text>
                        {`${person.Saldo}`}
                    </Card.Text>
                    <Card.Title>
                        Roles
                    </Card.Title>
                    {person.Roles.map(r => <Card.Text key = {r}>{r}</Card.Text>)} 

                </Card.Body>
            </Card>
        </Fragment>
    )
}

export default ModalViewPerson;