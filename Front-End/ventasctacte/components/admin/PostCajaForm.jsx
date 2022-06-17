import { Form, Field, Formik, ErrorMessage } from "formik";
import { Fragment } from "react";
import * as Yup from "yup";
import { Col, Row, Button } from "react-bootstrap";

const PostCajaForm = ({ onSave, options }) => {
    const initialValues = {
        UserName: "",
        NombreCaja: "",
        SaldoInicial: 0,
    };

    const validationSchema = Yup.object({
        UserName: Yup.string().email("Formato Email").required("Requerido"),
        NombreCaja: Yup.string().required("Requerido"),
        SaldoInicial: Yup.number().min(0, "No se aceptan valores negativos")
            .required("Requerido")
    });

    const handleSubmit = (values) => onSave(values);

    return (
        <Fragment>
            <Formik
                initialValues={initialValues}
                validationSchema={validationSchema}
                onSubmit={handleSubmit}
            >
                <Form>
                    <Row>
                        <Col sm={12}>
                            <label>Nombre de la Caja</label>
                        </Col>
                    </Row>
                    <Row>
                        <Col sm={12}>
                            <Field name="NombreCaja" type="text" />
                        </Col>
                    </Row>
                    <Row>
                        <Col sm={12}>
                            <ErrorMessage name="NombreCaja" />
                        </Col>
                    </Row>

                    <Row>
                        <Col sm={12}>
                            <label>UserName</label>
                        </Col>
                    </Row>

                    <Row>
                        <Col sm={12}>
                            <Field as="select" name="UserName">
                                <option value="">Seleccionar</option>
                                {options.map(u => <option key={u} value={u}>{u}</option>)}
                            </Field>
                        </Col>
                    </Row>
                    
                    <Row>
                        <Col sm={12}>
                            <ErrorMessage name="UserName"></ErrorMessage>
                        </Col>
                    </Row>

                    <Row>
                        <Col sm={12} >

                            <label>Saldo Inicial</label>
                        </Col>
                    </Row>

                    <Row>

                        <Col sm={12}>
                            <Field type="number" name="SaldoInicial" />
                        </Col>
                    </Row>
                    <Row>

                        <Col sm={12}>
                            <ErrorMessage name="SaldoInicial"></ErrorMessage>
                        </Col>
                    </Row>

                    <Button type="submit">Guardar</Button>

                </Form>

            </Formik>
        </Fragment >
    )

}

export default PostCajaForm;