import React from 'react'
import {Formik, Form} from "formik";
import * as Yup from "Yup";
import FormikControl from "./FormikControl"
import styles from '../styles/LogIn.module.css'

function LoginForm(){
    const initialValues ={
        user: '',
        pass: ''
    }

    const validacion = Yup.object({
        user: Yup.string().required('*Obligatorio'),
        //pass: Yup.string().required('*Obligatorio').matches(/^.*(?=.{8,})((?=.*[!@#$%^&*()\-_=+{};:,<.>]){1})(?=.*\d)((?=.*[a-z]){1})((?=.*[A-Z]){1}).*$/,
        //    "La contraseña debe contener al menos 8 letras, uno Mayuscula un numero y una figura")
        pass: Yup.string().required('*Obligatorio')
    })

    //Se debe cambiar a el submit del BDO
    const onSubmit = values =>{
        console.log('Form data', values)
    }

    return(
        <div className = {styles.LoginPanel}>
            <h1 className={styles.letraGrande}>Login</h1>
            <Formik initialValues={initialValues} validationSchema = {validacion} onSubmit={onSubmit}>
                {
                    formik => {
                        return <Form>
                            <FormikControl
                                control = 'input'
                                type = 'text'
                                label = "Usuario"
                                name = 'user'
                            />
                            <FormikControl
                                control = 'input'
                                type = 'password'
                                label = "Contraseña"
                                name = 'pass'
                            />
                            <button type="submit" className={styles.boton} disabled = {!formik.isValid}> Login </button>
                        </Form>
                    }
                }
            </Formik>
        </div>
        
    );
}

export default LoginForm