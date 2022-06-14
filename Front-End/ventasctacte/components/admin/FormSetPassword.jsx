import { Form, Field, Formik, ErrorMessage } from "formik";
import { Fragment } from "react";
import * as Yup from "yup";
import { Col, Row, Button } from "react-bootstrap";

const FormSetPassword = ({username, onChangePassword}) => {
    const initialValues = {
        NewPassword: '',
        ConfirmPassword: ''
    }

    const validationSchema = Yup.object({
        NewPassword : Yup.string()
        .matches(
            /^.*(?=.{8,})((?=.*[!@#$%^&*()\-_=+{};:,<.>]){1})(?=.*\d)((?=.*[a-z]){1})((?=.*[A-Z]){1}).*$/,
            "Por lo menos 8 caracteres, Una Mayuscula y Minuscula, un caracter especial y un numerico"
        )
        .required("Requerido"),
        
        ConfirmPassword: Yup.string()
            .when("Password", {
                is: val => (val && val.length > 0 ? true : false),
                then: Yup.string().oneOf(
                    [Yup.ref("Password")],
                    "No coinciden los password"
                )
            })
    });

    const handleSubmit = (values) => {
        onChangePassword(username,values);
    }

    return(
        <Fragment>
            <Formik
                initialValues={initialValues}
                validationSchema = {validationSchema}
                onSubmit = {handleSubmit}
            >
                <Form>
                <Row>
                        <Col sm={6}>
                            <label htmlFor="NewPassword">New Password</label>
                        </Col>

                        <Col sm={6}>
                            <label htmlFor="ConfirmPassword">Confirm Password</label>
                        </Col>

                    </Row>
                    <Row>
                        <Col sm={6}>
                            <Field name="NewPassword" type="password" />
                        </Col>
                        <Col sm={6}>
                            <Field name="ConfirmPassword" type="password" />
                        </Col>

                    </Row>
                    <Row>

                        <Col sm={6}>
                            <ErrorMessage name="NewPassword" />
                        </Col>

                        <Col sm={6}>
                            <ErrorMessage name="ConfirmPassword" />
                        </Col>
                    </Row>
                    <Button type="submit">Guardar Cambios</Button>
                </Form>

            </Formik>
        </Fragment>
    )

}

export default FormSetPassword;